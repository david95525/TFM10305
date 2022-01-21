using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models;

namespace TeaBar.Controllers.APIs
{
    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsServiceController : ControllerBase
    {
        //注入db
        private readonly ApplicationDbContext _dbcontext;

        public ProductsServiceController(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        //[HttpGet]
        //[Route("Products")]
        //[Produces("application/json")]
        //[ResponseCache(Duration = 600)]
        //public IEnumerable<Products> GetProducts() //取得產品資料
        //{
        //    IEnumerable<Products> result = _dbcontext.Products;

        //    return result;
        //}

        [HttpGet]
        [Route("Products/{id}")]
        [Produces("application/json")]
        //[ResponseCache(Duration = 600)]
        public List<Products> GetProducts([FromRoute(Name ="id")] string storeid) //取得產品資料
        {
            var result = _dbcontext.Products
            .Where(s => s.Categories.StoreID == storeid)
            .Select(p=>p);

            List<Products> data = result.ToList();
            return data;
        }

        [HttpGet]
        [Route("Category/{id}")]
        [Produces("application/json")]
        public List<Categories> GetCategory([FromRoute(Name ="id")]string storeid) //取得產品分類
        {
            var result = _dbcontext.Categories
           .Where(c => c.StoreID == storeid)
           .Select(c => c);
            List<Categories> data = result.ToList();
            return data;
        }
    }
}
