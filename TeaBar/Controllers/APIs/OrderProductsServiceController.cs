using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models;
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
            if (User.Identity.Name == null)
            {
                return null;
            }
            string userName = User.Identity.Name;//當前identity中使用者名稱
            HttpContext.Session.SetString("username", userName);
         
            Products product = _dbcontext.Products.FirstOrDefault(p => p.ProductID == data.productID);
            Categories categories = _dbcontext.Categories.FirstOrDefault(c => c.CategoryID == product.CategoryID);
              //隨機決定discount
            List <Discount> discounts = _dbcontext.Discount.ToList();
            Random r = new Random();
            int index= r.Next(0, 10);
            Discount discount = discounts[index];
            // 轉換成購物車viewmodel
            List<CartViewModel> carts = new List<CartViewModel>();
            CartViewModel cart= new CartViewModel()
            {
                UserID = userName,
                ProductId = product.ProductID,
                ProductName = product.ProductName,
                UnitPrice = data.size.price,
                Quantity = data.quentity,
                Subtotal=data.subtotal,
                DiscountId = discount.DiscountID,
                Discount = discount.DiscountRule,
                Sweetness = data.sweetness,
                Ice = data.ice,
                Ingredient = data.ingredient[0].name,
                Picture = product.Picture,
                Note = "無",
                Size = data.size.name,
                StoreID = categories.StoreID
            };

            //讀舊的
            if (HttpContext.Session.Keys.Contains(userName))
            {
                string oldcart= HttpContext.Session.GetString(userName);
                carts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartViewModel>>(oldcart);
                carts.Add(cart);
            }
            else
            //在空的購物車填入
            {
                carts.Add(cart);
            }
            try
            {
                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(carts);
                //存入session
                HttpContext.Session.SetString(userName, jsonstring);

            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }
        
            return Ok();
        }

    }
}
