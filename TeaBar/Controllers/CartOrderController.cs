using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TeaBar.Data;
using TeaBar.Models;


namespace TeaBar.Controllers
{
    public class CartOrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        //注入進來的logger物件
      

        //建構子注入
        public CartOrderController(ApplicationDbContext context)
        {
            _db = context; 
        }
        public IActionResult ShoppingCart()
        {
            if (User.Identity.Name == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            //模擬購物車加入商品
            int product1id = 2;
            int product2id = 3;
            int product3id = 4;
            Random discountrandom = new Random();
            int discountid = discountrandom.Next(1, 11);
            string userName = User.Identity.Name;//當前identity中使用者名稱
            Products product1 = _db.Products.FirstOrDefault(p => p.ProductID == product1id);
            Products product2 = _db.Products.FirstOrDefault(p => p.ProductID == product2id);
            Products product3 = _db.Products.FirstOrDefault(p => p.ProductID == product3id);
            Discount discount = _db.Discount.FirstOrDefault(d => d.DiscountID == discountid);
            Categories categorie1 = _db.Categories.FirstOrDefault(c => c.CategoryID == product1.CategoryID);
            Stores store = _db.Stores.FirstOrDefault(s => s.StoreID == categorie1.StoreID);
            //存入session
            List<CartViewModel> Carts = new List<CartViewModel>
            {
               new CartViewModel()
               {
                   UserID=userName,
                   ProductId=product1.ProductID,
                   ProductName=product1.ProductName,
                   UnitPrice=product1.UnitPrice,
                   Quantity=2,
                   DiscountId=discount.DiscountID,
                   Discount=discount.DiscountRule,
                   Sweetness="半糖",
                   Ice="去冰",
                   Ingredient="珍珠",
                   Picture=product1.Picture,
                   Note="無",
                   Size="大",
                   StoreID=store.StoreID
               },
               new CartViewModel()
               {
                   UserID=userName,
                   ProductId=product2.ProductID,
                   ProductName=product2.ProductName,
                   UnitPrice=product2.UnitPrice,
                   Quantity=1,
                   DiscountId=discount.DiscountID,
                   Discount=discount.DiscountRule,
                   Sweetness="全糖",
                   Ice="少冰",
                   Ingredient="布丁",
                   Picture=product2.Picture,
                   Note="無",
                   Size="中",
                   StoreID=store.StoreID
               },
                             new CartViewModel()
               {
                   UserID=userName,
                   ProductId=product3.ProductID,
                   ProductName=product3.ProductName,
                   UnitPrice=product3.UnitPrice,
                   Quantity=1,
                   DiscountId=discount.DiscountID,
                   Discount=discount.DiscountRule,
                   Sweetness="微糖",
                   Ice="去冰",
                   Ingredient="無",
                   Picture=product3.Picture,
                   Note="無",
                   Size="中",
                   StoreID=store.StoreID
               }
           };

            string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(Carts);
            //存入sessions
            HttpContext.Session.SetString(userName, jsonstring);
            HttpContext.Session.SetString("username", userName);
            return View();
            //模擬前面存入cookie
        }
 

        public IActionResult OrderSteps()
        {
            if (User.Identity.Name == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            return View();
        }

    }
}
