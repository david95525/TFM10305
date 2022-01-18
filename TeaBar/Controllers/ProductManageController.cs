using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeaBar.Data;
using TeaBar.Models;

namespace TeaBar.Controllers
{
    public class ProductManageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;
        private readonly ApplicationDbContext db;
        private readonly string fileRoot = @"\images\";

        public ProductManageController(ApplicationDbContext context, IWebHostEnvironment env, ApplicationDbContext db)
        {
            _context = context;
            this.env = env;
            this.db = db;
        }

        // GET: ProductManage
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Categories);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: ProductManage/Create
        [HttpPost]
        public IActionResult Upload(ProductUploadModel data)
        {
            String 結果 = null;//新增後在上面顯示狀態

            try
            {
                var file = data.Picture.First();
                var combineFileName = $@"{fileRoot}{DateTime.Now.Ticks}{file.FileName}";
                using (var fileStream = System.IO.File.Create($@"{env.WebRootPath}{combineFileName}"))
                {
                    file.CopyTo(fileStream);
                }

                db.Products.Add(new Products
                {
                    ProductID = data.ProductID,
                    ProductName = data.ProductName,
                    CategoryID = data.CategoryID,
                    UnitPrice = data.UnitPrice,
                    Description = data.Description,
                    Picture = $"{combineFileName}"
                });
                db.SaveChanges();
                結果 = $"{data.ProductName}--新增成功!!";
            }
            catch
            {
                結果 = $"{data.ProductName}--新增失敗!請注意!";
            }

            //結果(字串物件)
            this.ViewBag.Message = 結果;
            var Pdata = db.Products.ToList();
           
            return View("~/Views/ProductManage/Index.cshtml", Pdata);
        }//IActionResult Upload

        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID");
            return View();
        }

        // POST: ProductManage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,CategoryID,UnitPrice,Description,Picture")] Products product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // GET: ProductManage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // POST: ProductManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,CategoryID,UnitPrice,Description,Picture")] Products product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // GET: ProductManage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: ProductManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
