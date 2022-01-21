using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StoreInfoMentController : Controller
    {
        public IActionResult StoreInfoMent()
        {
            return View();
        }
    }
}
