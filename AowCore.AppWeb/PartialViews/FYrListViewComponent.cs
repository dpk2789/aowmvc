using AowCore.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AowCore.AppWeb.PartialViews
{
    public class FYrListViewComponent : ViewComponent
    {
        private readonly IApplicationDbContext _context;

        public FYrListViewComponent(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(Guid? id)
        {
            var fyrList = await _context.FinancialYears.Where(x => x.CompanyId == id.Value).ToListAsync();
            ViewBag.CompanyId = id;
            return View("Index", fyrList);
        }
    }
}
