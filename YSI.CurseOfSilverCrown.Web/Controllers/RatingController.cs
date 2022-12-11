using Microsoft.AspNetCore.Mvc;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class RatingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
