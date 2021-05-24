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
using Microsoft.AspNetCore.Diagnostics;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;
using YSI.CurseOfSilverCrown.Core.ViewModels;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

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


        public IActionResult WarrioirsOnMap()
        {
            var domains = _context.Domains
                .Include(d => d.UnitsHere)
                .Include("UnitsHere.Domain")
                .OrderBy(d => d.Name);

            return View(domains);
        }

        public IActionResult Map()
        {
            var array = new Dictionary<string, MapElement>();
            var allDomains = _context.Domains
                .Include(p => p.User)
                .Include(p => p.Suzerain)
                .Include(p => p.UnitsHere)
                .ToList();
            var count = allDomains.Count;
            var colorParts = (int)Math.Ceiling(Math.Pow(count, 1/3.0));
            var colorStep = 255 / (colorParts - 1);
            var colorCount = (int)Math.Pow(colorParts, 3);
            var sqrt = (int)Math.Floor(Math.Sqrt(colorCount));
            foreach (var domain in allDomains)
            {
                var name = $"domain_{domain.Id}";
                var king = KingdomHelper.GetKingdomCapital(allDomains, domain);
                var colorNum = (king.Id % sqrt * (colorCount / sqrt)) + (king.Id / sqrt);

                var colorR = colorNum % colorParts * colorStep;
                var colorG = (colorNum / colorParts) % colorParts * colorStep;
                var colorB = (colorNum / colorParts / colorParts) % colorParts * colorStep;
                var alpha = domain.User == null && domain.SuzerainId == null
                    ? "0.0"
                    : "0.7";
                var color = Color.FromArgb(colorR, colorG, colorB);
                var groups = domain.UnitsHere
                    .Where(u => u.Status == enCommandStatus.ReadyToRun)
                    .GroupBy(u => u.DomainId);
                var unitText = new List<string>();
                foreach (var group in groups)
                {
                    var groupDomain = _context.Domains
                        .Include(d => d.Suzerain)
                        .Single(g => g.Id == group.Key);
                    var unitKing = KingdomHelper.GetKingdomCapital(allDomains, groupDomain);
                    var text = groupDomain.SuzerainId == null
                        ? $"{groupDomain.Name}: воинов {group.Sum(g => g.Warriors)}"
                        : groupDomain.SuzerainId == unitKing.Id
                            ? $"{groupDomain.Name} ({unitKing.Name}): воинов {group.Sum(g => g.Warriors)}"
                            : $"{groupDomain.Name} ({groupDomain.Suzerain.Name}, {unitKing.Name}): воинов {group.Sum(g => g.Warriors)}";
                    unitText.Add(text);
                }
                var titleText = domain.SuzerainId == null
                        ? $"{domain.Name} ({king.Name})"
                        : domain.SuzerainId == king.Id
                            ? $"{domain.Name} ({king.Name})"
                            : $"{domain.Name} ({domain.Suzerain.Name}, {king.Name})";
                array.Add(name, new MapElement(titleText, color, alpha, unitText));
            }

            array.Add("unknown_earth", new MapElement("Недоступные земли", Color.Black, "0.85", new List<string>()));
            return View(array);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerFeature>();
                var error = new Error
                {
                    Message = exceptionHandler?.Error?.Message,
                    TypeFullName = exceptionHandler?.Error?.GetType()?.FullName,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    StackTrace = exceptionHandler?.Error?.StackTrace
                };
                _context.Add(error);
                _context.SaveChangesAsync();
            }
            catch
            {
                
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
