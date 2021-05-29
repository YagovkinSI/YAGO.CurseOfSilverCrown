using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.EndOfTurn;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly EndOfTurnTask _endOfTurnService;
        private readonly IConfiguration _configuration;

        public AdminController(ApplicationDbContext context, UserManager<User> userManager, ILogger<HomeController> logger, IConfiguration configuration, EndOfTurnTask endOfTurnService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _endOfTurnService = endOfTurnService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NextTurn()
        {
            _endOfTurnService.Execute();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CheckTurn(string id)
        {
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            _endOfTurnService.Execute();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult CreateCommands(string id)
        {
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            _endOfTurnService.CreateCommands();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Update1(string id)
        {
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            var units = _context.Units;
            foreach (var unit in units)
            {
                if (unit.InitiatorDomainId != unit.DomainId || unit.Warriors < 10)
                    _context.Remove(unit);
                else
                {
                    unit.PositionDomainId = unit.DomainId;
                    unit.Type = Core.Database.Enums.enArmyCommandType.WarSupportDefense;
                    unit.Target2DomainId = null;
                    unit.TargetDomainId = unit.DomainId;
                    unit.Warriors = 80 + (unit.Id % 10) * 5;
                    _context.Update(unit);
                }
            }

            _context.RemoveRange(_context.Commands);
            _endOfTurnService.CreateCommands();

            var domains = _context.Domains
                .Include(d => d.Vassals)
                .Include(d => d.Suzerain);
            foreach (var domain in domains)
            {
                domain.Coffers = 3000 + ((domain.Id + 3) % 10) * 1000;
                domain.Investments += domain.Id % 10 > 5
                    ? (domain.Id % 10) * 1000
                    : 0;
                domain.Fortifications += domain.Id % 10 < 5
                    ? (domain.Id % 10) * 1000
                    : 0;
                if (domain.Vassals.Count < 7)
                {
                    domain.SuzerainId = null;
                }  
                else
                {
                    Domain suzerain = null;
                    for (var i = 7; i < domain.Vassals.Count; i++)
                    {
                        var vassal = domain.Vassals[i];
                        if (i % 7 == 0)
                            suzerain = vassal;
                        else
                            domain.SuzerainId = suzerain.Id;
                    }
                }
            }
            _context.SaveChanges();

            var domains2 = _context.Domains
                .Include(d => d.Vassals)
                .Include(d => d.Suzerain);
            foreach (var domain in domains2)
            {
                if (domain.SuzerainId == null)
                    continue;
                var routes = RouteHelper.GetAvailableRoutes(_context, domain.Suzerain, domain.SuzerainId.Value);
                if (!routes.Any(d => d.Id == domain.Id))
                    domain.SuzerainId = null;
            }

            var users = _context.Users;
            foreach (var user in users)
            {
                user.DomainId = null;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }        
    }
}
