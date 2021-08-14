using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AowCore.AppWeb.ViewModels;
using AowCore.AppWeb.Helpers;
using System.Threading;
using Newtonsoft.Json;
using AowCore.AppWeb.Interfaces;
using AowCore.Application.Services;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class JournalEntriesController : Controller
    {
        //  private readonly IApplicationDbContext _context;
        private readonly IVoucherViewModelService _voucherViewModelService;
        private readonly IVoucherService _voucherService;
        private readonly ICookieHelper _cookieHelper;
        public JournalEntriesController(ICookieHelper cookieHelper,
            IVoucherViewModelService voucherViewModelService, IVoucherService voucherService)
        {
            _voucherViewModelService = voucherViewModelService;
            _voucherService = voucherService;
            _cookieHelper = cookieHelper;
        }

        public async Task<IActionResult> Index()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            var fYrId = _cookieHelper.Get("fYrCookee");
            if (cmpid == null || fYrId == null)
            {
                return Redirect("/");
            }
            var cmpidG = Guid.Parse(cmpid);
            var fYrIdG = Guid.Parse(fYrId);
            var vouchers = await _voucherService.GetVouchers(fYrIdG, "Journal Entry");
            var list = await _voucherViewModelService.JEntryListViewModel(vouchers, cmpidG);
            return View(list);
        }

        public async Task<JsonResult> GetLedgersForJournalEntry(string term, string HeadName, string crdr)
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            if (cmpid == null)
            {
                return Json(new
                {
                    company = "no company"
                });
            }
            var cmpidG = Guid.Parse(cmpid);
            var data = await _voucherViewModelService.GetLedgersForVouchers(cmpidG, term, HeadName, crdr);


            List<LedgerViewModel> productViewModelList = data;


            return Json(productViewModelList.Select(m => new
            {
                id = m.Id,
                value = m.Name,
                //rootCategory = m.Parent.Name,
                label = String.Format("{0}/{1}", m.RootCategory, m.Name),
            }));
        }

        // GET: MyBooks/JournalEntries/Create
        public IActionResult Create()
        {
            var viewModel = new VoucherViewModel
            {
                Date = DateTime.Now
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string data, decimal Invoice, DateTime Date)
        {
            if (ModelState.IsValid)
            {
                if (data != null)
                {
                    try
                    {
                        var cmpid = _cookieHelper.Get("cmpCookee");
                        var fid = _cookieHelper.Get("fYrCookee");
                        Guid fyrId = Guid.Parse(fid);
                        if (cmpid == null)
                        {
                            return Json(new
                            {
                                company = "no company"
                            });
                        }
                        await _voucherViewModelService.CreateVourcherAsync("Journal Entry", data, Invoice, Date, fyrId);
                        // return RedirectToAction(nameof(Index));
                        return Json(new { success = true, newLocation = "/Accounts/Payment/Index/" });
                    }
                    catch (Exception ex)
                    {
                        var msg = new ModelStateException(ex);
                        TempData["MessageToClientError"] = msg;
                        return Json(new { success = false, message = msg.Message });
                        //ModelState.AddModelError("", msg);                           
                    }
                }

            }
            return View();
        }

        // GET: MyBooks/JournalEntries/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cmpid = _cookieHelper.Get("cmpCookee");
            if (cmpid == null)
            {
                return Redirect("/");
            }
            Guid cmpidG = Guid.Parse(cmpid);
            var viewModel = await _voucherViewModelService.GetVoucherForViewModel(id.Value, cmpidG);
            if (viewModel == null)
            {
                return NotFound();
            }
            var ledgers = await _voucherViewModelService.GetAllLedgers(cmpidG);
            if (ledgers == null)
            {
                return Redirect("/");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, string data, decimal Invoice, string voucherData)
        {
            var voucherViewModel = JsonConvert.DeserializeObject<VoucherViewModel>(data);
            if (id != voucherViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (data != null)
                    {
                        try
                        {
                            var cmpid = _cookieHelper.Get("cmpCookee");
                            var fid = _cookieHelper.Get("fYrCookee");
                            Guid fyrId = Guid.Parse(fid);
                            if (cmpid == null)
                            {
                                return Json(new
                                {
                                    company = "no company"
                                });
                            }
                            await _voucherViewModelService.EditVourcherAsync("Journal Entry", voucherData, Invoice, data, fyrId);
                            // return RedirectToAction(nameof(Index));
                            return Json(new { success = true, newLocation = "/Accounts/Payment/Index/" });
                        }
                        catch (Exception ex)
                        {
                            var msg = new ModelStateException(ex);
                            TempData["MessageToClientError"] = msg;
                            return Json(new { success = false, message = msg.Message });
                            //ModelState.AddModelError("", msg);                           
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_voucherViewModelService.VoucherExists(voucherViewModel.Id))
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

            return View(voucherViewModel);
        }

        // GET: MyBooks/JournalEntries/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cmpid = _cookieHelper.Get("cmpCookee");
            if (cmpid == null)
            {
                return Redirect("/");
            }
            Guid cmpidG = Guid.Parse(cmpid);
            var viewModel = await _voucherViewModelService.GetVoucherForViewModel(id.Value, cmpidG);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: MyBooks/JournalEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            await _voucherViewModelService.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
