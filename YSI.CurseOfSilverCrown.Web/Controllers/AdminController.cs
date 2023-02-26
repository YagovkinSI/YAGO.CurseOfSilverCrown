using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.AI;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.Core.MainModels.Users;

namespace YSI.CurseOfSilverCrown.Web.Controllers
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
