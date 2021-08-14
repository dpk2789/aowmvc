using Microsoft.AspNetCore.Mvc;

namespace AowCore.AppWeb.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult CheckOut()
        {
            return View();
        }

        public IActionResult PaymentSuccessfull()
        {
            return View();
        }
    }
}
