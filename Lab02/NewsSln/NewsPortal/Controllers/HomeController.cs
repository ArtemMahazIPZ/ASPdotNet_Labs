using Microsoft.AspNetCore.Mvc;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
