using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models;
using TeaBar.Models.ViewModels;

namespace TeaBar.Controllers.APIs
{
    public class MemberServiceController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUsers> _userManager;
        public MemberServiceController(ApplicationDbContext _dbContext, UserManager<ApplicationUsers> _userManager)
        {
            this._dbContext = _dbContext;
            this._userManager = _userManager;
        }

        //會員資料查詢
        [HttpGet]
        [Produces("application/json")]
        public string memberProfileQry()   //會員資料查詢
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resultAU = (from u in _dbContext.ApplicationUsers
                         where u.Id == id
                         select u).FirstOrDefault();
            var resultUC = (from c in _dbContext.UserOfCustomer
                            where c.UserID == id
                            select c).FirstOrDefault();

            var member = new RegisterOfMemberViewModel
            {
                Email = resultAU.Email,
                PhoneNumber = resultAU.PhoneNumber,
                Password = resultAU.PasswordHash,

                LastName = resultUC.LastName,
                FirstName = resultUC.FirstName,
                Birth = resultUC.Birth,
                City = resultUC.City,
                District = resultUC.District,
                Address = resultUC.Address,
                Gender = resultUC.Gender
            };            
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(member);

            return data;
        }


        //會員資料修改作業
        [HttpPut]
        [Produces("application/json")]
        [Consumes("application/json")]
        public string memberProfileUpdate([FromBody] RegisterOfMemberViewModel member)    //會員資料修改
        {
            string msg = null;

            var qryResult = (from c in _dbContext.ApplicationUsers
                             where c.Email == member.Email
                             select c).FirstOrDefault();
            var ucResult = (from u in _dbContext.UserOfCustomer
                            where u.UserID == qryResult.Id
                            select u).FirstOrDefault();

            if (qryResult == null)
            {
                msg = $"此帳號：{member.Email}不存在!";
            }
            else
            {
                _dbContext.Entry(qryResult).CurrentValues.SetValues(member);
                _dbContext.Entry(ucResult).CurrentValues.SetValues(member);
                try
                {
                    _dbContext.SaveChanges();
                    msg = "資料更新成功！";
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex);
                    msg = "資料更新失敗...";
                }
            }
            return msg;
        }

        //密碼變更作業(共用)
        [HttpPut]
        public async Task<string> membersPwdUpdate([FromBody] MemberPwdViewModel pwd)   //密碼變更
        {
            string msg = null;

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var qryResult = (from c in _dbContext.ApplicationUsers
                             where c.Id == id
                             select c).FirstOrDefault();
            if (qryResult == null)
            {
                msg = $"此帳號:{qryResult.Email}不存在!";
            }

            var result = await _userManager.ChangePasswordAsync(qryResult, pwd.OldPassword, pwd.NewPassword);
            if (result.Succeeded)
            {
                msg = "密碼變更成功！";
            }
            else
            {
                msg = "輸入資料有誤...請重新輸入";
            }
            return msg;
        }
    }
}
