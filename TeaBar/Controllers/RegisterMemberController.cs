using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public RegisterMemberController(ApplicationDbContext _dbContext, UserManager<ApplicationUsers> _userManager, ILogger<RegisterMemberController> _logger, IPasswordHasher<ApplicationUsers> _passwordHasher)
        {
            this._dbContext = _dbContext;
            this._userManager = _userManager;
            this._logger = _logger;
            this._passwordHasher = _passwordHasher;
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

                    _dbContext.ApplicationUsers.Add(user);

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Index","Home");
        }
    }
}
