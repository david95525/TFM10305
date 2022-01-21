using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeaBar.Data;

namespace TeaBar.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public MembersController(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public IActionResult MemberProfile()    //會員資料修改頁面
        {
            return View();
        }

        public IActionResult MemberPassword()   //會員密碼變更頁面
        {
            return View();
        }

        public IActionResult OrdersHistory()    //歷史訂單頁面
        {
            return View();
        }
    }
}
