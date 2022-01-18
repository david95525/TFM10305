using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaBar.Controllers
{
    public class ProductManageChartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StoreCategory()
        {
            return View();
        }

        public IActionResult ProductOrder()
        {
            return View();
        }

        public IActionResult StoreOrder()
        {
            return View();
        }

        public IActionResult StoreProduct()
        {
            return View();
        }

    }
}
