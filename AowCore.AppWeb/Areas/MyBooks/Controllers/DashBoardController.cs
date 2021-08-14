using System;
using System.Threading.Tasks;
using AowCore.Application;
using AowCore.AppWeb.Helpers;
using AowCore.AppWeb.ViewModels;
using AowCore.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class DashBoardController : Controller
    {
        private readonly ICookieHelper _cookieHelper;
        private readonly IApplicationDbContext _context;
        public DashBoardController(IApplicationDbContext context, ICookieHelper cookieHelper)
        {
            _context = context;
            _cookieHelper = cookieHelper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Blogs()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(Guid companyId, Guid financialYearId, string themeId)
        {
            _cookieHelper.Set("cmpCookee", companyId.ToString(), 60);
            _cookieHelper.Set("fYrCookee", financialYearId.ToString(), 60);
            return Json(new { success = true, newLocation = "/MyBooks/DashBoard/DashBoard/" });
        }

        public async Task<IActionResult> DashBoard()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            var fid = _cookieHelper.Get("fYrCookee");

            if (string.IsNullOrEmpty(cmpid) && string.IsNullOrEmpty(fid))
            {
                return Redirect("/");
            }

            Guid fyrId = Guid.Parse(fid);
            Guid cmpidG = Guid.Parse(cmpid);
            Company company = await _context.Companies.FindAsync(cmpidG);
            FinancialYear fyr = await _context.FinancialYears.FindAsync(fyrId);
            DashboardViewModel dashboardViewModel = new DashboardViewModel
            {
                CompanyName = company.CompanyName,
                FyrName = string.Format("{0} - {1}", fyr.Start.Value.ToString("yyyy-MM-dd"), fyr.End.Value.ToString("yyyy-MM-dd")).Trim()
            };
            return View(dashboardViewModel);
        }

        public IActionResult CookieCheck(string returnUrl)
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            var fid = _cookieHelper.Get("fYrCookee");

            if (string.IsNullOrEmpty(cmpid) && string.IsNullOrEmpty(fid))
            {
                return Redirect("/");
            }
            return RedirectToPage(returnUrl);
        }
    }
}