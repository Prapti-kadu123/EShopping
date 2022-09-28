using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShopping.Models;
using EShopping.Web.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace EShopping.Areas.eshopping.Controllers
{
    [Area("eshopping")]
    [Authorize]                         // check if the user has logged in.
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
             _logger = logger;
        }

        // GET: eshopping/Products
        public IActionResult NewProducts()
        {
            
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: eshopping/GetProductOfCategory?filterCategoryId=5
        public async Task<IActionResult> GetProductOfCategory(int filterCategoryId)
        {
            var viewmodel = await _context.Products
                                          .Where(b => b.CategoryId == filterCategoryId)
                                          .Include(b => b.Category)
                                          .ToListAsync();

            return View(viewName: "Index", model: viewmodel);
        }

        // GET: eshopping/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: eshopping/Products/Create
        [Authorize(Roles = "ProductAdmin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: eshopping/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ProductAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,Quantity,CategoryId")] Product product)
        {
            product.ProductName = product.ProductName.Trim();

            // Validation Checks - Server-side validation
            bool duplicateExists = _context.Products
              .Any(c => c.ProductName == product.ProductName);
            if (duplicateExists)
            {
                ModelState.AddModelError("ProductName", "Duplicate Product Found!");
            }



            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Created a New Category: ID = {product.ProductID} !");
                return RedirectToAction(nameof(NewProducts));

            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }


        // GET: eshopping/Products/Edit/5
        [Authorize(Roles = "ProductAdmin")]
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: eshopping/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ProductAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,Quantity,CategoryId")] Product product)
        {
            {
                if (id != product.ProductID)
                {
                    return NotFound();
                }

                product.ProductName = product.ProductName.Trim();

                // Validation Checks - Server-side validation
                bool duplicateExists = _context.Products
                     .Any(c => c.ProductName == product.ProductName && c.ProductID != product.ProductID);
                if (duplicateExists)
                {
                    ModelState.AddModelError("ProductName", "Duplicate Product Found!");
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

                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
                return View(product);
            }
        }


        // GET: eshopping/Products/Delete/5
        [Authorize(Roles = "ProductAdmin")]
        public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(m => m.ProductID == id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }


        // POST: eshopping/Products/Delete/5
        [Authorize(Roles = "ProductAdmin")]
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
