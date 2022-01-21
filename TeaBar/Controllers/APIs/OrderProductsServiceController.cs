using Microsoft.AspNetCore.Authorization;
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
            string userName = "";
            if (User.Identity.Name == null)
            {
                userName = "temp";
            }
            else
            {
                userName = User.Identity.Name;//當前identity中使用者名稱
                HttpContext.Session.SetString("username", userName);
            }
         
         
            Products product = _dbcontext.Products.FirstOrDefault(p => p.ProductID == data.productID);
            Categories categories = _dbcontext.Categories.FirstOrDefault(c => c.CategoryID == product.CategoryID);
              //決定discount
            List <Discount> discounts = _dbcontext.Discount.ToList();
            Discount discount = discounts[0];
    
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
                Picture = product.Picture,
                Note = "無",
                Size = data.size.name,
                StoreID = categories.StoreID,
                Ingredient=new List<string>()
            };
            
                for (int i = 0; i < data.ingredient.Length; i++)
            {
                string x = data.ingredient[i].name;
                cart.Ingredient.Add(x);
            }
            int index = 0;
            //讀舊的
            if (HttpContext.Session.Keys.Contains(userName))
            {
                string oldcart= HttpContext.Session.GetString(userName);
                carts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartViewModel>>(oldcart);
                if(carts[0].StoreID==cart.StoreID)
                {
                    //找出購物車裡是否加入過同樣商品
                    CartViewModel temp = carts.FirstOrDefault(c => c.ProductId == cart.ProductId);
                    //否的話 直接新增
                    if (temp == null)
                    {
                        carts.Add(cart);
                    }
                    else if (temp.Ingredient != cart.Ingredient||temp.Ice!=cart.Ice
                        ||temp.Size!=cart.Size||temp.Sweetness!=cart.Sweetness)
                    {
                        carts.Add(cart);
                    }
                    else  //是的話 原來數量+選購數量
                    {
                        index = carts.IndexOf(temp);
                        carts[index].Quantity = carts[index].Quantity + cart.Quantity;
                        carts[index].Subtotal = carts[index].Subtotal + cart.Subtotal;
                    }
                }
                else
                {
                    carts.Clear();
                    carts.Add(cart);
                }              
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
