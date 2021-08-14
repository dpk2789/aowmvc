using Microsoft.AspNetCore.Mvc;

namespace AowCore.AppWeb.Controllers
{
    public class MyAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
