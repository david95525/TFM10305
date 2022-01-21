using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Manager")]
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
        [Route("Ordercount")]
        [Produces("application/json")]
        public int Ordercount()
        {
            var countorder = (from i in _dBContext.Orders select i).Count();
            var countten = countorder / 10;
            var count = countorder % 10;

            if (count == 0)
            {
                return countten;
            }else if(countten >= 1 && count != 0)
            {
                return countten + 1;
            }else
            {
                return 1;
            }

        }


        [HttpGet]
        [Route("Search/{text}")]
        [Produces("application/json")]
        public  List<TopOrederview> Search(string text)
        {
            
                List<TopOrederview> Orderlist = (from p in _dBContext.UserOfCustomer
                                                 join q in _dBContext.Orders on p.UserID equals q.UserID
                                                 join r in _dBContext.Discount on q.DiscountID equals r.DiscountID
                                                 join t in _dBContext.OrderDetails on q.OrderID equals t.OrderID
                                                 join u in _dBContext.Products on t.ProductID equals u.ProductID
                                                 join v in _dBContext.Categories on u.CategoryID equals v.CategoryID
                                                 join w in _dBContext.Stores on v.StoreID equals w.StoreID
                                                 where p.FirstName == text || p.LastName == text || q.OrderID == text ||
                                                 w.StoreName == text
                                                 select new TopOrederview
                                                 {
                                                     OrderID = t.OrderID,
                                                     FirstName = p.FirstName,
                                                     LastName = p.LastName,
                                                     StoreName = w.StoreName,
                                                     OrderDate = q.OrderDate,
                                                     DiscountRule = r.DiscountRule,

                                                 }).Distinct().ToList();
                return Orderlist;
           
            
        }


        [HttpGet]
        [Route("Showorder1")]
        [Produces("application/json")]
        public List<TopOrederview> Showorder1()
        {

            List<TopOrederview> Orderlist = (from p in _dBContext.UserOfCustomer
                                             join q in _dBContext.Orders on p.UserID equals q.UserID
                                             join r in _dBContext.Discount on q.DiscountID equals r.DiscountID
                                             join t in _dBContext.OrderDetails on q.OrderID equals t.OrderID
                                             join u in _dBContext.Products on t.ProductID equals u.ProductID
                                             join v in _dBContext.Categories on u.CategoryID equals v.CategoryID
                                             join w in _dBContext.Stores on v.StoreID equals w.StoreID
                                             select new TopOrederview
                                             {
                                                 OrderID = t.OrderID,
                                                 FirstName = p.FirstName,
                                                 LastName = p.LastName,
                                                 StoreName = w.StoreName,
                                                 OrderDate = q.OrderDate,
                                                 DiscountRule = r.DiscountRule,

                                             }).Distinct().ToList();
            return Orderlist;
        }

        [HttpGet]
        [Route("Showorder/{pageSize?}/{pageIndex}")]
        [Produces("application/json")]
        public List<TopOrederview> Showorder(int pageIndex, int pageSize)
        {

            List<TopOrederview> Orderlist = (from p in _dBContext.UserOfCustomer
                                             join q in _dBContext.Orders on p.UserID equals q.UserID
                                             join r in _dBContext.Discount on q.DiscountID equals r.DiscountID
                                             join t in _dBContext.OrderDetails on q.OrderID equals t.OrderID
                                             join u in _dBContext.Products on t.ProductID equals u.ProductID
                                             join v in _dBContext.Categories on u.CategoryID equals v.CategoryID
                                             join w in _dBContext.Stores on v.StoreID equals w.StoreID
                                             select new TopOrederview
                                             {
                                                 OrderID = t.OrderID,
                                                 FirstName = p.FirstName,
                                                 LastName = p.LastName,
                                                 StoreName = w.StoreName,
                                                 OrderDate = q.OrderDate,
                                                 DiscountRule = r.DiscountRule,

                                             }).Distinct().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Orderlist;

        }


        //[HttpGet]
        //[Route("Showordertest")]
        //[Produces("application/json")]
        //public List<TopOrederview> Showordertest()
        //{

        //    List<TopOrederview> Orderlist = (from p in _dBContext.UserOfCustomer
        //                                     join q in _dBContext.Orders on p.UserID equals q.UserID
        //                                     join r in _dBContext.Discount on q.DiscountID equals r.DiscountID
        //                                     join t in _dBContext.OrderDetails on q.OrderID equals t.OrderID
        //                                     join u in _dBContext.Products on t.ProductID equals u.ProductID
        //                                     join v in _dBContext.Categories on u.CategoryID equals v.CategoryID
        //                                     join w in _dBContext.Stores on v.StoreID equals w.StoreID
        //                                     select new TopOrederview
        //                                     {
        //                                         OrderID = t.OrderID,
        //                                         FirstName = p.FirstName,
        //                                         LastName = p.LastName,
        //                                         StoreName = w.StoreName,
        //                                         OrderDate = q.OrderDate,
        //                                         DiscountRule = r.DiscountRule,

        //                                     }).Distinct().ToList();

        //    return Orderlist;

        //}

        [HttpPost]
        [Route("Save")]
        [Produces("application/json")]
        //[ValidateAntiForgeryToken]
        public IActionResult Save(List<Orderview> order)
        {

            try
            {
                foreach (Orderview item in order)
                {
                    var updateOrderDetails = (from i in _dBContext.OrderDetails
                                              where i.OrderID == item.OrderID
                                              && i.OrderDetailID == item.OrderDetailID
                                              select i);
                    foreach (var i in updateOrderDetails)
                    {
                        i.Quantity = item.Quantity;
                        i.Note = item.Note;
                    }
                    _dBContext.SaveChanges();

                }

            }
            catch (Exception e)
            {
                string b = e.ToString();
            }


            return RedirectToAction("Index");
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
                          }).Distinct().ToList();
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

        [HttpDelete]
        [Route("Delete/{id}")]
        [Produces("application/json")]
        public void Delete(string id)
        {

            List<OrderDetails> result = (from d in _dBContext.OrderDetails where d.OrderID == id select d).ToList();

            try
            {
                _dBContext.OrderDetails.RemoveRange(result);
                _dBContext.SaveChanges();
            }
            catch (Exception e)
            {
                string b = e.ToString();
            }

            List<Orders> result2 = (from d in _dBContext.Orders where d.OrderID == id select d).ToList();
            _dBContext.Orders.RemoveRange(result2);
            _dBContext.SaveChanges();

            //return RedirectToAction("Index");
        }
    }
}