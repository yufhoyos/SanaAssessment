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
    public class CategoriesController : Controller
    {
        private readonly SanaContext _context;
        private readonly SanaContext _contextMemory;       

        public CategoriesController(SanaContext context)
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

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await dbcontext.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await dbcontext.Categories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                dbcontext.Add(category);
                await dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await dbcontext.Categories.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("Id,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbcontext.Update(category);
                    await dbcontext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await dbcontext.Categories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var category = await dbcontext.Categories.SingleOrDefaultAsync(m => m.Id == id);
            dbcontext.Categories.Remove(category);
            await dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(short id)
        {
            return dbcontext.Categories.Any(e => e.Id == id);
        }
    }
}
