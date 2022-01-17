using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models;

namespace TeaBar.Controllers
{
    public class OrderAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        //注入進來的logger物件
        private readonly ILogger<OrderAPIController> _logger;
      
        //建構子注入
        public OrderAPIController(ApplicationDbContext context, ILogger<OrderAPIController> logger)
        {
            _db = context;_logger = logger;    
        }
        //讀cookie中店家
        //讀session店家
        [HttpGet]
        [ProducesAttribute("application/json")]
        public Stores GetStore()
        #region 讀店家
        {
            string username = User.Identity.Name;
            //if (HttpContext.Request.Cookies[username] != null)
             if(HttpContext.Session.Keys.Contains(username))
            {
                //string Carts = HttpContext.Request.Cookies[username];
                string Carts = HttpContext.Session.GetString(username);
                List<CartViewModel> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartViewModel>>(Carts);
               string storeid = data[0].StoreID;
                try
                {
                    Stores storenow =
                        _db.Stores.FirstOrDefault(s => s.StoreID == storeid);
                    return storenow;
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                    return null;
                }
            }
            return null;
        }
        #endregion
        
        //購物車資料存入來修改刪除cookie
        //購物車資料存入來修改刪除session
        [HttpPost]
        [Consumes("application/json")]
        public Message Savecart([FromBody] List<CartViewModel> carts)
        #region 購物車資料儲存更新
        {
            Message msg = new()
            {
                Code = 200,
                Msg ="cookie刪除",
                Now = DateTime.Now
            };
            if (carts!=null)
            {
                string username = User.Identity.Name;
                try
                {
                    string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(carts);
                    #region 存cookie
                    //CookieOptions option = new CookieOptions();
                    //option.Expires = DateTime.Now.AddDays(5);
                    //HttpContext.Response.Cookies.Append(Resources.cartcookie, jsonstring, option);
                    //msg.Msg = "成功存入cookie";

                    #endregion
                    #region 存session
                    HttpContext.Session.SetString(username, jsonstring);
                    msg.Msg = "成功存入session";
                    #endregion
                }
                catch
                {
                    msg.Code = 500;
                    msg.Msg = "失敗";
                    return msg;
                }

            }
            
            return msg;
        }
        #endregion
        //讀取購物車cookie
        //讀取購物車session
        [HttpGet]
        [ProducesAttribute("application/json")]
        public List<CartViewModel> Readcart()
        #region 購物車資料讀取
        {
            if (HttpContext.Session.Keys.Contains("username"))
            { string username = HttpContext.Session.GetString("username");
                #region 讀session
                if (HttpContext.Session.Keys.Contains(username))
                {
                    string jsonstring = HttpContext.Session.GetString(username);
                    List<CartViewModel> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartViewModel>>(jsonstring);
                    return data;
                }
                #endregion
            }

                #region 讀cookie
                //if (HttpContext.Request.Cookies[username] != null)
                //{
                //    string jsonstring = HttpContext.Request.Cookies[username];             
                //      List <CartViewModel> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartViewModel>>(jsonstring);
                //    return data;
                //}
                #endregion

                return null;
        }
        #endregion
        //寫入order資料庫
        [HttpGet]
        public async Task<Message> Saveorder()
        #region order寫入資料庫
        {
            // 建立回傳訊息物件
            Message msg = new Message
            {
                Code = 200,
                Msg = "儲存成功",
                Now = DateTime.Now
            };
            string username = User.Identity.Name;
            //if (HttpContext.Request.Cookies[username] != null)
                if(HttpContext.Session.Keys.Contains(username))
            {
                #region 讀cart
                //string Carts = HttpContext.Request.Cookies[username];
                string Carts = HttpContext.Session.GetString(username);
                List<CartViewModel> carts = Newtonsoft.Json.JsonConvert.
                    DeserializeObject<List<CartViewModel>>(Carts);
                #endregion
                #region 存入order表
                //台北時間
                DateTimeOffset taipeiTime = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));
                //找出order屬性

                string userid = _db.Users.Where(s => s.UserName == username).
                    Select(s => s.Id).FirstOrDefault();
                string storeid = HttpContext.Request.Cookies[username];
                string orderid =carts[0].OrderID;
                int discountid = carts[0].DiscountId;
                //建立Order物件
                Orders order = new Orders
                {
                    OrderID = orderid,
                    UserID = userid.ToString(),
                    DiscountID = discountid,
                    OrderDate = taipeiTime.ToString(),
                };
                //嘗試存入資料庫
                try
                {
                    _db.Orders.Add(order);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    msg.Msg = ex.ToString();
                    return msg;
                }
                #endregion
                #region 存入OrderDetails 表
               
                int index = 1;
                foreach (var item in carts)
                {
                    if(item.Ingredient=="布丁"|| item.Ingredient == "仙草凍" || item.Ingredient == "芋圓")
                    {
                        item.Ingredient = item.Ingredient + "+10元";
                    }
                    //建立orderdetails集合
                    OrderDetails orderdetail = new OrderDetails
                    {
                        TotalPrice=item.Subtotal,
                        OrderID=orderid,
                        ProductID = item.ProductId,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        Note = item.Note,
                        Customization = "甜度:" + item.Sweetness + "冰塊:" + item.Ice + "加料:" + item.Ingredient
                    };
                    index++;
                    try
                    {
                        _db.OrderDetails.Add(orderdetail);
                        await _db.SaveChangesAsync();
                    }
                    catch
                    {
                        msg.Msg = "儲存失敗";
                        return msg;
                    }
                }
                #endregion
            }
            return msg;
        }
        #endregion


    }
}
