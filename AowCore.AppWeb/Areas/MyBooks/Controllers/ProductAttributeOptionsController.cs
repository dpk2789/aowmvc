using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AowCore.Domain;
using AowCore.Application;
using System.Threading;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class ProductAttributeOptionsController : Controller
    {
        private readonly IApplicationDbContext _context;

        public ProductAttributeOptionsController(IApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MyBooks/ProductAttributeOptions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductAttributeOptions.Include(p => p.ProductAttributes);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MyBooks/ProductAttributeOptions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttributeOptions = await _context.ProductAttributeOptions
                .Include(p => p.ProductAttributes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productAttributeOptions == null)
            {
                return NotFound();
            }

            return View(productAttributeOptions);
        }

        // GET: MyBooks/ProductAttributeOptions/Create
        public IActionResult Create()
        {
            ViewData["ProductAttributeId"] = new SelectList(_context.ProductAttributes, "Id", "Id");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductAttributeOptions productAttributeOptions, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                productAttributeOptions.Id = Guid.NewGuid();
                _context.Add(productAttributeOptions);
                await _context.SaveChangesAsync(cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductAttributeId"] = new SelectList(_context.ProductAttributes, "Id", "Id", productAttributeOptions.ProductAttributeId);
            return View(productAttributeOptions);
        }

        // GET: MyBooks/ProductAttributeOptions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttributeOptions = await _context.ProductAttributeOptions.FindAsync(id);
            if (productAttributeOptions == null)
            {
                return NotFound();
            }
            ViewData["ProductAttributeId"] = new SelectList(_context.ProductAttributes, "Id", "Id", productAttributeOptions.ProductAttributeId);
            return View(productAttributeOptions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductAttributeOptions productAttributeOptions, CancellationToken cancellationToken)
        {
            if (id != productAttributeOptions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productAttributeOptions);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductAttributeOptionsExists(productAttributeOptions.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "ProductAttributes", new { id = productAttributeOptions.ProductAttributeId });
            }
            return View(productAttributeOptions);
        }

        // GET: MyBooks/ProductAttributeOptions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttributeOptions = await _context.ProductAttributeOptions
                .Include(p => p.ProductAttributes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productAttributeOptions == null)
            {
                return NotFound();
            }

            return View(productAttributeOptions);
        }
               
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var productAttributeOptions = await _context.ProductAttributeOptions.FindAsync(id);
            _context.ProductAttributeOptions.Remove(productAttributeOptions);
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction("Edit", "ProductAttributes", new { id = productAttributeOptions.ProductAttributeId });
        }

        private bool ProductAttributeOptionsExists(Guid id)
        {
            return _context.ProductAttributeOptions.Any(e => e.Id == id);
        }
    }
}
