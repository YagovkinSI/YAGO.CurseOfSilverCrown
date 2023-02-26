using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.ViewModels;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HistoryController(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.LastRoundEventStories = await EventStoryHelper.GetWorldHistoryLastRound(_context);
            ViewBag.LastEventStories = await EventStoryHelper.GetWorldHistory(_context);

            return View();
        }

        public async Task<IActionResult> FilterAsync()
        {
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            var hasDomain = currentUser?.PersonId != null;

            ViewBag.UserHasDomain = hasDomain;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HistoryAsync([Bind("Important,Region,Turns," +
            "AggressivePoliticalEvents,PeacefullPoliticalEvents,InvestmentEvents," +
            "BudgetEvents,CataclysmEvents")] HistoryFilter historyFilter)
        {
            if (historyFilter == null)
                historyFilter = new HistoryFilter();

            var currentUser =
                await _userManager.GetCurrentUser(HttpContext.User, _context);

            ViewBag.EventStories = await EventStoryHelper.GetHistory(_context, historyFilter, currentUser);

            return View();
        }
    }
}
