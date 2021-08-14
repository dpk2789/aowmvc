using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AowCore.Domain;
using AowCore.Application;
using System.Threading;
using AowCore.AppWeb.ViewModels;

namespace AowCore.AppWeb.Controllers
{
    public class FinancialYearsController : Controller
    {
        private readonly IApplicationDbContext _context;

        public FinancialYearsController(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FinancialYears.Include(f => f.Company);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialYear = await _context.FinancialYears
                .Include(f => f.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (financialYear == null)
            {
                return NotFound();
            }

            return View(financialYear);
        }

        public IActionResult Create(Guid? companyId)
        {
            FinancialYearViewModel viewModel = new FinancialYearViewModel
            {
                CompanyId = companyId.Value
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FinancialYear financialYear, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                financialYear.Id = Guid.NewGuid();
                financialYear.IsLocked = false;
                _context.FinancialYears.Add(financialYear);
                await _context.SaveChangesAsync(cancellationToken);
                return RedirectToAction("Edit", "Companies", new { id = financialYear.CompanyId });
            }

            return View(financialYear);
        }

        // GET: FinancialYears/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialYear = await _context.FinancialYears.FindAsync(id);
            if (financialYear == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", financialYear.CompanyId);
            return View(financialYear);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FinancialYear financialYear, CancellationToken cancellationToken)
        {
            if (id != financialYear.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.FinancialYears.Update(financialYear);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinancialYearExists(financialYear.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Companies", new { id = financialYear.CompanyId });
            }
            return View(financialYear);
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialYear = await _context.FinancialYears
                .Include(f => f.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (financialYear == null)
            {
                return NotFound();
            }

            return View(financialYear);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var financialYear = await _context.FinancialYears
                .Include(f => f.Vouchers)
                .FirstOrDefaultAsync(m => m.Id == id);
            foreach (var voucher in financialYear.Vouchers)
            {
                _context.Vouchers.Remove(voucher);
            }
            _context.FinancialYears.Remove(financialYear);
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction("Edit", "Companies", new { id = financialYear.CompanyId });
        }

        private bool FinancialYearExists(Guid id)
        {
            return _context.FinancialYears.Any(e => e.Id == id);
        }
    }
}
