using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AowCore.Domain;
using AowCore.Application;
using System.Threading;
using TreeUtility;
using System.Collections.Generic;
using AowCore.AppWeb.Helpers;
using AowCore.AppWeb.ViewModels;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    [RedirectingAction]
    public class ProductCategoriesController : Controller
    {
        private readonly IApplicationDbContext _context;
        private readonly ICookieHelper _cookieHelper;       

        public ProductCategoriesController(IApplicationDbContext context, ICookieHelper cookieHelper)
        {
            _context = context;           
            _cookieHelper = cookieHelper;           
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
                }
                categories.Add(c);
            }
            return categories;
        }

        private string EnumerateNodes(ProductCategory parent)
        {
            // Init an empty string
            string content = String.Empty;

            // Add <li> category name
            content += "<li class=\"treenode\">";
            content += parent.Name;
            content += String.Format("<a href=\"/MyBooks/ProductCategories/Edit/{0}\" class=\"btn btn-primary btn-xs treenodeeditbutton\">Edit</a>", parent.Id);
            content += String.Format("<a href=\"/MyBooks/ProductCategories/Delete/{0}\" class=\"btn btn-danger btn-xs treenodedeletebutton\">Delete</a>", parent.Id);

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
                    ProductCategory child = parent.Children[i];
                    content += EnumerateNodes(child);
                }

                // If this iteration's index points past the children, end the </ul>
                if (numberOfChildren > 0 && i == numberOfChildren)
                    content += "</ul>";
            }

            // Return the content
            return content;
        }
        public async Task<IActionResult> Index()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            if (string.IsNullOrEmpty(cmpid))
            {
                return Redirect("/");
            }
            // Start the outermost list
            string fullString = "<ul>";

            IList<ProductCategory> listOfNodes = await GetListOfNodes();
            IList<ProductCategory> topLevelCategories = TreeHelper.ConvertToForest(listOfNodes);

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

            var productCategory = await _context.ProductCategories
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        public IEnumerable<ProductCategoryViewModel> GetCategories()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            var cmpidG = Guid.Parse(cmpid);
            var categoryList = _context.ProductCategories.Where(x => x.CompanyId == cmpidG).Select(a => new ProductCategoryViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                ParentCategoryId = a.ParentCategoryId
            }).ToList();
            return categoryList;
        }
        private SelectList PopulateParentCategorySelectList(Guid? id)
        {
            SelectList selectList;

            if (id == null)
                selectList = new SelectList(GetCategories().Where(c => c.ParentCategoryId == null), "Id", "Name");
            else if (GetCategories().Count(c => c.ParentCategoryId == id) == 0)
                selectList = new SelectList(GetCategories().Where(c => c.ParentCategoryId == null && c.Id != id), "Id", "Name");
            else selectList = new SelectList(GetCategories(), "Id", "Name");
            return selectList;
        }

        public IActionResult Create()
        {
            ViewBag.ParentCategoryIdSelectList = PopulateParentCategorySelectList(null);
            //  var productCategoryViewModel = new ProductCategoryViewModel();         
            //var productAttributes = _context.ProductAttributes;
            //productCategoryViewModel.CategoryAttributesTags = productAttributes.Select(tag => new CategoryAttributesTag
            //{
            //    Id = tag.Id,
            //    Name = tag.Name,
            //    IsChecked = false
            //}).ToList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategoryViewModel viewModel, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var cmpid = _cookieHelper.Get("cmpCookee");
                var cmpidG = Guid.Parse(cmpid);
                var category = new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    ParentCategoryId = viewModel.ParentCategoryId,
                    Name = viewModel.Name,
                    CompanyId = cmpidG,
                    Type = "Voucher Item",
                };
            
                _context.Add(category);
                await _context.SaveChangesAsync(cancellationToken);
                return RedirectToAction(nameof(Index));
            }
          
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            ViewBag.ParentCategoryIdSelectList = PopulateParentCategorySelectList(productCategory.Id);
            return View(productCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductCategory productCategory, CancellationToken cancellationToken)
        {
            if (id != productCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCategory);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryExists(productCategory.Id))
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
            return View(productCategory);
        }

        // GET: MyBooks/ProductCategories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // POST: MyBooks/ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id);
            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryExists(Guid id)
        {
            return _context.ProductCategories.Any(e => e.Id == id);
        }
    }
}
