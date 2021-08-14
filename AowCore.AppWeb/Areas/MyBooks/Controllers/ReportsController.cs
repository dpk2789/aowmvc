using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AowCore.Domain;
using AowCore.Application;
using AowCore.Application.IRepository;
using AowCore.AppWeb.Helpers;
using AowCore.AppWeb.Interfaces;
using System.Collections.Generic;
using AowCore.AppWeb.ViewModels;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class ReportsController : Controller
    {
        private readonly IApplicationDbContext _context;
        private readonly IVoucherRepository _voucherRepository;
        private readonly ICookieHelper _cookieHelper;
        private readonly IVoucherViewModelService _voucherViewModelService;

        public ReportsController(IApplicationDbContext context, IVoucherRepository voucherRepository, ICookieHelper cookieHelper,
            IVoucherViewModelService voucherViewModelService)
        {
            _context = context;
            _voucherRepository = voucherRepository;
            _cookieHelper = cookieHelper;
            _voucherViewModelService = voucherViewModelService;
        }

        public async Task<IActionResult> Index()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            if (cmpid == null)
            {
                return Redirect("/");
            }
            Guid cmpidG = Guid.Parse(cmpid);
            var ledgers = await _voucherViewModelService.GetAllLedgers(cmpidG);
            if (ledgers == null)
            {
                return Redirect("/");
            }
            ViewData["Ledgers"] = new SelectList(ledgers.OrderBy(x => x.Name), "Id", "Name");
            return View();
        }

        public async Task<ActionResult> BankReconciliation()
        {
            string cmpid = _cookieHelper.Get("cmpCookee");
            string fYrId = _cookieHelper.Get("fYrCookee");
            if (cmpid == null || fYrId == null)
            {
                return Redirect("/");
            }
            Guid cmpidG = Guid.Parse(cmpid);
            Guid fYrIdG = Guid.Parse(fYrId);
            var vouchers = await _voucherRepository.GetAllVouchers(fYrIdG);
            var list = await _voucherViewModelService.JEntryListViewModel(vouchers, cmpidG);
            return View(list);
        }

        public async Task<ActionResult> DisplaySearchResults(Guid? Id, DateTime? todate, DateTime? fromdate)
        {
            var ledgerViewModelList = new List<LedgerReportViewModel>();
            IEnumerable<LedgerReportViewModel> LedgerViewModelEnu;
            string fYrId = _cookieHelper.Get("fYrCookee");
            if (fYrId == null)
            {
                return Redirect("/");
            }
            Guid fYrIdG = Guid.Parse(fYrId);
            if (Id != null)
            {
                var ledgerDetails = await _context.Ledgers.Where(x => x.Id == Id).FirstOrDefaultAsync();
                string modelString = string.Empty;
                if (todate != null || fromdate != null)
                {
                    var jEntries = _context.JournalEntries.Where(x => x.Vouchers.FinancialYearId == fYrIdG && x.LedgerId == Id).ToList();
                    if (Id != Guid.Empty)
                    {
                        foreach (var jEntry in jEntries)
                        {
                            foreach (var jEntrybyLedger in jEntry.Vouchers.JournalEntries)
                            {
                                if (jEntrybyLedger.LedgerId != Id)
                                {
                                    var viewModel = new LedgerReportViewModel()
                                    {
                                        Id = jEntrybyLedger.Id,
                                        VoucherName = jEntrybyLedger.VoucherName,
                                        Date = jEntrybyLedger.Date,
                                        LedgerName = jEntrybyLedger.Ledger.Name,
                                        CreditAmount = jEntrybyLedger.CreditAmount,
                                        DebitAmount = jEntrybyLedger.DebitAmount,
                                    };
                                    ledgerViewModelList.Add(viewModel);
                                }

                            }
                        }

                        LedgerViewModelEnu = ledgerViewModelList;
                        //  return PartialView("_FilterDaily", violationViewModelEnu);
                        modelString = await this.RenderViewAsync("_LedgerReport", LedgerViewModelEnu, true);
                        return Json(new { success = true, modelString });
                    }
                }
                else
                {
                    var jEntries = _context.JournalEntries.Where(x => x.Vouchers.FinancialYearId == fYrIdG && x.LedgerId == Id).ToList();
                    if (Id != Guid.Empty)
                    {
                        foreach (var jEntry in jEntries)
                        {
                            var viewModel = new LedgerReportViewModel()
                            {
                                Id = jEntry.Id,
                                VoucherName = jEntry.VoucherName,
                                Date = jEntry.Date,
                                LedgerName = jEntry.Ledger.Name,
                                CreditAmount = jEntry.CreditAmount,
                                DebitAmount = jEntry.DebitAmount,
                            };
                            ledgerViewModelList.Add(viewModel);
                        }

                        LedgerViewModelEnu = ledgerViewModelList;
                        //  return PartialView("_FilterDaily", violationViewModelEnu);
                        modelString = await this.RenderViewAsync("_LedgerReport", LedgerViewModelEnu, true);
                        return Json(new { success = true, modelString });
                    }
                }

                LedgerViewModelEnu = ledgerViewModelList;
                //  return PartialView("_FilterDaily", violationViewModelEnu);              
                modelString = await this.RenderViewAsync("_LedgerReport", LedgerViewModelEnu, true);
                return Json(new { success = true, modelString });
            }

            else
            {
                LedgerViewModelEnu = ledgerViewModelList;
            }
            return Json(new { success = true, LedgerViewModelEnu });
        }

        public IActionResult Create()
        {
            ViewData["FinancialYearId"] = new SelectList(_context.FinancialYears, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                voucher.Id = Guid.NewGuid();
                _context.Add(voucher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FinancialYearId"] = new SelectList(_context.FinancialYears, "Id", "Name", voucher.FinancialYearId);
            return View(voucher);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            ViewData["FinancialYearId"] = new SelectList(_context.FinancialYears, "Id", "Id", voucher.FinancialYearId);
            return View(voucher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Voucher voucher)
        {
            if (id != voucher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voucher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherExists(voucher.Id))
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
            ViewData["FinancialYearId"] = new SelectList(_context.FinancialYears, "Id", "Id", voucher.FinancialYearId);
            return View(voucher);
        }

        // GET: MyBooks/Reports/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers
                .Include(v => v.FinancialYear)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // POST: MyBooks/Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherExists(Guid id)
        {
            return _context.Vouchers.Any(e => e.Id == id);
        }
    }
}
