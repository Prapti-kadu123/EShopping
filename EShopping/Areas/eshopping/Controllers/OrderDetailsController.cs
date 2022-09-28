using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShopping.Models;
using EShopping.Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace EShopping.Areas.eshopping.Controllers
{
    [Area("eshopping")]
    [Authorize]                         // check if the user has logged in.
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: eshopping/OrderDetails

        public IActionResult IndexCreated()
        {

            return View();
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OrderDetails.Include(o => o.Customers).Include(o => o.Payments).Include(o => o.Products);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Products/GetOrderOfCategory?filterCategoryId=5
        public async Task<IActionResult> GetOrderOfCategory(int filterCategoryId)
        {
            var viewmodel = await _context.OrderDetails
                                          .Where(b => b.ProductID == filterCategoryId)
                                          .Include(b => b.Products)
                                          .ToListAsync();

            return View(viewName: "Index", model: viewmodel);
        }




        // GET: eshopping/OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Customers)
                .Include(o => o.Payments)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: eshopping/OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName");
            ViewData["PaymentMethod"] = new SelectList(_context.Payments, "OrderDetailId", "PaymentMethods");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName");
            return View();
        }

        // POST: eshopping/OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDetailId,Quantity,OrderPrice,CustomerId,ProductID,PaymentMethod")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                /*GetPrice obj=new GetPrice();
               double pricenew = obj.GetData(order.FoodItemId);*/
                int Price = _context.Products.SingleOrDefault(b => b.ProductID == orderDetail.ProductID).Quantity;
               
                OrderDetail.ToatlAmount = orderDetail.Quantity * Price;
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexCreated));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName", orderDetail.CustomerId);
            ViewData["PaymentMethod"] = new SelectList(_context.Payments, "OrderDetailId", "PaymentMethods", orderDetail.PaymentMethod);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", orderDetail.ProductID);
            return View(orderDetail);
        }

        // GET: eshopping/OrderDetails/Edit/5
        [Authorize(Roles = "ProductAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Address", orderDetail.CustomerId);
            ViewData["PaymentMethod"] = new SelectList(_context.Payments, "OrderDetailId", "PaymentMethods", orderDetail.PaymentMethod);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", orderDetail.ProductID);
            return View(orderDetail);
        }

        // POST: eshopping/OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ProductAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Edit(int id, [Bind("OrderDetailId,Quantity,OrderPrice,CustomerId,ProductID,PaymentMethod")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderDetailId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Address", orderDetail.CustomerId);
            ViewData["PaymentMethod"] = new SelectList(_context.Payments, "OrderDetailId", "PaymentMethods", orderDetail.PaymentMethod);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", orderDetail.ProductID);
            return View(orderDetail);
        }

        // GET: eshopping/OrderDetails/Delete/5
        [Authorize(Roles = "ProductAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Customers)
                .Include(o => o.Payments)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: eshopping/OrderDetails/Delete/5

        [Authorize(Roles = "ProductAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailId == id);
        }
    }
}
