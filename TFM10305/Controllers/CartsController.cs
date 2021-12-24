using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFM10305.Models;

namespace TFM10305.Controllers
{
    public class CartsController : Controller
    {

        private readonly TFM10305DBContext _db;

        public CartsController(TFM10305DBContext _context)
        {
            _db = _context;
            Console.WriteLine("注入" + _db);
        }
        public IActionResult ShoppingCart()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        
        //抓取購物車資料
        [HttpGet]
        [ProducesAttribute("application/json")]
        public List<Models.Cart> GetCart()
        {
            string cid = "John001";
            List<Cart> carts 
              = _db.Carts.Where(c => c.CustomerId == cid).Select(c => c).ToList<Cart>();
            foreach(Cart item in carts)
            {
                Console.WriteLine(item);
            }
            return carts;
        }

        //更新購物車資料

        //刪除購物車資料
        [HttpDelete]
        [Produces("application/json")]
        public Models.Message cartDelete
            ([FromQuery(Name = "cid")] string customerid, [FromQuery(Name = "pid")] string productid)
        {
            Models.Message msg = null;
            Models.Cart cart = _db.Carts.Where(c => c.CustomerId == customerid && c.ProductId==productid)
                .Select(c => c).FirstOrDefault();
           
            if (cart != null)
            {
                _db.Carts.Remove(cart);
                try
                {
                    _db.SaveChanges();
                    msg = new Models.Message
                    {
                        Code = 200,
                        Msg = $"{cart.ProductName} 刪除成功!!!",
                        Now = DateTime.Now
                    };
                }
                catch (DbUpdateException ex)
                {
                    msg = new Models.Message
                    {
                        Code = 200,
                        Msg = $"{ cart.ProductName }刪除不成功!!!{ex.Message}",
                        Now = DateTime.Now
                    };
                }
            }
            else
            {
                msg = new Models.Message()
                {
                    Code = 200,
                    Msg ="此產品已經不存在了!!!",
                    Now = DateTime.Now
                };
            }
            return msg;
        }
    }
}
