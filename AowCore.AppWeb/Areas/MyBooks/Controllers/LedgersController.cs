using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AowCore.Domain;
using AowCore.Application;
using System.Threading;
using System.Collections.Generic;
using AowCore.AppWeb.ViewModels;
using AowCore.AppWeb.Helpers;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class LedgersController : Controller
    {
        private readonly IApplicationDbContext _context;
        private readonly ICookieHelper _cookieHelper;
        public LedgersController(IApplicationDbContext context, ICookieHelper cookieHelper)
        {
            _cookieHelper = cookieHelper;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");

            if (cmpid == null)
            {
                return Redirect("/");
            }
          
            var cmpidG = Guid.Parse(cmpid);
            var ledgers = await GetLedgers(cmpidG);
            //List<LedgerViewModel> accountViewModelList = new List<LedgerViewModel>();
            //foreach (var ledger in ledgers.OrderBy(x => x.Name))
            //{
            //    LedgerViewModel viewModel = new LedgerViewModel();
            //    if (ledger.ParentCategoryId != null)
            //    {
            //        var ledgerParent = ledgers.Where(x => x.Id == ledger.ParentCategoryId).FirstOrDefault();
            //        viewModel.RootCategory = ledgerParent.Name;
            //    }
            //    viewModel.Id = ledger.Id;
            //    viewModel.Name = ledger.Name;
            //    viewModel.CategoryName = ledger.LedgerCategory.Name;
            //    // viewModel.RootCategory = ledger.LedgerCategory.Parent.Name;
            //    //accountViewModel.CurrentBalance = ledger.CurrentBalance;
            //    accountViewModelList.Add(viewModel);
            //}
            return View(ledgers);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledger = await _context.Ledgers
                .Include(l => l.LedgerCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ledger == null)
            {
                return NotFound();
            }

            return View(ledger);
        }
        public async Task<IList<LedgerCategoryViewModel>> GetCategories(Guid cmpid)
        {           
            var categoryList = await _context.LedgerCategories.Where(x => x.CompanyId == cmpid).Select(a => new LedgerCategoryViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                ParentCategoryId = a.ParentCategoryId
            }).OrderBy(x => x.Name).ToListAsync();
            return categoryList;
        }

        public async Task<IList<LedgerViewModel>> GetLedgers(Guid cmpidG)
        {          
            var ledgers = await _context.Ledgers.Include(x => x.LedgerCategory).Where(c => c.LedgerCategory.CompanyId == cmpidG).Select(a => new LedgerViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                CategoryName = a.LedgerCategory.Name,
                RootCategory = a.ParentCategoryId != null ? a.Parent.Name : a.LedgerCategory.Name
            }).OrderBy(x => x.Name).ToListAsync();
            
            return ledgers;
        }

        private async Task<SelectList> PopulateParentCategorySelectList(Guid? id, Guid cmpidG)
        {
            SelectList selectList;
            var categories = await GetCategories(cmpidG);
            if (id == null)
                selectList = new SelectList(categories.Where(c => c.ParentCategoryId == null), "Id", "Name");
            else if (categories.Count(c => c.ParentCategoryId == id) == 0)
                selectList = new SelectList(categories.Where(c => c.ParentCategoryId == null && c.Id != id), "Id", "Name");
            else selectList = new SelectList(categories, "Id", "Name");
            return selectList;
        }

        public async Task<IActionResult> Create()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");

            if (cmpid == null)
            {
                return Redirect("/");
            }

            var cmpidG = Guid.Parse(cmpid);
            var categories = await GetCategories(cmpidG);
            ViewData["LedgerCategoryId"] = new SelectList(categories.Where(c => c.ParentCategoryId != null), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ledger ledger, CancellationToken cancellationToken)
        {
            var cmpid = _cookieHelper.Get("cmpCookee");

            if (cmpid == null)
            {
                return Redirect("/");
            }

            var cmpidG = Guid.Parse(cmpid);
            if (ModelState.IsValid)
            {
                ledger.Id = Guid.NewGuid();
                _context.Add(ledger);
                await _context.SaveChangesAsync(cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            ViewData["LedgerCategoryId"] = PopulateParentCategorySelectList(null, cmpidG);
            return View(ledger);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledger = await _context.Ledgers.FindAsync(id);
            if (ledger == null)
            {
                return NotFound();
            }
            var cmpid = _cookieHelper.Get("cmpCookee");

            if (cmpid == null)
            {
                return Redirect("/");
            }

            var cmpidG = Guid.Parse(cmpid);
            var categories = await GetCategories(cmpidG);
            ViewData["LedgerCategoryId"] = new SelectList(categories, "Id", "Name", ledger.LedgerCategoryId);
            return View(ledger);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Ledger ledger, CancellationToken cancellationToken)
        {
            if (id != ledger.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ledger);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LedgerExists(ledger.Id))
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
            ViewData["LedgerCategoryId"] = new SelectList(_context.LedgerCategories, "Id", "Id", ledger.LedgerCategoryId);
            return View(ledger);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledger = await _context.Ledgers
                .Include(l => l.LedgerCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ledger == null)
            {
                return NotFound();
            }

            return View(ledger);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var ledger = await _context.Ledgers.FindAsync(id);
            _context.Ledgers.Remove(ledger);
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        private bool LedgerExists(Guid id)
        {
            return _context.Ledgers.Any(e => e.Id == id);
        }
    }
}
