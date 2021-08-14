using AowCore.Application;
using AowCore.AppWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AowCore.AppWeb.PartialViews
{
    public class ItemVariantsViewComponent : ViewComponent
    {
        private readonly IApplicationDbContext _context;

        public ItemVariantsViewComponent(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(Guid? id)
        {
            var varients = await _context.ProductVariants.Where(x => x.ProductId == id.Value).Include(p => p.ProductVariantProductAttributeOptions).ToListAsync();
            ViewBag.ProductId = id;
            var productVariantsViewModelList = new List<ProductVariantsViewModel>();
            var product = await _context.Products.Where(x => x.Id == id.Value).FirstOrDefaultAsync();
            var attoptions = await _context.ProductAttributeOptions.Where(x => x.ProductAttributes.ProductCategoryId == product.ProductCategoryId).ToListAsync();
            if (attoptions != null)
            {
                foreach (var variant in varients)
                {
                    var viewModel = new ProductVariantsViewModel();
                    viewModel.Id = variant.Id;
                    viewModel.Name = variant.Name;
                    int i = 1;
                    foreach (var option in variant.ProductVariantProductAttributeOptions.OrderBy(x => x.ProductAttributeOptions.Name))
                    {
                        if (i == 1)
                        {
                            var varientOption = attoptions.Where(x => x.Id == option.Id).FirstOrDefault();
                            viewModel.Option1Name = option.ProductAttributeOptions.Name;
                            viewModel.Option1Id = option.Id;
                            i++;
                        }
                        else
                        {
                            // var varientOption = attoptions.Where(x => x.Id == option.Id).FirstOrDefault();
                            viewModel.Option2Name = option.ProductAttributeOptions.Name;
                            viewModel.Option2Id = option.Id;
                        }
                    }
                    productVariantsViewModelList.Add(viewModel);

                }
            }


            return View("Index", productVariantsViewModelList);
        }
    }
}
