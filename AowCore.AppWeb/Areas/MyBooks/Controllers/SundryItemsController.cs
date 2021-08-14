using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AowCore.Domain;
using AowCore.Application;
using AowCore.AppWeb.Helpers;
using AowCore.AppWeb.ViewModels;
using System.Collections.Generic;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class SundryItemsController : Controller
    {
        private readonly IApplicationDbContext _context;
        private readonly ICookieHelper _cookieHelper;

        public SundryItemsController(IApplicationDbContext context, ICookieHelper cookieHelper)
        {
            _context = context;
            _cookieHelper = cookieHelper;
        }

        public async Task<IActionResult> Index()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");

            if (cmpid == null)
            {
                return Redirect("/");
            }
            var cmpidG = Guid.Parse(cmpid);
            var applicationDbContext = _context.Products.Where(x => x.ItemType == "Sundry Item" && x.ProductCategory.CompanyId == cmpidG).Include(p => p.Ledger).Include(p => p.ProductCategory).Include(p => p.PurchaseLedger);
            return View(await applicationDbContext.ToListAsync());
        }
        private async Task<List<ProductCategory>> GetListOfNodes()
        {
            var categories = new List<ProductCategory>();
            var cmpid = _cookieHelper.Get("cmpCookee");
            var cmpidG = Guid.Parse(cmpid);
            var sourceCategories = await _context.ProductCategories.Where(p => p.CompanyId == cmpidG).ToListAsync();
            foreach (var sourceCategory in sourceCategories)
            {
                var c = new ProductCategory();
                c.Id = sourceCategory.Id;
                c.Name = sourceCategory.Name;
                if (sourceCategory.ParentCategoryId != null)
                {
                    c.Parent = new ProductCategory();
                    c.Parent.Id = (Guid)sourceCategory.ParentCategoryId;
                    var parentCategory = sourceCategories.Where(x => x.Id == sourceCategory.ParentCategoryId).FirstOrDefault();
                    c.Parent.Name = parentCategory.Name;
                }
                categories.Add(c);
            }
            return categories;
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Ledger)
                .Include(p => p.ProductCategory)
                .Include(p => p.PurchaseLedger)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");

            if (cmpid == null)
            {
                return Redirect("/");
            }
            var cmpidG = Guid.Parse(cmpid);
            var categories = await GetListOfNodes();
            //IList<ProductCategory> topLevelCategories = TreeHelper.ConvertToForest(categories);
            ViewData["ProductCategoryId"] = new SelectList(categories.OrderBy(x => x.Name), "Id", "Name");
            ViewData["LedgerId"] = new SelectList(_context.Ledgers.Where(x => x.LedgerCategory.CompanyId == cmpidG).OrderBy(x => x.Name), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                product.ProductCategoryId = product.ProductCategoryId;
                product.ItemType = "Sundry Item";
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LedgerId"] = new SelectList(_context.Ledgers, "Id", "Name", product.LedgerId);
            return View(product);
        }

        public async Task<IActionResult> Edit(Guid? id)
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
            var cmpid = _cookieHelper.Get("cmpCookee");

            if (cmpid == null)
            {
                return Redirect("/");
            }
            var cmpidG = Guid.Parse(cmpid);
            ViewData["LedgerId"] = new SelectList(_context.Ledgers, "Id", "Name", product.LedgerId);
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories.Where(x => x.Type == "Sundry Item").
                Where(x => x.CompanyId == cmpidG).OrderBy(x => x.Name), "Id", "Name", product.ProductCategoryId);
            SundryItemViewModel viewModel = new SundryItemViewModel();
            viewModel.Id = product.Id;
            viewModel.ProductCategoryId = product.ProductCategoryId;
            viewModel.LedgerId = product.LedgerId;
            viewModel.Name = product.Name;
            viewModel.Percent = product.Percent;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SundryItemViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }
            var cmpid = _cookieHelper.Get("cmpCookee");
            if (cmpid == null)
            {
                return Redirect("/");
            }
            var cmpidG = Guid.Parse(cmpid);
            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Products.FindAsync(id);
                    product.LedgerId = viewModel.LedgerId;
                    product.ProductCategoryId = viewModel.ProductCategoryId;
                    product.Name = viewModel.Name;
                    product.Percent = viewModel.Percent;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(viewModel.Id))
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
            ViewData["LedgerId"] = new SelectList(_context.Ledgers, "Id", "Name", viewModel.LedgerId);
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories.Where(x => x.Type == "Tax").
                Where(x => x.CompanyId == cmpidG).OrderBy(x => x.Name), "Id", "Name", viewModel.ProductCategoryId);
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Ledger)
                .Include(p => p.ProductCategory)
                .Include(p => p.PurchaseLedger)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: MyBooks/SundryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
