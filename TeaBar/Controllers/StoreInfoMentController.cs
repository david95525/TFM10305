using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Controllers
{
    public class StoreInfoMentController : Controller
    {
        public IActionResult StoreInfoMent()
        {
            return View();
        }
    }
}
