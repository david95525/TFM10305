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
    public class StoresApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StoresApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("storeName")]
        [Produces("application/json")]
        public List<string> GetStoreName() //取得分店店名
        {
            var result = _context.Stores                
                .Select(s => s.StoreName).Distinct().ToList();            
            return result;
        }

        // GET: api/StoresApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stores>>> GetStores()
        {
            return await _context.Stores.ToListAsync();
        }

        // GET: api/StoresApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Stores>> GetStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);

            if (store == null)
            {
                return NotFound();
            }

            return store;
        }

        // PUT: api/StoresApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(string id, Stores store)
        {
            if (id != store.StoreID)
            {
                return BadRequest();
            }

            _context.Entry(store).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
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

        // POST: api/StoresApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Stores>> PostStore(Stores store)
        {
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStore", new { id = store.StoreID }, store);
        }

        // DELETE: api/StoresApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StoreExists(string id)
        {
            return _context.Stores.Any(e => e.StoreID == id);
        }
    }
}
