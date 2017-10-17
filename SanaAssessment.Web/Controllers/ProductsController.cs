using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanaAssessment.Data;
using SanaAssessment.Data.Entities;
using SanaAssessment.Web.Models;
using SanaAssessment.Web.Helper;

namespace SanaAssessment.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SanaContext _context;
        private readonly SanaContext _contextMemory;

        public ProductsController(SanaContext context)
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
        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await dbcontext.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await dbcontext.Products.Include(p => p.ProductCategories).SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            LoadCategoriesInfo(product, true);
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            LoadCategoriesInfo(null);
            return View();
        }

        private void LoadCategoriesInfo(Product product, bool assignedOnly = false)
        {
            var allCategories = dbcontext.Categories;
            var viewModel = new List<ProductsCategoriesViewModel>();
            var productCategories = new List<short>();
            if (product != null)
            {
                productCategories = product.ProductCategories.Select(pc => pc.CategoryId).ToList();
            }
            viewModel = allCategories.Select(c => new ProductsCategoriesViewModel { CategoryId = c.Id, CategoryDescription = c.Description, Assigned = productCategories.Contains(c.Id) }).ToList();
            if (assignedOnly)
            {
                viewModel = viewModel.Where(v => v.Assigned).ToList();
            }
            ViewData["Categories"] = viewModel;
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SKU,Description,Price")] Product product, List<short> selectedCategories)
        {
            if (ModelState.IsValid)
            {
                dbcontext.Add(product);
                UpdateProductCategories(product, selectedCategories);
                await dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            LoadCategoriesInfo(null);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await dbcontext.Products.Include(p => p.ProductCategories).SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            LoadCategoriesInfo(product);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SKU,Description,Price")] Product product, List<short> selectedCategories)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            var productForUpdate = await dbcontext.Products.Include(p => p.ProductCategories).SingleOrDefaultAsync(m => m.Id == id);
            product.ProductCategories = productForUpdate.ProductCategories;

            if (ModelState.IsValid)
            {
                try
                {                  
                    dbcontext.Update(product);
                    UpdateProductCategories(product, selectedCategories);
                    await dbcontext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            LoadCategoriesInfo(productForUpdate);
            return View(product);
        }

        private void UpdateProductCategories(Product product, List<short> categories)
        {
            if (categories == null)
            {
                product.ProductCategories = new List<ProductCategory>();
                return;
            }
            if (product.ProductCategories == null)
            {
                product.ProductCategories = new List<ProductCategory>();
            }

            var productCategories = product.ProductCategories.Select(pc => pc.CategoryId).ToList();
            var allCategories = dbcontext.Categories;
            foreach (var cat in allCategories)
            {
                if (categories.Contains(cat.Id))
                {
                    if (!productCategories.Contains(cat.Id))
                    {
                        product.ProductCategories.Add(new ProductCategory { CategoryId = cat.Id, ProductId = product.Id });
                    }
                }
                else
                {
                    if (productCategories.Contains(cat.Id))
                    {
                        ProductCategory courseToRemove = product.ProductCategories.FirstOrDefault(p => p.CategoryId == cat.Id);
                        dbcontext.Remove(courseToRemove);
                    }
                }
            }
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await dbcontext.Products
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await dbcontext.Products.SingleOrDefaultAsync(m => m.Id == id);
            dbcontext.Products.Remove(product);
            await dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return dbcontext.Products.Any(e => e.Id == id);
        }
    }
}
