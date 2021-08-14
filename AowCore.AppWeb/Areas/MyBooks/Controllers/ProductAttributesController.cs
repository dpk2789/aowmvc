using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AowCore.Domain;
using AowCore.Application;
using System.Threading;
using AowCore.AppWeb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using AowCore.AppWeb.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class ProductAttributesController : Controller
    {
        private readonly IApplicationDbContext _context;
        private readonly ICookieHelper _cookieHelper;
        private readonly ICompositeViewEngine _viewEngine;

        public ProductAttributesController(IApplicationDbContext context, ICookieHelper cookieHelper, ICompositeViewEngine viewEngine)
        {
            _context = context;
            _cookieHelper = cookieHelper;
            _viewEngine = viewEngine;
        }
       
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductAttributes.ToListAsync());
        }

        public async Task<IActionResult> Details()
        {
            var items = await _context.Products.ToListAsync();
            ViewBag.ProductId = new SelectList(items, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> ItemVarientsSearchPage(Guid? productId)
        {
            if (productId == null)
            {
                return RedirectToAction("Details");
            }
            var searchPageViewModel = new ItemVarientsSearchViewModel();
            var product = await _context.Products.Where(x => x.Id == productId).Include(p => p.ProductVariants).FirstOrDefaultAsync();
            var productsAttributes = await _context.ProductAttributes.Where(x => x.ProductCategoryId == product.ProductCategoryId)
                                            .Include(p => p.ProductAttributeOptions).ToListAsync();
            var productVariantsViewModelList = new List<ProductVariantsViewModel>();
            var attributesViewModelList = new List<AttributesViewModel>();
            foreach (var att in productsAttributes)
            {
                searchPageViewModel.AttributesViewModel = productsAttributes.Select(tag => new AttributesViewModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    AttributesOptionsViewModels = tag.ProductAttributeOptions.Select(x => new AttributesOptionsViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsChecked = false,
                    }),
                    // IsChecked = false,
                }).ToList();
            }
            foreach (var variant in product.ProductVariants)
            {
                ProductVariantsViewModel viewModel = new ProductVariantsViewModel();
                viewModel.Id = variant.Id;
                viewModel.Name = variant.Name;
                viewModel.Size = variant.Size;
                viewModel.CostPrice = variant.CostPrice;
                viewModel.SalePrice = variant.SalePrice;
                viewModel.MRP = variant.MRP;
                productVariantsViewModelList.Add(viewModel);
            }
            searchPageViewModel.ProductVariantsViewModel = productVariantsViewModelList;
            // searchPageViewModel.AttributesViewModel = attributesViewModelList;
            //return PartialView("_Index", productVariantsViewModelList);

            return View(searchPageViewModel);
        }

        public async Task<IActionResult> ItemVarientsSearchPageReport(string data)
        {
            ItemVarientsSearchViewModel searchPageViewModel = new ItemVarientsSearchViewModel();

            var productVariantsViewModelList = new List<ProductVariantsViewModel>();
            var attributesViewModelList = new List<AttributesViewModel>();
            var deserialiseList = JsonConvert.DeserializeObject<List<ProductVariantsViewModel>>(data);

            foreach (var option in deserialiseList)
            {
                var attOption = await _context.ProductAttributeOptions.Where(x => x.Id == option.Id).Include(p => p.ProductVariantProductAttributeOptions).FirstOrDefaultAsync();
                if (attOption != null)
                {
                    foreach (var varientNOption in attOption.ProductVariantProductAttributeOptions)
                    {
                        var varient = await _context.ProductVariants.Where(x => x.Id == varientNOption.ProductVariantId).FirstOrDefaultAsync();
                        var viewModel = new ProductVariantsViewModel();
                        viewModel.Id = varient.Id;
                        viewModel.Name = varient.Name;
                        productVariantsViewModelList.Add(viewModel);

                    }
                }
            }
            searchPageViewModel.ProductVariantsViewModel = productVariantsViewModelList;
            // searchPageViewModel.AttributesViewModel = attributesViewModelList;
            //return PartialView("_Index", productVariantsViewModelList);

            string modelString = await RenderRazorViewToString("_attSeachReport", searchPageViewModel);
            return Json(new { success = true, modelString });
        }

        public async Task<string> RenderRazorViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;
            using StringWriter writer = new StringWriter();
            ViewEngineResult viewResult =
                _viewEngine.FindView(ControllerContext, viewName, false);

            ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, writer, new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);

            return writer.GetStringBuilder().ToString();
        }
        public IActionResult Create(Guid? categoryId)
        {
            var productAttribute = new ProductsAttributeViewModel();
            productAttribute.ProductCategoryId = categoryId.Value;
            //ViewBag.ProductId = PopulateParentCategorySelectList(null);
            return View(productAttribute);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductsAttributeViewModel viewModel, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var productAttribute = new ProductAttribute();
                productAttribute.Id = Guid.NewGuid();
                productAttribute.ProductCategoryId = viewModel.ProductCategoryId;
                productAttribute.Name = viewModel.Name;
                _context.Add(productAttribute);
                await _context.SaveChangesAsync(cancellationToken);
                return RedirectToAction("Edit", "ProductCategories", new { id = productAttribute.ProductCategoryId });
            }
            return View(viewModel);
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
            selectList = new SelectList(GetCategories(), "Id", "Name", id.Value);
            return selectList;
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttribute = await _context.ProductAttributes.Include(i => i.ProductAttributeOptions).FirstOrDefaultAsync(i => i.Id == id.Value);
            if (productAttribute == null)
            {
                return NotFound();
            }
            ViewBag.ProductCategoryId = PopulateParentCategorySelectList(productAttribute.ProductCategoryId);
            var productsAttributeViewModel = new ProductsAttributeViewModel();
            productsAttributeViewModel.Id = productAttribute.Id;
            productsAttributeViewModel.Name = productAttribute.Name;
            productsAttributeViewModel.ProductCategoryId = productAttribute.ProductCategoryId;
            //  productsAttributeViewModel.AttributesOptions = productAttribute.ProductAttributeOptions;
            if (productAttribute.ProductAttributeOptions != null)
            {
                productsAttributeViewModel.AttributesOptions = productAttribute.ProductAttributeOptions.OrderBy(x => x.Name).Select(tag => new AttributesOptions
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    IsChecked = false
                }).ToList();
            }
            return View(productsAttributeViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductsAttributeViewModel viewModel, string[] AttributeOptions, CancellationToken cancellationToken)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productAttribute = await _context.ProductAttributes.FindAsync(id);
                    productAttribute.Name = viewModel.Name;
                    productAttribute.ProductCategoryId = viewModel.ProductCategoryId;
                    if (productAttribute == null)
                    {
                        return NotFound();
                    }
                    if (AttributeOptions != null)
                    {
                        foreach (var option in AttributeOptions)
                        {
                            var attributeOption = new ProductAttributeOptions();
                            attributeOption.Id = Guid.NewGuid();
                            attributeOption.ProductAttributeId = viewModel.Id;
                            attributeOption.Name = option;
                            //  ProductAttributeOptions retriveAttributeOption = attributeOptionsList.Where(t => t.Name == tag).FirstOrDefault();
                            // newBlog.Tags.Add(retriveTag);
                            _context.ProductAttributeOptions.Add(attributeOption);
                        }
                    }
                    _context.Update(productAttribute);
                    await _context.SaveChangesAsync(cancellationToken);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductAttributeExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "ProductCategories", new { id = viewModel.ProductCategoryId, tabindex = 2 });
            }
            return View(viewModel);
        }

        // GET: MyBooks/ProductAttributes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttribute = await _context.ProductAttributes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productAttribute == null)
            {
                return NotFound();
            }

            return View(productAttribute);
        }

        // POST: MyBooks/ProductAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var productAttribute = await _context.ProductAttributes.FindAsync(id);
            _context.ProductAttributes.Remove(productAttribute);
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction("Edit", "ProductCategories", new { id = productAttribute.ProductCategoryId });
        }

        private bool ProductAttributeExists(Guid id)
        {
            return _context.ProductAttributes.Any(e => e.Id == id);
        }
    }
}
