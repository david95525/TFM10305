using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models;
using TeaBar.Models.ViewModels;

namespace TeaBar.Controllers
{
    public class RegisterMemberController : Controller
    {
        private ApplicationDbContext _dbContext;
        private UserManager<ApplicationUsers> _userManager;
        private ILogger<RegisterMemberController> _logger;
        private IPasswordHasher<ApplicationUsers> _passwordHasher;
        private readonly IEmailSender _emailSender;


        public RegisterMemberController(ApplicationDbContext _dbContext, UserManager<ApplicationUsers> _userManager, ILogger<RegisterMemberController> _logger, IPasswordHasher<ApplicationUsers> _passwordHasher,IEmailSender emailSender)//要+,IEmailSender emailSender
        {
            this._dbContext = _dbContext;
            this._userManager = _userManager;
            this._logger = _logger;
            this._passwordHasher = _passwordHasher;
            _emailSender = emailSender;

        }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email欄位不能空白")]
            [EmailAddress(ErrorMessage = "請輸入正確Email格式")]
            public string Email { get; set; }
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> memberRegister(RegisterOfMemberViewModel customer)
        {
            if (ModelState.IsValid)
            {
                //建立Member資料
                var user = new ApplicationUsers
                {
                    UserName = customer.Email,
                    NormalizedUserName = customer.Email,
                    Email = customer.Email,
                    NormalizedEmail = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    PasswordHash=customer.Password,

                    UserOfCustomer = new UserOfCustomer
                    {                        
                        LastName = customer.LastName,
                        FirstName = customer.FirstName,
                        Birth = customer.Birth,
                        Gender = customer.Gender,
                        City = customer.City,
                        District = customer.District,
                        Address = customer.Address
                    }
                };
                var result = await _userManager.CreateAsync(user, customer.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = "https://localhost:44374/Identity/Account/ConfirmEmail?userId=" + userId + "&code=" + code;
                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Email驗證",
                        $"請點選此連結開通帳號 <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>開通連結</a>.");

                    ModelState.AddModelError(string.Empty, "確認信已發送，請至您的信箱確認。");
                    return View();

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            return RedirectToAction("Index","Home");
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> RegisterEmailConfirm(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return View();
        }

    }
}
