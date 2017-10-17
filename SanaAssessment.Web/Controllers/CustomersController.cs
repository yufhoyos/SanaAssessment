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
    public class CustomersController : Controller
    {
        private readonly SanaContext _context;
        private readonly SanaContext _contextMemory;

        public CustomersController(SanaContext context)
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

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await dbcontext.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await dbcontext.Customers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdentificationNumber,FirtsName,MiddleName,LastName")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                dbcontext.Add(customer);
                await dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await dbcontext.Customers.SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdentificationNumber,FirtsName,MiddleName,LastName")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbcontext.Update(customer);
                    await dbcontext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await dbcontext.Customers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await dbcontext.Customers.SingleOrDefaultAsync(m => m.Id == id);
            dbcontext.Customers.Remove(customer);
            await dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return dbcontext.Customers.Any(e => e.Id == id);
        }
    }
}
