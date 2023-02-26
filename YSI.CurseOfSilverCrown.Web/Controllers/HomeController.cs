using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Errors;
using YSI.CurseOfSilverCrown.Core.MainModels.Users;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;
using YSI.CurseOfSilverCrown.Web.Models;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Cards = new List<Card>()
            {
                await GetWelcomeCardAsync(),
                GetMapCard(),
                GetHistoryCard(),
                //GetRatingCard(),
            };

            return View();
        }

        private async Task<Card> GetWelcomeCardAsync()
        {
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            return currentUser == null
                ? GetWelcomeCardForGuest()
                : await GetWelcomeCardForUserAsync(currentUser);
        }

        private Card GetWelcomeCardForGuest()
        {
            return new Card
            {
                Title = "Добро пожаловать в игру Проклятие Серебряной Короны!",
                Text = "Возьми под управление один из регионов средневекового мира. Развивай свои земли, " +
                        "воюй или договаривайся с соседями, заполучи вассалов и заслужи титул короля.",
                Links = new List<ILink>
                    {
                        new AspPage("Identity", "/Account/Register", "Регистрация"),
                        new AspPage("Identity", "/Account/Login", "Вход"),
                    }
            };
        }

        private async Task<Card> GetWelcomeCardForUserAsync(User currentUser)
        {
            var turn = await _context.Turns
                   .SingleAsync(t => t.IsActive);
            var currentDate = DateTime.UtcNow;
            var time = currentDate.Hour > 2
                ? currentDate.Date + new TimeSpan(1, 2, 0, 0)
                : currentDate.Date + new TimeSpan(2, 0, 0);

            return new Card
            {
                Title = $"Здравствуйте, {currentUser.UserName}! " +
                    $"На дворе {GameSessionHelper.GetName(_context, turn)}",
                Text = time.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
                SpecialOperation = 1,
                Links = new List<ILink>
                    {
                        new AspAction("Domain", "Index", "К владению"),
                        new UrlLink("https://vk.com/club189975977", "Группа в ВК"),
                    }
            };
        }

        private Card GetMapCard()
        {
            return new Card
            {
                Image = Url.Content("~/assets/images/cardMap.jpg"),
                Title = "Проработанная карта мира с множеством индивидуальных игровых регионов.",
                Text = "На текущий момент в игре более сотни регионов. Сейчас они мало чем отличаются друг " +
                "от друга, но со временем каждый регион будет иметь индивидуальные черты.",
                Links = new List<ILink>
                {
                    new AspAction("Map", "Index", "Карта"),
                }
            };
        }

        private Card GetHistoryCard()
        {
            return new Card
            {
                Image = Url.Content("~/assets/images/cardHistory.jpg"),
                Title = "История мира на основе действий игроков.",
                Text = "Возвышения королевств, мятежи вассалов, войны, постройки замков. " +
                "Игровые события сохраняются в истории мира и Вы можете внести свою главу в " +
                "развитии мира.",
                Links = new List<ILink>
                {
                    new AspAction("History", "Index", "История"),
                }
            };
        }

        private Card GetRatingCard()
        {
            return new Card
            {
                Image = Url.Content("~/assets/images/cardRating.jpg"),
                Title = "Соревнуйся с другими игроками в различных рейтингах.",
                Text = "Самый большая казна, самый развитый регион, самая большоая армия," +
                " самый неприступный замок и другие рейтинги.",
                Links = new List<ILink>
                {
                    new AspAction("Rating", "Index", "Рейтинг"),
                }
            };
        }

        private Card GetRulesCard()
        {
            return new Card
            {
                Image = Url.Content("~/assets/images/cardRules.jpg"),
                Title = "Узнай подробности об игре и правилах.",
                Text = "Как проходит игра? Как долго строится замок? Когда мои войска дойдут до цели? " +
                "Как помочь соседу в войне? ОТветы на эти вопросы можно получить в разделе Правила.\r\n" +
                "А Если вы хотите узнать больше и пообщаться с другими игроками, то добро пожаловать в группу " +
                "игры в vk.com",
                Links = new List<ILink>
                {
                    new AspAction("Rules", "Index", "Правила"),
                    new UrlLink("https://vk.com/club189975977", "Группа в ВК"),
                }
            };
        }

        public async Task<IActionResult> AllMovingsInLastRound()
        {
            var currentRound = _context.Turns
                .Single(t => t.IsActive);

            var eventTypes = new List<string>
            {
                "\"EventResultType\":2001",
                "\"EventResultType\":2002",
                "\"EventResultType\":2003",
                "\"EventResultType\":2004",
                "\"EventResultType\":2005",
                "\"EventResultType\":2006",
                "\"EventResultType\":104001",
                "\"EventResultType\":104002"
            };

            var events = _context.EventStories
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
