using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeaBar.Data;

namespace TeaBar.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _dbcontent;

        //DI注入DB
        public ProductsController(ApplicationDbContext dbContext)
        {
            _dbcontent = dbContext;
        }
        public IActionResult ProductsIntro() //茶飲介紹
        {
            return View();
        }
        public IActionResult OrderProducts() //線上訂餐
        {
            return View();
        }

        public string GetSession() //測試讀取session
        {
            var data = HttpContext.Session.GetString("cartItem");
            return data;
        }
    }
}
