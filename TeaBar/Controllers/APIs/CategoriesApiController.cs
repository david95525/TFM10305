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
    public class CategoriesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("CategoryName")]
        [Produces("application/json")]
        public List<string> GetCategoryName() //取得種類名
        {
            var result = _context.Categories
                .Select(c => c.CategoryName).ToList();
            return result;
        }

        [HttpGet]
        [Route("CategoryQuantity")]
        [Produces("application/json")]       
        public List<List<int>> GetCategoryQuantity()//取得種類數量
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

            List<List<int>> CategoryCount = new List<List<int>>();
            List<int> TeaCount = new List<int>();
            List<int> MilkCount = new List<int>();
            List<int> FruitCount = new List<int>();
            var resultT1 = ProductManageChartView.Where(c => c.CategoryID == 1 && c.Note.IndexOf("緯育") >= 0).Count();
            var resultT2 = ProductManageChartView.Where(c => c.CategoryID == 1 && c.Note.IndexOf("台南") >= 0).Count();
            var resultT3 = ProductManageChartView.Where(c => c.CategoryID == 1 && c.Note.IndexOf("台北") >= 0).Count();
            var resultT4 = ProductManageChartView.Where(c => c.CategoryID == 1 && c.Note.IndexOf("台中") >= 0).Count();
            var resultM1 = ProductManageChartView.Where(c => c.CategoryID == 2 && c.Note.IndexOf("緯育") >= 0).Count();
            var resultM2 = ProductManageChartView.Where(c => c.CategoryID == 2 && c.Note.IndexOf("台南") >= 0).Count();
            var resultM3 = ProductManageChartView.Where(c => c.CategoryID == 2 && c.Note.IndexOf("台北") >= 0).Count();
            var resultM4 = ProductManageChartView.Where(c => c.CategoryID == 2 && c.Note.IndexOf("台中") >= 0).Count();
            var resultF1 = ProductManageChartView.Where(c => c.CategoryID == 3 && c.Note.IndexOf("緯育") >= 0).Count();
            var resultF2 = ProductManageChartView.Where(c => c.CategoryID == 3 && c.Note.IndexOf("台南") >= 0).Count();
            var resultF3 = ProductManageChartView.Where(c => c.CategoryID == 3 && c.Note.IndexOf("台北") >= 0).Count();
            var resultF4 = ProductManageChartView.Where(c => c.CategoryID == 3 && c.Note.IndexOf("台中") >= 0).Count();

            TeaCount.Add(resultT1); TeaCount.Add(resultT2); TeaCount.Add(resultT3); TeaCount.Add(resultT4);
            MilkCount.Add(resultM1); MilkCount.Add(resultM2); MilkCount.Add(resultM3); MilkCount.Add(resultM4);
            FruitCount.Add(resultF1); FruitCount.Add(resultF2); FruitCount.Add(resultF3); FruitCount.Add(resultF4);
            CategoryCount.Add(TeaCount); CategoryCount.Add(MilkCount); CategoryCount.Add(FruitCount);

            //1.茶   data: [緯育,台南,台北,台中]
            //2.奶   data: [緯育,台南,台北,台中]
            //3.水果 data: [緯育,台南,台北,台中]
            //return CategoryCount;
            return CategoryCount;
        }

        // GET: api/CategoriesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categories>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // GET: api/CategoriesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categories>> GetCategories(int id)
        {
            var categories = await _context.Categories.FindAsync(id);

            if (categories == null)
            {
                return NotFound();
            }

            return categories;
        }

        // PUT: api/CategoriesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategories(int id, Categories categories)
        {
            if (id != categories.CategoryID)
            {
                return BadRequest();
            }

            _context.Entry(categories).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriesExists(id))
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

        // POST: api/CategoriesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Categories>> PostCategories(Categories categories)
        {
            _context.Categories.Add(categories);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategories", new { id = categories.CategoryID }, categories);
        }

        // DELETE: api/CategoriesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategories(int id)
        {
            var categories = await _context.Categories.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriesExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
        }
    }
}
