using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.EndOfTurn;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly EndOfTurnTask _endOfTurnService;
        private readonly IConfiguration _configuration;

        public AdminController(ApplicationDbContext context, UserManager<User> userManager, ILogger<HomeController> logger, IConfiguration configuration, EndOfTurnTask endOfTurnService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _endOfTurnService = endOfTurnService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NextTurn()
        {
            _endOfTurnService.Execute();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CheckTurn(string id)
        {
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            _endOfTurnService.Execute();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult CreateCommands(string id)
        {
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            _endOfTurnService.CreateCommands();
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
