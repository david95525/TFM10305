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

        [HttpGet]
        [Route("Products")]
        [Produces("application/json")]
        //[ResponseCache(Duration = 600)]
        public IEnumerable<Products> GetProducts() //取得產品資料
        {
            IEnumerable<Products> result = _dbcontext.Products;

            return result;
        }
        [HttpGet]
        [Route("Category")]
        [Produces("application/json")]
        public IEnumerable<Categories> GetCategory() //取得產品分類
        {
            IEnumerable<Categories> result = _dbcontext.Categories;
            return result;
        }
    }
}
