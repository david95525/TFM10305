using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles = "Manager")]
    public class RegisterStoreManagerController : Controller
    {
        private ApplicationDbContext _dbContext;
        private UserManager<ApplicationUsers> _userManager;
        private ILogger<RegisterStoreManagerController> _logger;
        private IPasswordHasher<ApplicationUsers> _passwordHasher;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterStoreManagerController(ApplicationDbContext _dbContext, UserManager<ApplicationUsers> _userManager, ILogger<RegisterStoreManagerController> _logger, IPasswordHasher<ApplicationUsers> _passwordHasher, RoleManager<IdentityRole> _roleManager)
        {
            this._dbContext = _dbContext;
            this._userManager = _userManager;
            this._logger = _logger;
            this._passwordHasher = _passwordHasher;
            this._roleManager = _roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> storeManagerRegister(RegisterOfStoreManagerViewModel store)    //門市註冊
        {
            var storeUser = new ApplicationUsers
            {
                UserName = store.Email,
                NormalizedUserName = store.Email,
                Email = store.Email,
                NormalizedEmail = store.Email,
                PasswordHash = store.Password,

                Stores = new Stores
                {
                    StoreName = store.StoreName,
                    StoreAddress = store.StoreAddress,
                    StorePhone = store.StorePhone,
                    StoreCity = store.StoreCity,
                    StoreDistrict = store.StoreDistrict
                }
            };

            var result = await _userManager.CreateAsync(storeUser, store.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("StoreUser created a new account with password.");

                //Adding Admin Role
                var roleCheck = await _roleManager.RoleExistsAsync("Admin");
                if (!roleCheck)
                {
                    //建立角色
                    IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                await _userManager.AddToRoleAsync(storeUser, "Admin");

                _dbContext.ApplicationUsers.Add(storeUser);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return RedirectToAction("Index", "Home");
        }


        public IActionResult ManagerIndex()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> taebarManagerRegister(ApplicationUsers teabarmanager)    //總公司註冊
        {
            var TBmanager = new ApplicationUsers
            {
                UserName = teabarmanager.Email,
                NormalizedUserName = teabarmanager.Email,
                Email = teabarmanager.Email,
                NormalizedEmail = teabarmanager.Email,
                PasswordHash = teabarmanager.PasswordHash
            };

            var result = await _userManager.CreateAsync(TBmanager, teabarmanager.PasswordHash);
            if (result.Succeeded)
            {
                _logger.LogInformation("Manager created a new account with password.");

                //Adding Manager Role
                var roleCheck = await _roleManager.RoleExistsAsync("Manager");
                if (!roleCheck)
                {
                    //建立角色
                    IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole("Manager"));
                }
                await _userManager.AddToRoleAsync(TBmanager, "Manager");

                _dbContext.ApplicationUsers.Add(TBmanager);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
