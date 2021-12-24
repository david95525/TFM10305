using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TFM10305.Models;

namespace TFM10305.Controllers
{
    public class CartOrderController : Controller
    {
    
        public IActionResult Index()
        {
            return View();
        }
   
        public IActionResult OrderStep1()
        {
            string SessionValue = null;
            if (HttpContext.Session.Keys.Contains("carts"))
            {
                SessionValue = HttpContext.Session.GetString("carts");

            }
            List<Cart> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cart>>(SessionValue);
            ViewBag.cart=data;
            return View();
        }
    }
}
