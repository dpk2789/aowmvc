using Microsoft.AspNetCore.Mvc;


namespace API.Controllers.V1
{
    public class TestController : Controller
    {
     
        public IActionResult Index()
        {
            return Ok(new { name = "hello" });
        }
    }
}
