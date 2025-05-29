using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUser;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Users;
using YAGO.World.Host.Models;
using YAGO.World.Infrastructure.APIModels.AspActions;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Errors;
using YAGO.World.Infrastructure.Database.Models.Events;
using YAGO.World.Infrastructure.Helpers;
using YAGO.World.Infrastructure.Helpers.Events;
using YAGO.World.Infrastructure.Promt;

namespace YAGO.World.Host.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepositoryOrganizations _repositoryOrganizations;
        private readonly IRepositoryTurns _repositoryTurns;
        private readonly ICurrentUserService _currentUserService;
        private readonly PromtCreator _promtCreator;

        public HomeController(ApplicationDbContext context,
            IRepositoryOrganizations repositoryOrganizations,
            IRepositoryTurns repositoryTurns,
            ICurrentUserService currentUserService,
            PromtCreator promtCreator)
        {
            _context = context;
            _repositoryOrganizations = repositoryOrganizations;
            _repositoryTurns = repositoryTurns;
            _currentUserService = currentUserService;
            _promtCreator = promtCreator;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _currentUserService.Get(User);
            var isAdmin = await _currentUserService.IsAdmin(currentUser?.Id);

            var cards = new List<Card>()
            {
                await GetWelcomeCardAsync(),
                GetMapCard(),
            };

            var pageModel = new HomePageModel(cards, isAdmin);

            return View(pageModel);
        }

        private async Task<Card> GetWelcomeCardAsync()
        {
            var currentUser = await _currentUserService.Get(User);
            return await GetPromptCard(currentUser);
        }

        private async Task<Card> GetPromptCard(User currentUser)
        {
            if (currentUser == null)
                return GetPromptForGuest();

            var organization = await _repositoryOrganizations.GetOrganizationByUser(currentUser.Id);

            return organization == null
                ? GetPromptForChooseDomain(currentUser)
                : await GetPromptDefault(currentUser.UserName, organization.Id);
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

        private async Task<Card> GetPromptDefault(string userName, int organizationId)
        {
            var currentTurnId = await _repositoryTurns.GetCurrentTurnId();
            var currentDate = DateTime.UtcNow;
            var time = currentDate.Hour > 2
                ? currentDate.Date + new TimeSpan(1, 2, 0, 0)
                : currentDate.Date + new TimeSpan(2, 0, 0);
            var (text, link) = await GetPrompt(organizationId);

            return new Card
            {
                Title = $"Здравствуйте, {userName}! " +
                    $"На дворе {TurnHelper.GetName(currentTurnId)}",
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

        private async Task<(string text, AspAction link)> GetPrompt(int organizationId) => await _promtCreator.Create(organizationId);

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
