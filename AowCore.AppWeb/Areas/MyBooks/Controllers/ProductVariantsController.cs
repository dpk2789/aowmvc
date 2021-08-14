using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AowCore.Domain;
using AowCore.Application;
using System.Threading;
using AowCore.AppWeb.ViewModels;
using System.Collections.Generic;
using AowCore.AppWeb.Helpers;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using AowCore.Domain.Payroll;

namespace AowCore.AppWeb.Areas.MyBooks.Controllers
{
    [Area("MyBooks")]
    public class ProductVariantsController : Controller
    {
        private readonly IApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        private readonly ICookieHelper _cookieHelper;

        public ProductVariantsController(IApplicationDbContext context, IWebHostEnvironment environment, ICookieHelper cookieHelper)
        {
            _context = context;
            _environment = environment;
            _cookieHelper = cookieHelper;
        }

        // GET: MyBooks/ProductVariants
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductVariants.Include(p => p.Products);
            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariant = await _context.ProductVariants
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productVariant == null)
            {
                return NotFound();
            }

            return View(productVariant);
        }

        public IActionResult Upload()
        {

            return View();
        }

        public async Task<IActionResult> Import(string productName)
        {
            var resultForClient = new JsonResultClientSide();
            try
            {
                IFormFile file = Request.Form.Files[0];
                string path = Path.Combine(this._environment.WebRootPath, "Uploads");
                string fileName = file.FileName;
                string fileContentType = file.ContentType;
                var fileLocation = new FileInfo(path);
                FileStream stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);

                //IExcelDataReader reader = null;
                var cmpid = _cookieHelper.Get("cmpCookee");
                if (cmpid == null)
                {
                    return Redirect("/");
                }
                var cmpidG = Guid.Parse(cmpid);
                int i = 0;
                using var reader = ExcelReaderFactory.CreateReader(stream);
                DataSet result = reader.AsDataSet();
                //var result1 = reader.AsDataSet(new ExcelDataSetConfiguration()
                //{
                //    ConfigureDataTable = (data) => new ExcelDataTableConfiguration()
                //    {
                //        UseHeaderRow = true
                //    }
                //});
                var retriveProduct = _context.Products.Include(x => x.ProductCategory)
                    .FirstOrDefault(x => x.Name == productName && x.ProductCategory.CompanyId == cmpidG);
                var retriveAttributes = _context.ProductAttributes.
                                       Where(x => x.ProductCategoryId == retriveProduct.ProductCategory.Id);
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    var rows = row.Table.Rows[0];
                    string att1 = row.ItemArray[1].ToString();

                    ProductVariant varient = new ProductVariant();
                    Guid varientId = Guid.NewGuid();
                    varient.Id = varientId;
                    varient.ProductId = retriveProduct.Id;
                    string empFullName = string.Empty;
                    foreach (DataColumn col in result.Tables[0].Columns)
                    {
                        var value = row[col.ColumnName].ToString();

                        if (i == 0)
                        {

                        }
                        else
                        {
                            if (col.ColumnName == "Column0")
                            {
                                var retriveVarient = _context.ProductVariants.FirstOrDefault(x => x.Name == value && x.ProductId == retriveProduct.Id);
                                if (retriveVarient == null)
                                {
                                    varient.Name = value;
                                    _context.ProductVariants.Add(varient);
                                }
                                else
                                {
                                    varientId = retriveVarient.Id;
                                }
                            }
                            if (col.ColumnName == "Column1")
                            {
                                var rowsZero = row.Table.Rows[0];
                                string columnOneAtt = rowsZero.ItemArray[1].ToString();

                                var retriveAtt = retriveAttributes.FirstOrDefault(x => x.Name == columnOneAtt);
                                if (retriveAtt == null)
                                {
                                    Guid producesAttributeId = Guid.NewGuid();
                                    ProductAttribute producesAttribute = new ProductAttribute
                                    {
                                        Id = producesAttributeId,
                                        Name = columnOneAtt,
                                        ProductCategoryId = retriveProduct.ProductCategoryId
                                    };
                                    _context.ProductAttributes.Add(producesAttribute);

                                    var retriveAttrOption = _context.ProductAttributeOptions.
                                    FirstOrDefault(x => x.Name == value && x.ProductAttributeId == producesAttributeId);
                                    if (retriveAttrOption == null)
                                    {
                                        ProductAttributeOptions productAttributeOptions = new ProductAttributeOptions();
                                        productAttributeOptions.Id = Guid.NewGuid();
                                        productAttributeOptions.Name = value;
                                        productAttributeOptions.ProductAttributeId = producesAttributeId;
                                        _context.ProductAttributeOptions.Add(productAttributeOptions);
                                    }
                                }
                                else
                                {
                                    var retriveAttrOption = _context.ProductAttributeOptions.
                                    FirstOrDefault(x => x.Name == value && x.ProductAttributeId == retriveAtt.Id);
                                    if (retriveAttrOption == null)
                                    {
                                        ProductAttributeOptions productAttributeOptions = new ProductAttributeOptions();
                                        Guid produtAttributeOptionId = Guid.NewGuid();
                                        productAttributeOptions.Id = produtAttributeOptionId;
                                        productAttributeOptions.Name = value;
                                        productAttributeOptions.ProductAttributeId = retriveAtt.Id;
                                        _context.ProductAttributeOptions.Add(productAttributeOptions);

                                        ProductVariantProductAttributeOption productVariantProductAttributeOption = new ProductVariantProductAttributeOption();
                                        productVariantProductAttributeOption.Id = Guid.NewGuid();
                                        productVariantProductAttributeOption.ProductVariantId = varientId;
                                        productVariantProductAttributeOption.ProductAttributeOptionsId = produtAttributeOptionId;
                                        _context.ProductVariantProductAttributeOptions.Add(productVariantProductAttributeOption);
                                    }
                                    else
                                    {
                                        ProductVariantProductAttributeOption productVariantProductAttributeOption = new ProductVariantProductAttributeOption();
                                        productVariantProductAttributeOption.Id = Guid.NewGuid();
                                        productVariantProductAttributeOption.ProductVariantId = varientId;
                                        productVariantProductAttributeOption.ProductAttributeOptionsId = retriveAttrOption.Id;
                                        _context.ProductVariantProductAttributeOptions.Add(productVariantProductAttributeOption);
                                    }
                                }
                            }
                            if (col.ColumnName == "Column2")
                            {
                                var rowsZero = row.Table.Rows[0];
                                string columnTwoAtt = rowsZero.ItemArray[2].ToString();

                                var retriveAtt = retriveAttributes.FirstOrDefault(x => x.Name == columnTwoAtt);
                                if (retriveAtt == null)
                                {
                                    Guid producesAttributeId = Guid.NewGuid();
                                    ProductAttribute producesAttribute = new ProductAttribute
                                    {
                                        Id = producesAttributeId,
                                        Name = columnTwoAtt,
                                        ProductCategoryId = retriveProduct.ProductCategoryId
                                    };
                                    _context.ProductAttributes.Add(producesAttribute);

                                    var retriveAttrOption = _context.ProductAttributeOptions.
                                    FirstOrDefault(x => x.Name == value && x.ProductAttributeId == producesAttributeId);
                                    if (retriveAttrOption == null)
                                    {
                                        ProductAttributeOptions productAttributeOptions = new ProductAttributeOptions();
                                        productAttributeOptions.Id = Guid.NewGuid();
                                        productAttributeOptions.Name = value;
                                        productAttributeOptions.ProductAttributeId = producesAttributeId;
                                        _context.ProductAttributeOptions.Add(productAttributeOptions);
                                    }
                                }
                                else
                                {
                                    var retriveAttrOption = _context.ProductAttributeOptions.
                                    FirstOrDefault(x => x.Name == value && x.ProductAttributeId == retriveAtt.Id);
                                    if (retriveAttrOption == null)
                                    {
                                        ProductAttributeOptions productAttributeOptions = new ProductAttributeOptions();
                                        Guid produtAttributeOptionId = Guid.NewGuid();
                                        productAttributeOptions.Id = produtAttributeOptionId;
                                        productAttributeOptions.Name = value;
                                        productAttributeOptions.ProductAttributeId = retriveAtt.Id;
                                        _context.ProductAttributeOptions.Add(productAttributeOptions);

                                        ProductVariantProductAttributeOption productVariantProductAttributeOption = new ProductVariantProductAttributeOption();
                                        productVariantProductAttributeOption.Id = Guid.NewGuid();
                                        productVariantProductAttributeOption.ProductVariantId = varientId;
                                        productVariantProductAttributeOption.ProductAttributeOptionsId = produtAttributeOptionId;
                                        _context.ProductVariantProductAttributeOptions.Add(productVariantProductAttributeOption);
                                    }
                                    else
                                    {
                                        ProductVariantProductAttributeOption productVariantProductAttributeOption = new ProductVariantProductAttributeOption();
                                        productVariantProductAttributeOption.Id = Guid.NewGuid();
                                        productVariantProductAttributeOption.ProductVariantId = varientId;
                                        productVariantProductAttributeOption.ProductAttributeOptionsId = retriveAttrOption.Id;
                                        _context.ProductVariantProductAttributeOptions.Add(productVariantProductAttributeOption);
                                    }
                                }
                            }
                            if (col.ColumnName == "Column3")
                            {
                                var rowsZero = row.Table.Rows[0];
                                string columnThreeAtt = rowsZero.ItemArray[3].ToString();

                                var retriveAtt = retriveAttributes.FirstOrDefault(x => x.Name == columnThreeAtt);
                                if (retriveAtt == null)
                                {
                                    Guid producesAttributeId = Guid.NewGuid();
                                    ProductAttribute producesAttribute = new ProductAttribute
                                    {
                                        Id = producesAttributeId,
                                        Name = columnThreeAtt,
                                        ProductCategoryId = retriveProduct.ProductCategoryId
                                    };
                                    _context.ProductAttributes.Add(producesAttribute);

                                    var retriveAttrOption = _context.ProductAttributeOptions.
                                    FirstOrDefault(x => x.Name == value && x.ProductAttributeId == producesAttributeId);
                                    if (retriveAttrOption == null)
                                    {
                                        ProductAttributeOptions productAttributeOptions = new ProductAttributeOptions();
                                        productAttributeOptions.Id = Guid.NewGuid();
                                        productAttributeOptions.Name = value;
                                        productAttributeOptions.ProductAttributeId = producesAttributeId;
                                        _context.ProductAttributeOptions.Add(productAttributeOptions);
                                    }
                                }
                                else
                                {
                                    var retriveAttrOption = _context.ProductAttributeOptions.
                                    FirstOrDefault(x => x.Name == value && x.ProductAttributeId == retriveAtt.Id);
                                    if (retriveAttrOption == null)
                                    {
                                        ProductAttributeOptions productAttributeOptions = new ProductAttributeOptions();
                                        Guid produtAttributeOptionId = Guid.NewGuid();
                                        productAttributeOptions.Id = produtAttributeOptionId;
                                        productAttributeOptions.Name = value;
                                        productAttributeOptions.ProductAttributeId = retriveAtt.Id;
                                        _context.ProductAttributeOptions.Add(productAttributeOptions);

                                        ProductVariantProductAttributeOption productVariantProductAttributeOption = new ProductVariantProductAttributeOption();
                                        productVariantProductAttributeOption.Id = Guid.NewGuid();
                                        productVariantProductAttributeOption.ProductVariantId = varientId;
                                        productVariantProductAttributeOption.ProductAttributeOptionsId = produtAttributeOptionId;
                                        _context.ProductVariantProductAttributeOptions.Add(productVariantProductAttributeOption);
                                    }
                                    else
                                    {
                                        ProductVariantProductAttributeOption productVariantProductAttributeOption = new ProductVariantProductAttributeOption();
                                        productVariantProductAttributeOption.Id = Guid.NewGuid();
                                        productVariantProductAttributeOption.ProductVariantId = varientId;
                                        productVariantProductAttributeOption.ProductAttributeOptionsId = retriveAttrOption.Id;
                                        _context.ProductVariantProductAttributeOptions.Add(productVariantProductAttributeOption);
                                    }
                                }
                            }
                            if (col.ColumnName == "Column4")
                            {
                                var rowsZero = row.Table.Rows[0];
                                string columnFourAtt = rowsZero.ItemArray[4].ToString();

                                var retriveAtt = retriveAttributes.FirstOrDefault(x => x.Name == columnFourAtt);
                                if (retriveAtt == null)
                                {
                                    Guid producesAttributeId = Guid.NewGuid();
                                    ProductAttribute producesAttribute = new ProductAttribute
                                    {
                                        Id = producesAttributeId,
                                        Name = columnFourAtt,
                                        ProductCategoryId = retriveProduct.ProductCategoryId
                                    };
                                    _context.ProductAttributes.Add(producesAttribute);

                                    var retriveAttrOption = _context.ProductAttributeOptions.
                                    FirstOrDefault(x => x.Name == value && x.ProductAttributeId == producesAttributeId);
                                    if (retriveAttrOption == null)
                                    {
                                        ProductAttributeOptions productAttributeOptions = new ProductAttributeOptions();
                                        productAttributeOptions.Id = Guid.NewGuid();
                                        productAttributeOptions.Name = value;
                                        productAttributeOptions.ProductAttributeId = producesAttributeId;
                                        _context.ProductAttributeOptions.Add(productAttributeOptions);
                                    }
                                }
                                else
                                {
                                    var retriveAttrOption = _context.ProductAttributeOptions.
                                    FirstOrDefault(x => x.Name == value && x.ProductAttributeId == retriveAtt.Id);
                                    if (retriveAttrOption == null)
                                    {
                                        ProductAttributeOptions productAttributeOptions = new ProductAttributeOptions();
                                        Guid produtAttributeOptionId = Guid.NewGuid();
                                        productAttributeOptions.Id = produtAttributeOptionId;
                                        productAttributeOptions.Name = value;
                                        productAttributeOptions.ProductAttributeId = retriveAtt.Id;
                                        _context.ProductAttributeOptions.Add(productAttributeOptions);

                                        ProductVariantProductAttributeOption productVariantProductAttributeOption = new ProductVariantProductAttributeOption();
                                        productVariantProductAttributeOption.Id = Guid.NewGuid();
                                        productVariantProductAttributeOption.ProductVariantId = varientId;
                                        productVariantProductAttributeOption.ProductAttributeOptionsId = produtAttributeOptionId;
                                        _context.ProductVariantProductAttributeOptions.Add(productVariantProductAttributeOption);
                                    }
                                    else
                                    {
                                        ProductVariantProductAttributeOption productVariantProductAttributeOption = new ProductVariantProductAttributeOption();
                                        productVariantProductAttributeOption.Id = Guid.NewGuid();
                                        productVariantProductAttributeOption.ProductVariantId = varientId;
                                        productVariantProductAttributeOption.ProductAttributeOptionsId = retriveAttrOption.Id;
                                        _context.ProductVariantProductAttributeOptions.Add(productVariantProductAttributeOption);
                                    }
                                }
                            }
                        }
                    }
                    i++;
                    await _context.SaveChangesAsync();
                }
                reader.Close();
                return Json(new { resultForClient });

            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                resultForClient.Msg = ex.InnerException.Message;

            }
            return Json(new { resultForClient });

        }

        // GET: MyBooks/ProductVariants/Create
        public IActionResult Create(Guid? productId)
        {
            var product = _context.Products.Where(c => c.Id == productId).Include(p => p.ProductCategory).FirstOrDefault();
            var viewModel = new ProductVariantsViewModel();
            viewModel.ProductId = productId.Value;
            var attributes = _context.ProductAttributes.Where(x => x.ProductCategoryId == product.ProductCategoryId).Include(p => p.ProductAttributeOptions).ToList();

            viewModel.AttributesViewModels = attributes.Select(tag => new AttributesViewModel
            {
                Id = tag.Id,
                Name = tag.Name,
                AttributesOptionsViewModels = tag.ProductAttributeOptions.Select(x => new AttributesOptionsViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }),
                IsChecked = false
            }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVariantsViewModel viewModel, Guid[] OptionsSelectedOnView, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var productVariant = new ProductVariant();
                productVariant.Id = Guid.NewGuid();
                productVariant.ProductId = viewModel.ProductId.Value;
                productVariant.Name = viewModel.Name;
                _context.Add(productVariant);
                if (OptionsSelectedOnView != null)
                {
                    foreach (var option in OptionsSelectedOnView)
                    {
                        var optionVarient = new ProductVariantProductAttributeOption();
                        optionVarient.Id = Guid.NewGuid();
                        optionVarient.ProductAttributeOptionsId = option;
                        optionVarient.ProductVariantId = productVariant.Id;
                        await _context.AddAsync(optionVarient);
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
                return RedirectToAction("Edit", "Products", new { id = productVariant.ProductId });
            }
            return View(viewModel);
        }

        // GET: MyBooks/ProductVariants/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariant = await _context.ProductVariants.Where(x => x.Id == id).Include(p => p.ProductVariantProductAttributeOptions).
                Include(p => p.Products.ProductCategory.ProductAttributes).FirstOrDefaultAsync();
            if (productVariant == null)
            {
                return NotFound();
            }
            var attoptions = await _context.ProductAttributeOptions.
                Where(x => x.ProductAttributes.ProductCategoryId == productVariant.Products.ProductCategoryId).ToListAsync();
            var viewModel = new ProductVariantsViewModel();
            viewModel.ProductId = productVariant.ProductId;
            viewModel.Name = productVariant.Name;
            viewModel.Size = productVariant.Size;
            viewModel.Discription = productVariant.Discription;
            viewModel.CostPrice = productVariant.CostPrice;
            viewModel.MRP = productVariant.MRP;
            viewModel.SalePrice = productVariant.SalePrice;
            var attviewModelList = new List<AttributesViewModel>();
            var varientsInPvPat = await _context.ProductVariantProductAttributeOptions.Where(x => x.ProductVariantId == productVariant.Id).ToListAsync();

            foreach (var att in productVariant.Products.ProductCategory.ProductAttributes.OrderBy(x => x.Name))
            {
                var attviewModel = new AttributesViewModel();
                attviewModel.Id = att.Id;
                attviewModel.Name = att.Name;
                var optionsViewModelList = new List<AttributesOptionsViewModel>();
                foreach (var attributeOption in att.ProductAttributeOptions)
                {
                    var optionsViewModel = new AttributesOptionsViewModel();
                    optionsViewModel.Id = attributeOption.Id;
                    optionsViewModel.Name = attributeOption.Name;
                    if (varientsInPvPat.Any(x => x.ProductAttributeOptionsId == attributeOption.Id))
                    {
                        optionsViewModel.IsChecked = true;
                    }
                    optionsViewModelList.Add(optionsViewModel);
                }
                attviewModel.AttributesOptionsViewModels = optionsViewModelList;
                attviewModelList.Add(attviewModel);
            }
            viewModel.AttributesViewModels = attviewModelList;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductVariantsViewModel viewModel, Guid[] OptionsSelectedOnView)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var retriveProductVariant = await _context.ProductVariants.Where(x => x.Id == viewModel.Id)
                        .Include(p => p.ProductVariantProductAttributeOptions).FirstOrDefaultAsync();
                    retriveProductVariant.Name = viewModel.Name;
                    // var productOptions = _context.ProductAttributeOptions.Where(x => x.ProductAttributes.ProductId == productVariant.ProductId);


                    foreach (var option in retriveProductVariant.ProductVariantProductAttributeOptions)
                    {
                        var productVariantProductAttributeOptions = await _context.ProductVariantProductAttributeOptions.Where(x => x.Id == option.Id).FirstOrDefaultAsync();
                        _context.ProductVariantProductAttributeOptions.Remove(productVariantProductAttributeOptions);
                    }
                    if (OptionsSelectedOnView != null)
                    {
                        foreach (var option in OptionsSelectedOnView)
                        {
                            var optionVarient = new ProductVariantProductAttributeOption();
                            optionVarient.Id = Guid.NewGuid();
                            optionVarient.ProductAttributeOptionsId = option;
                            optionVarient.ProductVariantId = viewModel.Id;
                            await _context.ProductVariantProductAttributeOptions.AddAsync(optionVarient);
                        }
                    }
                    _context.Update(retriveProductVariant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductVariantExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Products", new { id = viewModel.ProductId });
            }
            return View(viewModel);
        }

        // GET: MyBooks/ProductVariants/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVariant = await _context.ProductVariants
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productVariant == null)
            {
                return NotFound();
            }

            return View(productVariant);
        }

        // POST: MyBooks/ProductVariants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var productVariant = await _context.ProductVariants.FindAsync(id);
            _context.ProductVariants.Remove(productVariant);
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductVariantExists(Guid id)
        {
            return _context.ProductVariants.Any(e => e.Id == id);
        }
    }
}






//viewModel.AttributesViewModels = productVariant.Products.ProductCategory.ProductAttributes.OrderBy(x => x.Name).Select(att => new AttributesViewModel
//            {
//                Id = att.Id,
//                Name = att.Name,
//               // AttributesOptionsViewModels = att.ProductAttributeOptions,
//                AttributesOptionsViewModels = att.ProductAttributeOptions.OrderBy(x => x.Name).Select(x => new AttributesOptionsViewModel
//                {
//                    Id = x.Id,
//                    Name = x.Name,
//                    IsChecked = productVariant.ProductVariantProductAttributeOptions.Contains(x.ProductVariantProductAttributeOptions),
//                }),
//                // IsChecked = false,
//            }).ToList();




