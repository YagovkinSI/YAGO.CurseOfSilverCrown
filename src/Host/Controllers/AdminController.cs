using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Host.Database.Users;
using YAGO.World.Host.Helpers;
using YAGO.World.Host.Helpers.Commands;
using YAGO.World.Host.AI;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly TurnRunNextTask _endOfTurnService;
        private readonly IConfiguration _configuration;

        public AdminController(ApplicationDbContext context, UserManager<User> userManager, ILogger<HomeController> logger, IConfiguration configuration, TurnRunNextTask endOfTurnService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _endOfTurnService = endOfTurnService;
        }

        public async Task<IActionResult> CheckTurn(string id)
        {
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            var domains = _context.Domains.ToList();
            foreach (var domain in domains)
            {
                CommandHelper.CheckAndFix(_context, domain.Id);
            }

            AIHelper.AICommandsPrepare(_context);

            _endOfTurnService.Execute();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Update1(string id)
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Wipe(string id)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
