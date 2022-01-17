using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //TODO
        //[HttpGet]
        //[Route("CategoryQuantity")]
        //[Produces("application/json")]
        //public int GetCategoryQuantity() //取得種類數量
        //{            
        //    var result1 = _context.OrderDetails
        //    .Where(o => o.ProductID == 1).Count();
        //    var result2 = _context.OrderDetails
        //    .Where(o => o.ProductID == 2).Count();
        //    var result3 = _context.OrderDetails
        //    .Where(o => o.ProductID == 3).Count();

        //    //1.茶
        //    //2.奶
        //    //3.水果
        //    return (result1, result2, result3);

        //    var result[] = { result1, result2, result3 };
        //    return result;
        //}

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
