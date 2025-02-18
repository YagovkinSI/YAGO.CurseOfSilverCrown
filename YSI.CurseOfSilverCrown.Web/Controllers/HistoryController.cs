using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Database;
using YSI.CurseOfSilverCrown.Web.Database.Users;
using YSI.CurseOfSilverCrown.Web.Helpers;
using YSI.CurseOfSilverCrown.Web.APIModels;
using YSI.CurseOfSilverCrown.Web.Helpers.Events;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

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
            ViewBag.LastEventStories = await EventHelper.GetTopHistory(_context);

            return View();
        }

        public async Task<IActionResult> FilterAsync(int? defaultDomainId)
        {
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            var hasDomain = currentUser?.Domains.Any();

            ViewBag.UserHasDomain = hasDomain;

            var domains = _context.Domains.ToList();
            var domainDict = domains
                .ToDictionary(d => d.Name, d => (int?)d.Id)
                .Prepend( new KeyValuePair<string, int?> ("Все владения", null));
            ViewData["Domain"] = new SelectList(domainDict, "Value", "Key", defaultDomainId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HistoryAsync([Bind("DomainId,Important,Region,Turns," +
            "AggressivePoliticalEvents,PeacefullPoliticalEvents,InvestmentEvents," +
            "BudgetEvents,CataclysmEvents")] HistoryFilter historyFilter)
        {
            if (historyFilter == null)
                historyFilter = new HistoryFilter();

            var currentUser =
                await _userManager.GetCurrentUser(HttpContext.User, _context);

            ViewBag.EventStories = await EventHelper.GetHistory(_context, historyFilter, currentUser);

            return View();
        }
    }
}
