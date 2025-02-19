using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YAGO.World.Host.Database.Users;
using YAGO.World.Host.Helpers;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Controllers
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
