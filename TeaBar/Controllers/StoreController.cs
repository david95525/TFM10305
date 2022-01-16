using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult StoreInfo() //門市據點
        {
            return View();
        }
    }
}
