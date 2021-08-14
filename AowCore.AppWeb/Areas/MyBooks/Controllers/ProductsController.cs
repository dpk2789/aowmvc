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
using System.Collections.Generic;
using AowCore.AppWeb.Helpers;
using AowCore.Application.IRepository;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class ProductsController : Controller
    {
        private readonly IApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICookieHelper _cookieHelper;
        public ProductsController(IApplicationDbContext context, ICookieHelper cookieHelper, IProductRepository productRepository)
        {
            _context = context;
            _cookieHelper = cookieHelper;
            _productRepository = productRepository;
        }

        public IEnumerable<ProductCategoryViewModel> GetCategories()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            var cmpidG = Guid.Parse(cmpid);
            var categoryList = _context.ProductCategories.Where(x => x.CompanyId == cmpidG && x.Type != "Tax").Select(a => new ProductCategoryViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                ParentCategoryId = a.ParentCategoryId
            }).ToList();
            return categoryList;
        }

        public async Task<IActionResult> Index()
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            var cmpidG = Guid.Parse(cmpid);
            var products = await _productRepository.GetProducts(cmpidG);
            var list = products.Where(x => x.ItemType != "Sundry Item");
            return View(list);
        }

        public async Task<JsonResult> GetProductsForAutocomplete(string term)
        {
            var cmpid = _cookieHelper.Get("cmpCookee");
            var cmpidG = Guid.Parse(cmpid);
            var productsMatching = String.IsNullOrWhiteSpace(term) ? null : await _productRepository.GetProductsByTerm(cmpidG, term);

            List<ProductViewModel> productViewModelList = new List<ProductViewModel>();
            foreach (var product in productsMatching.ToList())
            {
                ProductViewModel productViewModel = new ProductViewModel
                {
                    Id = product.Id,
                    ProductId = product.Id,
                    Code = product.Code,
                    Name = product.Name,
                    AutoGenerateName = product.AutoGenerateName,
                    SalePrice = product.SalePrice,
                    PurchasePrice = product.PurchasePrice,
                    Discription = product.Discription,
                    Percent = product.Percent,
                    ItemType = product.ItemType
                };
                //  productViewModel.AccountId = product.PurchaseAccountId;
                //  productViewModel.AccountCategoryName = product.PurchaseAccount.AccountCategory.Name != null ? product.PurchaseAccount.AccountCategory.Name.Replace(" ", string.Empty) : null;
                // productViewModel.StoreViewModels = product.StoreProducts.ToList();
                productViewModelList.Add(productViewModel);
            }


            return Json(productViewModelList.Select(m => new
            {
                id = m.Id,
                value = m.Name,
                label = String.Format("{0}/{1}/{2}", m.Code, m.Name, m.SalePrice),
                mRPPerUnit = m.SalePrice,
                CostPrice = m.PurchasePrice,
                name = m.Name,
                node = m.Code,
                accountCategoryName = m.AccountCategoryName != null ? m.AccountCategoryName.Replace(" ", string.Empty) : null,
                productAccountId = m.LedgerId,
                productId = m.ProductId,
                itemtype = m.ItemType != null ? m.ItemType.Replace(" ", string.Empty) : null,
            }));

        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Ledger)
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            var vm = new ProductViewModel();
            vm.ProductCategorySelectList = new SelectList(GetCategories(), "Id", "Name");
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel viewModel, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var product = new Product();
                product.Id = Guid.NewGuid();
                product.Name = viewModel.Name;
                product.ProductCategoryId = viewModel.ProductCategoryId;
                product.ItemType = viewModel.ItemType;
                product.ProductTaxCode = viewModel.ProductTaxCode;
                _context.Add(product);
                await _context.SaveChangesAsync(cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            viewModel.ProductCategorySelectList = new SelectList(GetCategories(), "Id", "Name", viewModel.ProductCategoryId);
            return View(viewModel);
        }

        // GET: MyBooks/Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            var viewModel = new ProductViewModel
            {
                Id = product.Id,
                ProductCategoryId = product.ProductCategoryId,
                Name = product.Name,
                LedgerId = product.LedgerId,
                ProductTaxCode = product.ProductTaxCode,
                Code = product.Code,
                ModelNumber = product.ModelNumber,
                Title = product.Title,
                Discription = product.Discription,
                Specifications = product.Specifications,
                ThumbMainImageName = product.ThumbMainImageName,
                CategoryName = product.ProductCategory.Name,
                SalePrice = product.SalePrice,
                PurchasePrice = product.PurchasePrice,
                ItemTypeId = product.ItemType
            };
            ViewBag.ProductCategoryId = new SelectList(_context.ProductCategories.Where(x => x.Type != "Tax"), "Id", "Name", product.ProductCategoryId);
            ViewBag.ItemTypeIdList = viewModel.getItemTypeList();
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel viewModel, CancellationToken cancellationToken)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Product product = await _context.Products.FindAsync(viewModel.Id);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    product.Name = viewModel.Name;
                    product.ProductCategoryId = viewModel.ProductCategoryId;
                    product.SalePrice = viewModel.SalePrice == null ? 0 : viewModel.SalePrice.Value;
                    product.PurchasePrice = viewModel.PurchasePrice == null ? 0 : viewModel.PurchasePrice.Value;
                    product.ItemType = viewModel.ItemTypeId;
                    product.ProductTaxCode = viewModel.ProductTaxCode;
                    product.TaxType = viewModel.TaxType;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_productRepository.ProductExistsAny(viewModel.Id))
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

            return View(viewModel);
        }

        // GET: MyBooks/Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: MyBooks/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                 .Include(p => p.ProductCategory).Include(p => p.ProductVariants)
                 .FirstOrDefaultAsync(m => m.Id == id);

            foreach (var varient in product.ProductVariants)
            {
                var vairentNOptions = await _context.ProductVariantProductAttributeOptions.Where(x => x.ProductVariantId == varient.Id).ToListAsync();
                foreach (var varientOpp in vairentNOptions)
                {
                    _context.ProductVariantProductAttributeOptions.Remove(varientOpp);
                }
                _context.ProductVariants.Remove(varient);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
    }
}
