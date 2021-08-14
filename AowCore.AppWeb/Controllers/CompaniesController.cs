using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AowCore.Application;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AowCore.AppWeb.Helpers;
using AowCore.Application.Services;
using AowCore.AppWeb.Mapping;
using Microsoft.EntityFrameworkCore;
using AowCore.AppWeb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using AowCore.Domain;
using AowCore.Domain.Region;
using System.Linq;
using System.Collections.Generic;

namespace AowCore.AppWeb.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICookieHelper _cookieHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompaniesController(ICompanyService companyService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,
            ICurrentUserService currentUserService, ICookieHelper cookieHelper)
        {
            _companyService = companyService;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _cookieHelper = cookieHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            _cookieHelper.Set("cmpCookee", "x", 60);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("user_id", "1");

            var userId = _currentUserService.UserId;
            var cmpList = new List<CompanyViewModel>();
            var userCompnies = await _companyService.GetCompaniesByUser(userId);
            foreach (var userCmp in userCompnies)
            {
                var cmp = await _companyService.GetCompanyById(userCmp.CompanyId);
                var viewModel = new CompanyViewModel();
                viewModel.Id = cmp.Id;
                viewModel.CompanyName = cmp.CompanyName;
                cmpList.Add(viewModel);
            }
            return View(cmpList);
        }

        public async Task<IActionResult> AllCompanies()
        {
            var cmpnies = await _companyService.GetAll();
            var cmpList = new List<CompanyViewModel>();
            foreach (var cmp in cmpnies)
            {               
                var viewModel = new CompanyViewModel();
                viewModel.Id = cmp.Id;
                viewModel.CompanyName = cmp.CompanyName;
                cmpList.Add(viewModel);
            }
            return View(cmpList);
        }

        public async Task<JsonResult> GetCountriesForAutocomplete(string term)
        {
            var countries = await _companyService.GetAllCountries();
            Country[] productsMatching = String.IsNullOrWhiteSpace(term) ? null
                : countries.Where(ii => ii.CountryName.Contains(term) || ii.Code.Contains(term)).ToArray();

            return Json(productsMatching.Select(m => new
            {
                id = m.Id,
                value = m.CountryName,
                label = $"{m.CountryName}: {m.Code}"
            }));
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _companyService.GetCompanyById(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyViewModel viewModelRequest, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                // var userId = _userManager.GetUserId(_currentUserService.GetUser());               
                var userId = _currentUserService.UserId;
                var company = new Company { CompanyName = viewModelRequest.CompanyName, TaxType = viewModelRequest.TaxType };
                var created = await _companyService.CreateCompany(company, userId, cancellationToken);
                if (created)
                    return RedirectToAction(nameof(Index));
            }
            return View(viewModelRequest);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _companyService.GetCompanyById(id.Value);
            if (company == null)
            {
                return NotFound();
            }
            var companyMapping = new CompanyMapping();
            var vm = companyMapping.DomainToResponse(company);
            ViewBag.Currency = new SelectList(vm.CurrencyList, "Value", "Text");
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CompanyViewModel viewModel, CancellationToken cancellationToken)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var companyMapping = new CompanyMapping();
                    var company = await _companyService.GetCompanyById(id);
                    var updated = companyMapping.ViewModelToDomain(company, viewModel);
                    var result = await _companyService.UpdateCompany(updated, cancellationToken);
                    if (result)
                        return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (CompanyExists(viewModel.Id).Result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(viewModel);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _companyService.GetCompanyById(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var company = await _companyService.GetCompanyById(id);
            await _companyService.DeleteCompany(company, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CompanyExists(Guid id)
        {
            var company = await _companyService.GetCompanyById(id);
            if (company == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
