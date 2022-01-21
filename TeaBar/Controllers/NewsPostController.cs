using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Controllers
{
    public class NewsPostController : Controller
    {
        //發布最新消息
        [Authorize(Roles = "Manager")]
        public IActionResult pubNews()
        {
            return View();
        }

        //訂閱最新消息
        public IActionResult showNews()
        {
            return View();
        }
    }
}
