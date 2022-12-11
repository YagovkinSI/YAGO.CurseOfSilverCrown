using Microsoft.AspNetCore.Mvc;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class HistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
