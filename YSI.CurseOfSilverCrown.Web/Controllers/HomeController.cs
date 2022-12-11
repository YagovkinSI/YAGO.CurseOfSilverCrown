using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.ViewModels;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;
using YSI.CurseOfSilverCrown.Web.Models;

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
            ViewBag.Turn = GameSessionHelper.GetName(_context, turn);

            ViewBag.LastRoundEventStories = await EventStoryHelper.GetWorldHistoryLastRound(_context);
            ViewBag.LastEventStories = await EventStoryHelper.GetWorldHistory(_context);

            return View();
        }

        public async Task<IActionResult> AllMovingsInLastRound()
        {
            var currentRound = _context.Turns
                .Single(t => t.IsActive);

            var eventTypes = new List<string>
            {
                "\"EventResultType\":2001",
                "\"EventResultType\":2002",
                "\"EventResultType\":2005",
                "\"EventResultType\":104001",
                "\"EventResultType\":104002"
            };

            var events = _context.EventStories
                .Include(e => e.Turn)
                .Where(e => e.TurnId == currentRound.Id - 1)
                .ToList()
                .Where(e => eventTypes.Any(p => e.EventStoryJson.Contains(p)))
                .OrderByDescending(d => d.Id)
                .ToList();

            var textList = await EventStoryHelper.GetTextStories(_context, events);

            return View(textList);
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
