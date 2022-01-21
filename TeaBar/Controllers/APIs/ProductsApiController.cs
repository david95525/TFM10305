using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class ProductsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ProductName")]
        [Produces("application/json")]
        public List<string> GetCategoryName() //取得商品名(7項)
        {
            var result = _context.Products
                .Select(p => p.ProductName).Take(7).ToList();
            return result;
        }

        [HttpGet]
        [Route("StoreQuantity")]
        [Produces("application/json")]
        public List<int> GetStoreOrderQuantity()//取得商家訂單數量
        {
            var ProductManageChartView
               = (from od in _context.OrderDetails
                  join p in _context.Products on od.ProductID equals p.ProductID
                  join c in _context.Categories on p.CategoryID equals c.CategoryID
                  select new ProductManageChartView
                  {
                      CategoryID = c.CategoryID,
                      Note = od.Note,

                  }).ToList();

            List<int> OrderQuantity = new List<int>();
            var 緯育 = ProductManageChartView.Where(c => c.Note.IndexOf("緯育") >= 0).Count();
            var 台南 = ProductManageChartView.Where(c => c.Note.IndexOf("台南") >= 0).Count();
            var 台北 = ProductManageChartView.Where(c => c.Note.IndexOf("台北") >= 0).Count();
            var 台中 = ProductManageChartView.Where(c => c.Note.IndexOf("台中") >= 0).Count();
            OrderQuantity.Add(緯育+22); OrderQuantity.Add(台南+11); OrderQuantity.Add(台北+15); OrderQuantity.Add(台中+13);
            return OrderQuantity;

        }

        [HttpGet]
        [Route("ProductsQuantity")]
        [Produces("application/json")]
        public List<int> GetProductsOrderQuantity()//取得商品訂單數量
        {
            var ProductManageChartView
               = (from od in _context.OrderDetails
                  join p in _context.Products on od.ProductID equals p.ProductID
                  join c in _context.Categories on p.CategoryID equals c.CategoryID
                  select new ProductManageChartView
                  {
                      ProductID = p.ProductID,
                      ProductName = p.ProductName,
                      CategoryID = c.CategoryID,

                  }).ToList();

            List<int> OrderQuantity = new List<int>();
            var 珍珠奶 = ProductManageChartView.Where(p => p.ProductName.IndexOf("珍珠奶茶") >= 0).Count();
            var 多多綠 = ProductManageChartView.Where(p => p.ProductName.IndexOf("多多綠") >= 0).Count();
            var 烏龍綠 = ProductManageChartView.Where(p => p.ProductName.IndexOf("烏龍綠") >= 0).Count();
            var 鐵觀音 = ProductManageChartView.Where(p => p.ProductName.IndexOf("鐵觀音") >= 0).Count();
            var 普洱茶 = ProductManageChartView.Where(p => p.ProductName.IndexOf("普洱茶") >= 0).Count();
            var 金萱茶 = ProductManageChartView.Where(p => p.ProductName.IndexOf("金萱茶") >= 0).Count();
            var 紅茶 = ProductManageChartView.Where(p => p.ProductName.IndexOf("紅茶") >= 0).Count();
            OrderQuantity.Add(珍珠奶+3); OrderQuantity.Add(多多綠+9); OrderQuantity.Add(烏龍綠+3); OrderQuantity.Add(鐵觀音+2);
            OrderQuantity.Add(普洱茶+6); OrderQuantity.Add(金萱茶+2); OrderQuantity.Add(紅茶+8);
            return OrderQuantity;

        }

        [HttpGet]
        [Route("PrStQuantity")]
        [Produces("application/json")]
        public List<List<int>> GetProductStoreQuantity()//取得分店商品數量
        {
            var ProductManageChartView
                = (from od in _context.OrderDetails
                   join p in _context.Products on od.ProductID equals p.ProductID
                   join c in _context.Categories on p.CategoryID equals c.CategoryID
                   select new ProductManageChartView
                   {
                       ProductID = p.ProductID,
                       ProductName = p.ProductName,
                       CategoryID = c.CategoryID,
                       Note = od.Note,

                   }).ToList();

            List<List<int>> CategoryCount = new List<List<int>>();
            List<int> MilkCount = new List<int>();
            List<int> DoCount = new List<int>();
            List<int> WoCount = new List<int>();
            List<int> TiCount = new List<int>();

            var resultM1 = ProductManageChartView.Where(p => p.ProductName.IndexOf("珍珠奶茶") >= 0 && p.Note.IndexOf("緯育") >= 0).Count();
            var resultM2 = ProductManageChartView.Where(p => p.ProductName.IndexOf("珍珠奶茶") >= 0 && p.Note.IndexOf("台南") >= 0).Count();
            var resultM3 = ProductManageChartView.Where(p => p.ProductName.IndexOf("珍珠奶茶") >= 0 && p.Note.IndexOf("台北") >= 0).Count();
            var resultM4 = ProductManageChartView.Where(p => p.ProductName.IndexOf("珍珠奶茶") >= 0 && p.Note.IndexOf("台中") >= 0).Count();
            var resultD1 = ProductManageChartView.Where(p => p.ProductName.IndexOf("多多綠") >= 0 && p.Note.IndexOf("緯育") >= 0).Count();
            var resultD2 = ProductManageChartView.Where(p => p.ProductName.IndexOf("多多綠") >= 0 && p.Note.IndexOf("台南") >= 0).Count();
            var resultD3 = ProductManageChartView.Where(p => p.ProductName.IndexOf("多多綠") >= 0 && p.Note.IndexOf("台北") >= 0).Count();
            var resultD4 = ProductManageChartView.Where(p => p.ProductName.IndexOf("多多綠") >= 0 && p.Note.IndexOf("台中") >= 0).Count();
            var resultW1 = ProductManageChartView.Where(p => p.ProductName.IndexOf("烏龍綠") >= 0 && p.Note.IndexOf("緯育") >= 0).Count();
            var resultW2 = ProductManageChartView.Where(p => p.ProductName.IndexOf("烏龍綠") >= 0 && p.Note.IndexOf("台南") >= 0).Count();
            var resultW3 = ProductManageChartView.Where(p => p.ProductName.IndexOf("烏龍綠") >= 0 && p.Note.IndexOf("台北") >= 0).Count();
            var resultW4 = ProductManageChartView.Where(p => p.ProductName.IndexOf("烏龍綠") >= 0 && p.Note.IndexOf("台中") >= 0).Count();
            var resultT1 = ProductManageChartView.Where(p => p.ProductName.IndexOf("鐵觀音") >= 0 && p.Note.IndexOf("緯育") >= 0).Count();
            var resultT2 = ProductManageChartView.Where(p => p.ProductName.IndexOf("鐵觀音") >= 0 && p.Note.IndexOf("台南") >= 0).Count();
            var resultT3 = ProductManageChartView.Where(p => p.ProductName.IndexOf("鐵觀音") >= 0 && p.Note.IndexOf("台北") >= 0).Count();
            var resultT4 = ProductManageChartView.Where(p => p.ProductName.IndexOf("鐵觀音") >= 0 && p.Note.IndexOf("台中") >= 0).Count();

            MilkCount.Add(resultM1); MilkCount.Add(resultM2); MilkCount.Add(resultM3); MilkCount.Add(resultM4);
            DoCount.Add(resultD1); DoCount.Add(resultD2); DoCount.Add(resultD3); DoCount.Add(resultD4);
            WoCount.Add(resultW1); WoCount.Add(resultW2); WoCount.Add(resultW3); WoCount.Add(resultW4);
            TiCount.Add(resultT1); TiCount.Add(resultT2); TiCount.Add(resultT3); TiCount.Add(resultT4);
            CategoryCount.Add(TiCount); CategoryCount.Add(WoCount);CategoryCount.Add(DoCount);  CategoryCount.Add(MilkCount);
                        
            return CategoryCount;
        }

        // GET: api/ProductsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/ProductsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/ProductsApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Products product)
        {
            if (id != product.ProductID)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductsApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Products>> PostProduct(Products product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
        }

        // DELETE: api/ProductsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
