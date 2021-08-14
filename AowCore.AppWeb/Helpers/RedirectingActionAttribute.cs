using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace AowCore.AppWeb.Helpers
{
    public class RedirectingActionAttribute : ActionFilterAttribute
    {       
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            
            var cmpid = filterContext.HttpContext.Request.Cookies["cmpCookee"];
            var fid = filterContext.HttpContext.Request.Cookies["fYrCookee"];

            if (string.IsNullOrEmpty(cmpid) && string.IsNullOrEmpty(fid))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Index"
                }));
            }
        }
    }
}
