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
    [Authorize]        // check if user has logged in, and is a User with Admin Role
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public object GetCategories()
        {
            throw new NotImplementedException();
        }
        public IActionResult IndexCreated()
        {
            return View();
        }

        // Receive the registered ApplicationDbContext through the DI Container.
        // The registration was done in the Startup.cs ConfigureServices() method.
        public CategoriesController(ApplicationDbContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //------- ASynchronous Version of the Index method
        //        which internally calls another Async method ( ToListAsync() ).
        // GET: Products/Categories
        public async Task<IActionResult> Index()
        {
            var viewmodel = await _context.Categories
                                          .Include(c => c.Products)
                                          .ToListAsync();
            return View(viewmodel);
        }

        //------- Synchronous Version of the Index method
        //public IActionResult Index()
        //{
        //    var viewmodel = _context.Categories.Include(c => c.Books).ToList();
        //    return View(viewmodel);
        //}

        //------- Asynchronous Version of the Index method
        //public Task<IActionResult> Index()
        //{
        //    var viewmodel = _context.Categories.Include(c => c.Books).ToList();
        //    return View(viewmodel);
        //}


        // GET: Products/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Products/Categories/Create
        [Authorize(Roles = "ProductAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ProductAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] Category category)
        {
            // Sanitize the data
            category.CategoryName = category.CategoryName.Trim();

            // Validation Checks - Server-side validation
            bool duplicateExists = _context.Categories.Any(c => c.CategoryName == category.CategoryName);
            if (duplicateExists)
            {
                ModelState.AddModelError("CategoryName", "Duplicate Category Found!");
            }

            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Created a New Category: ID = {category.CategoryId} !");
                return RedirectToAction(nameof(IndexCreated));
            }
            return View(category);
        }


        // GET: Products/Categories/Edit/5
        [Authorize(Roles = "ProductAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Products/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ProductAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] Category category)
        {

            // Validation Check if the CategoryId matches with the ID for the row to edit.
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            // Sanitize the data
            category.CategoryName = category.CategoryName.Trim();

            // Validation Checks - Server-side validation
            bool duplicateExists = _context.Categories
                .Any(c => c.CategoryName == category.CategoryName && c.CategoryId != category.CategoryId);
            if (duplicateExists)
            {
                ModelState.AddModelError("CategoryName", "Duplicate Category Found!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        // GET: Products/Categories/Delete/5
        [Authorize(Roles = "ProductAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Products/Categories/Delete/5
        [Authorize(Roles = "ProductAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
