using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Web.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using System.Drawing;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, UserManager<User> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var turn = await _context.Turns
                .SingleAsync(t => t.IsActive);
            ViewBag.Turn = turn.Name;

            ViewBag.LastRoundEventStories = await EventStoryHelper.GetWorldHistoryLastRound(_context);
            ViewBag.LastEventStories = await EventStoryHelper.GetWorldHistory(_context);

            return View();
        }

        public IActionResult Map()
        {
            var array = new Dictionary<string, string>();
            var allOrganizations = _context.Organizations
                .Where(o => o.OrganizationType == Core.Database.Enums.enOrganizationType.Lord)
                .Include(p => p.Suzerain)
                .ToList();
            foreach (var organization in allOrganizations)
            {
                var name = $"province_{organization.ProvinceId}";
                var king = KingdomHelper.GetKingdomCapital(allOrganizations, organization);
                var color = Color.FromArgb(
                    (king.Id[0] % 10) * 25, 
                    (king.Id[king.Id.Length/2] % 10) * 25, 
                    (king.Id[king.Id.Length-1] % 10) * 25);
                array.Add(name, $"rgba({color.R}, {color.G}, {color.B}, 0.7)");
            }

            return View(array);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
