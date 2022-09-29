using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShopping.Web.Data;
using EShopping.Web.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace EShopping.Areas.eshopping.Controllers
{
    [Area("eshopping")]
    [Authorize]                         // check if the user has logged in.
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ApplicationDbContext context, ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: eshopping/Customers

        public IActionResult CustomerCreated()
        {

            return View();
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }
        // GET: eshopping/GetCustomer?filterCategoryId=5
        


        // GET: eshopping/Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: eshopping/Customers/Create
        [Authorize(Roles = "ProductMember")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: eshopping/Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ProductMember")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,Address,PhoneNumber,Email,OrderDateTime")] Customer customer)
        {
            customer.CustomerName = customer.CustomerName.Trim();

            // Validation Checks - Server-side validation
            bool duplicateExists = _context.Customers
              .Any(c => c.CustomerName == customer.CustomerName);
            if (duplicateExists)
            {
                ModelState.AddModelError("CustomerName", "Duplicate Customer Found!");
            }
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Created a New Category: ID = {customer.CustomerId} !");
                return RedirectToAction(nameof(CustomerCreated));

            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", customer.CustomerId);
            return View(customer);
        }


        // GET: eshopping/Customers/Edit/5
        [Authorize(Roles = "ProductMember")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: eshopping/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ProductMember")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerName,Address,PhoneNumber,Email,OrderDateTime")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: eshopping/Customers/Delete/5
        [Authorize(Roles = "ProductMember")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: eshopping/Customers/Delete/5
        [Authorize(Roles = "ProductMember")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
