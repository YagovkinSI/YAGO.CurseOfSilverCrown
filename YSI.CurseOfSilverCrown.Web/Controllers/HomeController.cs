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
using YSI.CurseOfSilverCrown.Core.APIModels.BudgetModels;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.Errors;
using YSI.CurseOfSilverCrown.Core.Database.Users;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Helpers.Commands;
using YSI.CurseOfSilverCrown.Core.Helpers.Events;
using YSI.CurseOfSilverCrown.Core.Parameters;
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
            return GetPromptCard(currentUser);
        }

        private Card GetPromptCard(User currentUser)
        {
            if (currentUser == null)
                return GetPromptForGuest();
            if (currentUser.CharacterId == null)
                return GetPromptForChooseDomain(currentUser);

            var domain = currentUser.Character.Domains.Single();
            return GetPromptDefault(currentUser, domain).Result;
        }

        private Card GetPromptForGuest()
        {
            return new Card
            {
                Title = "Добро пожаловать в игру Проклятие Серебряной Короны!",
                Text = "Возьмите под управление один из регионов средневекового мира. " +
                "Развивайте свои земли, воюйте или договаривайтесь с соседями, заполучите вассалов и заслужите титул короля. " +
                "Войдите в свой аккаунт или пройдите регистрацию, чтобы присоединиться к игре.",
                Links = new List<ILink>
                    {
                        new AspPage("Identity", "/Account/Login", "Вход"),
                        new AspPage("Identity", "/Account/Register", "Регистрация")
                    }
            };
        }

        private Card GetPromptForChooseDomain(User currentUser)
        {
            return new Card
            {
                Title = $"Здравствуйте, {currentUser.UserName}!",
                Text = "Выберите владение под своё управление из списка.",
                Links = new List<ILink>
                    {
                        new AspAction("Domain", "Index", "Список владений"),
                    }
            };
        }

        private async Task<Card> GetPromptDefault(User currentUser, Domain domain)
        {
            var turn = await _context.Turns
                   .SingleAsync(t => t.IsActive);
            var currentDate = DateTime.UtcNow;
            var time = currentDate.Hour > 2
                ? currentDate.Date + new TimeSpan(1, 2, 0, 0)
                : currentDate.Date + new TimeSpan(2, 0, 0);
            var (text, link) = GetPrompt(currentUser, domain);

            return new Card
            {
                Title = $"Здравствуйте, {currentUser.UserName}! " +
                    $"На дворе {turn.GetName()}",
                Time = time.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
                SpecialOperation = 1,
                Text = text,
                Links = new List<ILink>
                {
                    link,
                    new AspAction("Domain", "Index", "Управление владением"),
                    new UrlLink("https://vk.com/club189975977", "Группа в ВК", true),
                }
            };
        }

        private (string text, AspAction link) GetPrompt(User currentUser, Domain domain)
        {
            CommandHelper.CheckAndFix(_context, domain.Id, domain.OwnerId);

            var budget = new Budget(_context, domain, domain.OwnerId);
            var totalExpected = budget.Lines.Single(l => l.Type == BudgetLineType.Total).Coffers.ExpectedValue.Value;
            var isDeficit = totalExpected - domain.Gold < 0;

            if (!isDeficit && domain.Gold - domain.Commands.Sum(c => c.Gold) > CoffersParameters.StartCount / 5)
                return ($"В казне владения имеется {domain.Gold} золотых монет. Вложите их грамотно.",
                    new AspAction("Commands", "Index", "Экономические и политические приказы"));
            if (domain.Relations.Count == 0)
                return ($"Выставите в отношениях какие владения вы готовы защищать. " +
                    $"Вы всегда защищаете своё владение и владения прямых вассалов.",
                    new AspAction("DomainRelations", "Index", "Управление отношениями"));
            if (domain.Units.All(u => u.Type == Core.Database.Units.UnitCommandType.WarSupportDefense))
                return ($"Все отряды имеют приказы защиты. Возможно часть войск стоит отправить в атаку?",
                        new AspAction("Units", "Index", "Управление военными отрядами"));
            return ($"Кажется все приказы отданы. Или нет?", null);

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
                    new UrlLink("https://vk.com/club189975977", "Группа в ВК", true),
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

            var events = _context.Events
                .Where(e => e.TurnId == currentRound.Id - 1)
                .ToList()
                .Where(e => eventTypes.Any(p => e.EventJson.Contains(p)))
                .OrderByDescending(d => d.Id)
                .ToList();

            var textList = await EventHelper.GetTextStories(_context, events);

            return View(textList);
        }

        public IActionResult Privacy() => View();

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
