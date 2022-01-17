using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models.ViewModels;

namespace TeaBar.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductsServiceController : ControllerBase
    {
        //注入db
        private readonly ApplicationDbContext _dbcontext;
        public OrderProductsServiceController(ApplicationDbContext dbContext)
        {

            _dbcontext = dbContext;

        }

        [HttpPost]
        [Route("orderDetail")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public IActionResult AddToCart([FromBody] OrderProductsViewModel data) //取得產品資料
        {
            try
            {
                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                //存入session
                HttpContext.Session.SetString("cartItem", jsonstring);

            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }
            return Ok();
        }

    }
}
