using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Host.Models;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Errors;
using YAGO.World.Infrastructure.Database.Models.Events;
using YAGO.World.Infrastructure.Helpers.Events;

namespace YAGO.World.Host.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AllMovingsInLastRound()
        {
            var currentRound = _context.Turns
                .Single(t => t.IsActive);

            var eventTypes = new List<EventType>
            {
                EventType.FastWarSuccess,
                EventType.FastWarFail,
                EventType.FastRebelionSuccess,
                EventType.FastRebelionFail,
                EventType.DestroyedUnit,
                EventType.SiegeFail,
                EventType.UnitMove,
                EventType.UnitCantMove,
            };

            var events = _context.Events
                .Where(e => eventTypes.Contains(e.Type))
                .Where(e => e.TurnId == currentRound.Id - 1)
                .ToList()
                .OrderByDescending(d => d.Id)
                .ToList();

            var textList = await EventHelper.GetTextStories(_context, events);

            return View(textList);
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
                _ = _context.Add(error);
                _ = _context.SaveChangesAsync();
            }
            catch
            {

            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
