using Microsoft.AspNetCore.Mvc;

namespace YAGO.World.Host.Controllers
{
    public class RatingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
