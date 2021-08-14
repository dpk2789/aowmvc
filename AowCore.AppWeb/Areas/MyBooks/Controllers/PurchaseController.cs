using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AowCore.Domain;
using AowCore.AppWeb.Interfaces;
using AowCore.AppWeb.Helpers;
using AowCore.Application;
using System.Collections.Generic;
using AowCore.AppWeb.ViewModels;
using AowCore.Application.Services;
using AowCore.Application.IRepository;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class PurchaseController : Controller
    {
        private readonly IApplicationDbContext _context;
        private readonly IVoucherViewModelService _voucherViewModelService;
        private readonly ICookieHelper _cookieHelper;
        private readonly IVoucherService _voucherService;
        private readonly IVoucherRepository _voucherRepository;

        public PurchaseController(IApplicationDbContext context, ICookieHelper cookieHelper,
            IVoucherViewModelService voucherViewModelService, IVoucherService voucherService, IVoucherRepository voucherRepository)
        {
            _context = context;
            _voucherService = voucherService;
            _voucherViewModelService = voucherViewModelService;
            _cookieHelper = cookieHelper;
            _voucherRepository = voucherRepository;
        }

        // GET: MyBooks/Purchase
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
            var vouchers = await _voucherService.GetVouchers(fYrIdG, "Purchase Bill");
            var list = await _voucherViewModelService.PurchaseListToViewModel(vouchers, cmpidG);
            return View(list);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journalEntry = await _context.JournalEntries
                .Include(j => j.Ledger)
                .Include(j => j.Vouchers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (journalEntry == null)
            {
                return NotFound();
            }

            return View(journalEntry);
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

        public JsonResult GetSundryItemForAutocomplete(string term)
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            var cmpidG = Guid.Parse(cmpid);

            Product[] productsMatching = String.IsNullOrWhiteSpace(term) ? null
       : _context.Products.Include(x => x.ProductCategory).Include(x => x.Ledger).Where(p => p.ProductCategory.CompanyId == cmpidG).
       Where(x => x.ItemType == "Sundry Item").Where(ii => ii.Name.Contains(term)).ToArray();

            List<ProductViewModel> productViewModelList = new List<ProductViewModel>();
            foreach (var product in productsMatching.ToList())
            {
                ProductViewModel productViewModel = new ProductViewModel
                {
                    Id = product.Id,
                    ProductId = product.Id,
                    Code = product.Code,
                    Name = product.Name,
                    Percent = product.Percent,
                    ItemType = product.ItemType,
                    LedgerId = product.Ledger.Id,
                    AccountCategoryName = product.ProductCategory.Name != null ? product.ProductCategory.Name.Replace(" ", string.Empty) : null
                };
                productViewModelList.Add(productViewModel);
            }
            //  productViewModel.AccountId = product.PurchaseAccountId;
            // productViewModel.StoreViewModels = product.StoreProducts.ToList();

            return Json(productViewModelList.Select(m => new
            {
                id = m.Id,
                value = m.Name,
                label = String.Format("{0}/{1}/{2}", m.LedgerName, m.Name, m.Percent),
                name = m.Name,
                percent = m.Percent,
                itemCategoryName = m.AccountCategoryName != null ? m.AccountCategoryName.Replace(" ", string.Empty) : null,
                productLedgerId = m.LedgerId,
                productId = m.ProductId,
                itemtype = m.ItemType != null ? m.ItemType.Replace(" ", string.Empty) : null,
            }));

        }
        public IActionResult Create()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            var fYrId = _cookieHelper.Get("fYrCookee");
            if (cmpid == null || fYrId == null)
            {
                return Redirect("/");
            }
            var viewModel = new VoucherInvoiceViewModel
            {
                Date = DateTime.Today
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string data, string data2, decimal Invoice, DateTime Date, int? termsDays, Guid AccountId,
            decimal? Total, string Note, string buttonValue)
        {
            if (ModelState.IsValid)
            {
                if (data != null)
                {
                    try
                    {
                        var cmpid = _cookieHelper.Get("cmpCookee");
                        var fid = _cookieHelper.Get("fYrCookee");
                        var cmpidG = Guid.Parse(cmpid);
                        var fyrId = Guid.Parse(fid);
                        if (cmpid == null || fyrId == null)
                        {
                            return Redirect("/");
                        }
                        FinancialYear fyr = _context.FinancialYears.Find(fyrId);

                        if (fyr.IsLocked == false && fyr.Start <= Date.Date && fyr.End >= Date.Date)
                        {
                            var voucher = new Voucher();
                            Guid voucherId = Guid.NewGuid();
                            var ledger = await _context.Ledgers.Where(x => x.LedgerCategory.CompanyId == cmpidG && x.Name == "Purchase Account").FirstOrDefaultAsync();
                            var result = await _voucherViewModelService.EditVoucherInvoice(data, data2, "Purchase Bill", Invoice, Date, termsDays, AccountId, voucherId, Total, Note, voucher, cmpidG, fyr.Id, "Create");
                            if (buttonValue == "Save & Next")
                            {
                                return Json(new { success = true, invoice = Invoice, date = Date, btnvalue = buttonValue });
                            }
                            else
                            {
                                return Json(new { success = true, invoice = Invoice, btnvalue = buttonValue });
                            }

                        }
                        // return RedirectToAction(nameof(Index));
                        return Json(new { success = true, newLocation = "/MyBooks/Purchase/Index/" });
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

        // GET: MyBooks/Purchase/Edit/5
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
            var viewModel = await _voucherViewModelService.GetVoucherInvoiceForViewModel(id.Value, cmpidG);
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
        public async Task<ActionResult> Edit(string data, string data2, decimal Invoice, DateTime Date, int? termsDays, Guid AccountId, Guid VoucherId,
            decimal? Total, string Note, string buttonValue)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var voucher = await _voucherRepository.GetVoucherByIdIncludeItems(VoucherId);
                    if (voucher == null)
                    {
                        return NotFound();
                    }

                    var cmpid = _cookieHelper.Get("cmpCookee");
                    var fYrId = _cookieHelper.Get("fYrCookee");
                    if (cmpid == null || fYrId == null)
                    {
                        return Redirect("/");
                    }
                    var cmpidG = Guid.Parse(cmpid);
                    var fYrIdG = Guid.Parse(fYrId);
                    await _voucherViewModelService.EditVoucherInvoice(data, data2, "Purchase Bill", Invoice, Date, termsDays, AccountId, VoucherId, Total, Note, voucher, cmpidG, fYrIdG, "Update");
                    return Json(new { success = true, newLocation = "/Accounts/Payment/Index/" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JournalEntryExists(VoucherId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    var msg = new ModelStateException(ex);
                    TempData["MessageToClientError"] = msg;
                    return Json(new { success = false, message = msg.Message });
                    //ModelState.AddModelError("", msg);                           
                }

            }
            return View();
        }

        // GET: MyBooks/Purchase/Delete/5
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

        // POST: MyBooks/Purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _voucherViewModelService.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        private bool JournalEntryExists(Guid id)
        {
            return _context.JournalEntries.Any(e => e.Id == id);
        }
    }
}
