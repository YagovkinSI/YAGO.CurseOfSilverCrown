using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class MapController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public MapController(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var array = KingdomHelper.GetDomainColors(_context);
            return View(array);
        }
    }
}
