using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanaAssessment.Data;
using SanaAssessment.Data.Entities;
using SanaAssessment.Web.Helper;

namespace SanaAssessment.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly SanaContext _context;
        private readonly SanaContext _contextMemory;

        public OrdersController(SanaContext context)
        {
            _context = context;
            var options = new DbContextOptionsBuilder<SanaContext>()
                         .UseInMemoryDatabase("SanaInMemory")
                         .Options;
            _contextMemory = new SanaContext(options);
        }
        private SanaContext dbcontext
        {
            get
            {
                var currentStorage = HttpContext.Session.Get<string>(SessionExtensions.SessionKeyStorageType);
                if (currentStorage == "SQL")
                {
                    return _context;
                }
                else
                {
                    return _contextMemory;
                }
            }
        }
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var sanaContext = dbcontext.Orders.Include(o => o.Customer);
            return View(await sanaContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await dbcontext.Orders
                .Include(o => o.Customer)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(dbcontext.Customers, "Id", "FirtsName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,OrderDate,DeliveryDate,Total")] Order order)
        {
            if (ModelState.IsValid)
            {
                dbcontext.Add(order);
                await dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(dbcontext.Customers, "Id", "FirtsName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await dbcontext.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(dbcontext.Customers, "Id", "FirtsName", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,OrderDate,DeliveryDate,Total")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbcontext.Update(order);
                    await dbcontext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["CustomerId"] = new SelectList(dbcontext.Customers, "Id", "FirtsName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await dbcontext.Orders
                .Include(o => o.Customer)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await dbcontext.Orders.SingleOrDefaultAsync(m => m.Id == id);
            dbcontext.Orders.Remove(order);
            await dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return dbcontext.Orders.Any(e => e.Id == id);
        }
    }
}
