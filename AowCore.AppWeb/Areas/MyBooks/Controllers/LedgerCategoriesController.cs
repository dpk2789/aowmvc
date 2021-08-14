using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AowCore.Domain;
using AowCore.AppWeb.ViewModels;
using AowCore.Application;
using AowCore.AppWeb.Helpers;
using TreeUtility;
using System.Threading;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class LedgerCategoriesController : Controller
    {
        private readonly IApplicationDbContext _context;
        private readonly ICookieHelper _cookieHelper;
        public LedgerCategoriesController(IApplicationDbContext context, ICookieHelper cookieHelper)
        {
            _cookieHelper = cookieHelper;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            var cmpidG = Guid.Parse(cmpid);
            var categories = await _context.LedgerCategories.Include(l => l.Parent).Where(x => x.CompanyId == cmpidG).ToListAsync();
            return View(categories);
        }

        public async Task<IList<LedgerCategoryViewModel>> GetCategories()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            var categoryList = new List<LedgerCategoryViewModel>();
            if (cmpid != null)
            {
                var cmpidG = Guid.Parse(cmpid);
                categoryList = await _context.LedgerCategories.Where(x => x.CompanyId == cmpidG).Select(a => new LedgerCategoryViewModel()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Type = a.Type,
                    ParentCategoryId = a.ParentCategoryId
                }).OrderBy(x => x.Name).ToListAsync();
                return categoryList;
            }
            return categoryList;
        }

        private async Task<List<LedgerCategoryViewModel>> GetListOfNodes()
        {
            var sourceCategories = await GetCategories();
            List<LedgerCategoryViewModel> categories = new List<LedgerCategoryViewModel>();

            foreach (var sourceCategory in sourceCategories)
            {
                var c = new LedgerCategoryViewModel();
                c.Id = sourceCategory.Id;
                c.Name = sourceCategory.Name;
                c.Type = sourceCategory.Type;
                //c.OpeningBalance = sourceCategory.OpeningBalance;            

                if (sourceCategory.ParentCategoryId != null)
                {
                    c.Parent = new LedgerCategoryViewModel();
                    c.Parent.Id = (Guid)sourceCategory.ParentCategoryId;
                }
                categories.Add(c);
            }
            return categories;
        }

        private string EnumerateNodes(LedgerCategoryViewModel parent)
        {
            // Init an empty string
            string content = String.Empty;

            // Add <li> category name
            content += "<li class=\"treenode\">";
            content += parent.Name;
            if (parent.Type != "Master")
            {
                content += String.Format("&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"/MyBooks/LedgerCategories/Edit/{0}\" class=\"btn btn-primary btn-sm treenodeeditbutton\">Edit</a>", parent.Id);
                content += String.Format("&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"/MyBooks/LedgerCategories/Delete/{0}\" class=\"btn btn-danger btn-sm treenodedeletebutton\">Delete</a>", parent.Id);

            }


            // If there are no children, end the </li>
            if (parent.Children.Count == 0)
                content += "</li>";
            else   // If there are children, start a <ul>
                content += "<ul>";

            // Loop one past the number of children
            int numberOfChildren = parent.Children.Count;
            for (int i = 0; i <= numberOfChildren; i++)
            {
                // If this iteration's index points to a child,
                // call this function recursively
                if (numberOfChildren > 0 && i < numberOfChildren)
                {
                    LedgerCategoryViewModel child = parent.Children[i];
                    content += EnumerateNodes(child);
                }

                // If this iteration's index points past the children, end the </ul>
                if (numberOfChildren > 0 && i == numberOfChildren)
                    content += "</ul>";
            }

            // Return the content
            return content;
        }
        public async Task<ActionResult> HierarchyOfLedgers()
        {
            // Start the outermost list
            string fullString = "<ul>";

            IList<LedgerCategoryViewModel> listOfNodes = await GetListOfNodes();
            IList<LedgerCategoryViewModel> topLevelCategories = TreeHelper.ConvertToForest(listOfNodes);

            foreach (var category in topLevelCategories)
                fullString += EnumerateNodes(category);

            // End the outermost list
            fullString += "</ul>";

            return View((object)fullString);
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledgerCategory = await _context.LedgerCategories
                .Include(l => l.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ledgerCategory == null)
            {
                return NotFound();
            }

            return View(ledgerCategory);
        }
        private void ValidateParentsAreParentless(LedgerCategoryViewModel category)
        {
            // There is no parent
            if (category.ParentCategoryId == null)
                return;

            // The parent has a parent
            LedgerCategory parentCategory = _context.LedgerCategories.Find(category.ParentCategoryId);
            if (parentCategory.ParentCategoryId != null)
                // throw new InvalidOperationException("You cannot nest this category more than two levels deep.");
                return;

            // The parent does NOT have a parent, but the category being nested has children
            int numberOfChildren = _context.LedgerCategories.Count(c => c.ParentCategoryId == category.Id);
            if (numberOfChildren > 0)
                throw new InvalidOperationException("You cannot nest this category's children more than two levels deep.");
        }
        private async Task<SelectList> PopulateParentCategorySelectList(Guid? id)
        {
            SelectList selectList;
            var categories = await GetCategories();
            if (id == null)
                selectList = new SelectList(categories.Where(c => c.ParentCategoryId == null), "Id", "Name");
            else if (categories.Count(c => c.ParentCategoryId == id) == 0)
                selectList = new SelectList(categories.Where(c => c.ParentCategoryId == null && c.Id != id), "Id", "Name");
            else selectList = new SelectList(categories, "Id", "Name");
            return selectList;
        }
        public async Task<IActionResult> Create()
        {
            ViewData["ParentCategoryIdSelectList"] = new SelectList(await GetCategories(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LedgerCategoryViewModel viewModel, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ValidateParentsAreParentless(viewModel);
                    var cmpid = _cookieHelper.Get("cmpCookee");
                    var cmpidG = Guid.Parse(cmpid);
                    var category = new LedgerCategory();
                    category.Id = Guid.NewGuid();
                    category.ParentCategoryId = viewModel.ParentCategoryId;
                    category.Name = viewModel.Name;
                    category.Code = viewModel.Code;
                    category.Type = "User";
                    category.CompanyId = cmpidG;
                    _context.LedgerCategories.Add(category);

                    await _context.SaveChangesAsync(cancellationToken);
                    return RedirectToAction("HierarchyOfLedgers");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ViewData["ParentCategoryIdSelectList"] = new SelectList(await GetCategories(), "Id", "Name", viewModel.ParentCategoryId);
                    return View(viewModel);
                }

            }

            ViewBag.ParentCategoryIdSelectList = PopulateParentCategorySelectList(null);
            return View(viewModel);
        }

        // GET: MyBooks/LedgerCategories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledgerCategory = await _context.LedgerCategories.FindAsync(id);
            if (ledgerCategory == null)
            {
                return NotFound();
            }
            ViewData["ParentCategoryIdSelectList"] = new SelectList(await GetCategories(), "Id", "Name", ledgerCategory.ParentCategoryId);
            return View(ledgerCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, LedgerCategory ledgerCategory, CancellationToken cancellationToken)
        {
            if (id != ledgerCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ledgerCategory);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LedgerCategoryExists(ledgerCategory.Id))
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
            return View(ledgerCategory);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ledgerCategory = await _context.LedgerCategories
                .Include(l => l.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ledgerCategory == null)
            {
                return NotFound();
            }

            return View(ledgerCategory);
        }

        // POST: MyBooks/LedgerCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var ledgerCategory = await _context.LedgerCategories.FindAsync(id);
            _context.LedgerCategories.Remove(ledgerCategory);
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        private bool LedgerCategoryExists(Guid id)
        {
            return _context.LedgerCategories.Any(e => e.Id == id);
        }
    }
}
