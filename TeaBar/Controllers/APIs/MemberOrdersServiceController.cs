using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models;
using TeaBar.Models.ViewModels;

namespace TeaBar.Controllers.APIs
{
    public class MemberOrdersServiceController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public MemberOrdersServiceController(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        //訂單紀錄查詢
        [HttpGet]
        [Consumes("application/json")]
        public List<HistoryOrdersViewModel> memberOrdersQry()  //訂單紀錄查詢
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Orders> orders = _dbContext.Orders.Where(o => o.UserID == id).OrderByDescending(o => o.OrderDate).ToList<Orders>();
            List<HistoryOrdersViewModel> data = new List<HistoryOrdersViewModel>();
            
            if (orders == null)
            {
                return data = null;
            }
            else
            {
                foreach (Orders item in orders)
                {
                    var od = _dbContext.OrderDetails.Where(od => od.OrderID == item.OrderID).FirstOrDefault();
                    var p = _dbContext.Products.Where(p => p.ProductID == od.ProductID).FirstOrDefault();
                    var c = _dbContext.Categories.Where(c => c.CategoryID == p.CategoryID).FirstOrDefault();
                    var s = _dbContext.Stores.Where(s => s.StoreID == c.StoreID).FirstOrDefault();
                    var storename = s.StoreName;
                    var historyOrders = new HistoryOrdersViewModel
                    {
                        OrderID = item.OrderID,
                        OrderDate = item.OrderDate,
                        StoreName = storename
                    };
                    data.Add(historyOrders);
                }
            }
            return data;
        }


        //訂單明細查詢
        [HttpGet]
        [Consumes("application/json")]
        public List<HistoryOrderDetailsViewModel> OrderDetailsQry([FromRoute] string id)  //訂單明細查詢
        {
            List<OrderDetails> orderdetail = _dbContext.OrderDetails.Where(od => od.OrderID == id).ToList<OrderDetails>();
            List<HistoryOrderDetailsViewModel> data = new List<HistoryOrderDetailsViewModel>();

            foreach (OrderDetails item in orderdetail)
            {
                var productname = _dbContext.Products.Where(p => p.ProductID == item.ProductID).Select(n => n.ProductName).FirstOrDefault().ToString();
                var result = new HistoryOrderDetailsViewModel
                {
                    ProductName = productname,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Note = item.Note,
                    Customization = item.Customization,
                    TotalPrice = item.TotalPrice
                };
                data.Add(result);
            }

            return data;
        }
    }
}
