using Microsoft.AspNetCore.Mvc;

namespace YAGO.World.Host.Controllers
{
    public class RulesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
