using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models;
using TeaBar.Models.OrderViewModels;

namespace TeaBar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderServiceController : Controller
    {
        private readonly ApplicationDbContext _dBContext;
        private IWebHostEnvironment _env;

        public OrderServiceController(ApplicationDbContext dBContext, IWebHostEnvironment env)
        {
            _dBContext = dBContext;
            _env = env;
        }

        [HttpGet]
        [Route("Showorder")]
        [Produces("application/json")]
        public List<Orders> Showorder()
        {
            List<Orders> orderresult = _dBContext.Orders.ToList();

            return orderresult;
        }

        //[HttpPost]
        //[Route("Save/{id?}")]
        //[Produces("application/json")]
        //[ValidateAntiForgeryToken]
        //public bool Save(int orderDetails)
        //{
        //    OrderDetails updateOrderDetails = (from item in _dBContext.OrderDetails
        //                                       where item.OrderID == orderDetails
        //                                       select item).FirstOrDefault();
        //    if (updateOrderDetails != null)
        //    {
        //        updateOrderDetails.Quantity = orderDetails;
                
        //        _dBContext.SaveChanges();
        //        return true;
        //    }
        //    return false;

        //}


        [HttpPost("{id}")]
        [Route("Save/{id?}")]
        [Produces("application/json")]
        //[ValidateAntiForgeryToken]
        public IActionResult Save(string id)
        {
            string orderDetails = id;
            OrderDetails updateOrderDetails = (from item in _dBContext.OrderDetails
                                               where item.OrderID == orderDetails
                                               select item).FirstOrDefault();
            if (updateOrderDetails != null)
            {
                

                _dBContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        private bool OrderDetailExists(string id)
        {
            return _dBContext.OrderDetails.Any(e => e.OrderID == id);
        }

        [HttpGet]
        [Route("Detail/{id}")]
        [Produces("application/json")]
        public List<Orderview> Detail(string id)
        {

            var result = (from s in _dBContext.Orders
                          join t in _dBContext.OrderDetails on s.OrderID equals t.OrderID
                          join u in _dBContext.Products on t.ProductID equals u.ProductID
                          where s.OrderID == id
                          select new Orderview
                          {
                              OrderDetailID = t.OrderDetailID,
                              OrderID = t.OrderID,
                              ProductName = u.ProductName,
                              UnitPrice = t.UnitPrice,
                              Quantity = t.Quantity,
                              Note = t.Note,
                              Customization = t.Customization,
                          }).ToList();
            return result;
        }


        [HttpGet("{id}")]
        [Route("Product/{id}")]
        [Produces("application/json")]
        public List<Products> Product(int id)
        {
            //從詳細訂單查詢訂單ID一樣的資料
            var Productresult = (from p in _dBContext.Products where p.ProductID == id select p).ToList();

            return Productresult;
        }

        [HttpDelete("{id}")]
        [Route("Delete/{id}")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(int id)
        {
            var orders = await _dBContext.Orders.FindAsync(new object[] { id });
            _dBContext.Orders.Remove(orders);
            await _dBContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
