using AowCore.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AowCore.AppWeb.PartialViews
{
    public class ProductAttributeListViewComponent : ViewComponent
    {
        private readonly IApplicationDbContext _context;

        public ProductAttributeListViewComponent(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(Guid? id)
        {
            var attributes = await _context.ProductAttributes.Where(x => x.ProductCategoryId == id.Value).ToListAsync();
            ViewBag.CategoryId = id;
            return View("Index", attributes);
        }
    }
}
