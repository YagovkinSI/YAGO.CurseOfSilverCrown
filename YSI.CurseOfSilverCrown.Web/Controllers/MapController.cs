using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.ViewModels;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class MapController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public MapController(ApplicationDbContext context, 
            UserManager<User> userManager, 
            ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var array = new Dictionary<string, MapElement>();
            var allDomains = _context.Domains
                .Include(p => p.Person)
                .Include("Person.User")
                .Include(p => p.Suzerain)
                .Include(p => p.Vassals)
                .Include(p => p.UnitsHere)
                .ToList();
            foreach (var domain in allDomains)
            {
                var name = $"domain_{domain.Id}";
                var king = KingdomHelper.GetKingdomCapital(allDomains, domain);
                var color = KingdomHelper.GetColor(_context, allDomains, domain);
                var alpha = domain.Person.User == null && domain.SuzerainId == null
                    ? "0.0"
                    : "0.7";
                var unitIds = domain.UnitsHere
                    .Select(u => u.Id);
                var unitHere = _context.Units
                    .Include(u => u.Domain)
                    .Where(u => unitIds.Contains(u.Id))
                    .ToList();
                unitHere = unitHere
                    .Where(u => u.InitiatorPersonId == u.Domain.PersonId)
                    .ToList();
                var groups = unitHere
                    .GroupBy(u => u.DomainId);
                var unitText = new List<string>();
                var allWarriosCount = unitHere
                    .Sum(u => u.Warriors);
                unitText.Add($"Всего воинов во владении: {allWarriosCount}");
                foreach (var group in groups)
                {
                    var groupDomain = _context.Domains
                        .Include(d => d.Suzerain)
                        .Single(g => g.Id == group.Key);
                    var unitKing = KingdomHelper.GetKingdomCapital(allDomains, groupDomain);
                    var text = groupDomain.SuzerainId == null
                        ? $"- {groupDomain.Name}: воинов {group.Sum(g => g.Warriors)}"
                        : groupDomain.SuzerainId == unitKing.Id
                            ? $"- {groupDomain.Name} ({unitKing.Name}): воинов {group.Sum(g => g.Warriors)}"
                            : $"- {groupDomain.Name} ({groupDomain.Suzerain.Name}, {unitKing.Name}): воинов {group.Sum(g => g.Warriors)}";
                    unitText.Add(text);
                }
                var titleText = domain.SuzerainId == null
                        ? $"{domain.Name} ({king.Name})"
                        : domain.SuzerainId == king.Id
                            ? $"{domain.Name} ({king.Name})"
                            : $"{domain.Name} ({domain.Suzerain.Name}, {king.Name})";
                var fortification = FortificationsHelper.GetDefencePercent(domain.Fortifications);
                titleText += $"\r\n[Укрепления - {fortification}%]";
                array.Add(name, new MapElement(titleText, color, alpha, unitText, domain.MoveOrder));
            }

            array.Add("unknown_earth", new MapElement("Недоступные земли", Color.Black, "0.85", new List<string>()));
            return View(array);
        }
    }
}
