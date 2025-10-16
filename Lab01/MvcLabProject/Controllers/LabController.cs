using Microsoft.AspNetCore.Mvc;

namespace MvcLabProject.Controllers
{
    public class LabController : Controller
    {
        public IActionResult Info()
        {
            ViewData["LabNumber"] = "Лабораторна робота №1";
            ViewData["Topic"] = "Вступ до ASP.NET Core";
            ViewData["Goal"] = "ознайомитися з основними принципами роботи .NET типів";
            ViewData["Author"] = "Artem Mahaz";

            return View();
        }
    }
}
