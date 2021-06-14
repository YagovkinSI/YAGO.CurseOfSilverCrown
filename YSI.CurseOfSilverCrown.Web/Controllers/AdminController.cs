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
using YSI.CurseOfSilverCrown.Core.Parameters;

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

            var units = _context.Units.ToList();
            foreach (var unit in units)
            {
                unit.InitiatorPersonId = unit.InitiatorDomainId;
            }
            _context.UpdateRange(units);


            var commands = _context.Commands.ToList();
            foreach (var unit in commands)
            {
                unit.InitiatorPersonId = unit.InitiatorDomainId;
            }
            _context.UpdateRange(units);
            _context.SaveChanges();


            return RedirectToAction("Index", "Home");
        }

        public IActionResult Wipe(string id)
        {

            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            //var currentTurn = _context.Turns.Single(t => t.IsActive == true);
            //var sessionEnd = _context.GameSessions.Single(t => t.EndSeesionTurnId >= currentTurn.Id);
            //sessionEnd.EndSeesionTurnId = currentTurn.Id - 1;
            //_context.Update(sessionEnd);

            //var newSession = new GameSession
            //{
            //    EndSeesionTurnId = int.MaxValue,
            //    StartSeesionTurnId = currentTurn.Id,
            //    NumberOfGame = sessionEnd.NumberOfGame + 1
            //};
            //_context.Add(newSession);


            //_context.RemoveRange(_context.Commands);
            //_endOfTurnService.CreateCommands();

            //_context.RemoveRange(_context.Units);
            //var domains = _context.Domains
            //   .Include(d => d.Vassals)
            //   .Include(d => d.Suzerain)
            //   .ToList();
            //foreach (var domain in domains)
            //{
            //    var unit = new Unit
            //    {
            //        ActionPoints = 100,
            //        Coffers = 0,
            //        DomainId = domain.Id,
            //        InitiatorDomainId = domain.Id,
            //        PositionDomainId = domain.Id,
            //        Status = Core.Database.Enums.enCommandStatus.ReadyToMove,
            //        TargetDomainId = domain.Id,
            //        Type = Core.Database.Enums.enArmyCommandType.WarSupportDefense,
            //        Warriors = 80 + (domain.Id % 10) * 5
            //    };
            //    _context.Add(unit);
            //}
            //_context.SaveChanges();

            ////убираем некорретных васслов
            //var haveChanges = true;
            //var circle = 1;
            //while (haveChanges)
            //{
            //    domains = _context.Domains
            //       .Include(d => d.Vassals)
            //       .Include(d => d.Suzerain)
            //       .ToList();

            //    foreach (var domain in domains)
            //    {
            //        if (domain.Vassals.Count == 0)
            //            continue;

            //        //вассалы которые не могут дойти до сюзерена или далеко от него образуют своё королевство
            //        if (circle == 1)
            //        {
            //            var farVassals = new List<Domain>();
            //            foreach (var vassal in domain.Vassals)
            //            {
            //                var routes = RouteHelper.GetAvailableRoutes(_context, vassal.Id, 5);
            //                if (!routes.Any(r => r.Id == domain.Id))
            //                    farVassals.Add(vassal);
            //            }
            //            if (farVassals.Count > 0)
            //            {
            //                haveChanges = true;
            //                var suzerain = farVassals[0];
            //                foreach (var vassal in farVassals)
            //                {
            //                    var domainVassal = domains.Single(d => d.Id == vassal.Id);
            //                    var isSuzerain = domainVassal.Id == suzerain.Id;
            //                    vassal.Suzerain = isSuzerain ? null : suzerain;
            //                    vassal.SuzerainId = isSuzerain ? null : suzerain.Id;
            //                    _context.Update(vassal);
            //                    _context.SaveChanges();
            //                }
            //            }
            //        }

            //        //слабые сюзерены теряют васслов
            //        var count = (int)(3.5 - circle / 10.0);
            //        if (domain.Vassals.Count < count)
            //        {
            //            haveChanges = true;
            //            for (int i = 0; i < domain.Vassals.Count; i++)
            //            {
            //                Domain vassal = domain.Vassals[i];
            //                var domainVassal = domains.Single(d => d.Id == vassal.Id);
            //                domainVassal.SuzerainId = domain.SuzerainId;
            //                domainVassal.Suzerain = domain.Suzerain;
            //                _context.Update(domainVassal);
            //                _context.SaveChanges();
            //            }
            //            continue;
            //        }

            //        //крутые вассалы становятся независимыми
            //        var count2 = (int)(9.5 - circle / 10.0);
            //        if (domain.Suzerain != null && domain.Vassals.Count > count2)
            //        {
            //            haveChanges = true;
            //            domain.SuzerainId = null;
            //            domain.Suzerain = null;
            //            _context.Update(domain);
            //            _context.SaveChanges();
            //        }

            //        //если много вассалов, образуются гранд-вассалы
            //        var count3 = (int)(5.0 + circle / 10.0);
            //        if (domain.Vassals.Count > count3)
            //        {
            //            haveChanges = true;
            //            var random = new Random().Next(domain.Vassals.Count);
            //            var grandLord = domain.Vassals[random];
            //            var neigbours = RouteHelper.GetNeighbors(_context, grandLord.Id);
            //            foreach (var neibour in neigbours)
            //            {
            //                if (!KingdomHelper.IsSameKingdoms(_context.Domains, neibour, grandLord))
            //                    continue;
            //                var vassal = domains.Single(d => d.Id == neibour.Id);
            //                if (vassal.Vassals.Count > 0 || vassal.SuzerainId == grandLord.Id)
            //                    continue;
            //                vassal.Suzerain = grandLord;
            //                vassal.SuzerainId = grandLord.Id;
            //                _context.Update(vassal);
            //                _context.SaveChanges();
            //            }
            //        }                    
            //    }
            //    circle++;
            //}            

            //исправляем характеристики
            var domains3 = _context.Domains
                .Include(d => d.Vassals)
                .Include(d => d.Suzerain);
            foreach (var domain in domains3)
            {
                domain.Coffers = 3000 + ((domain.Id + 3) % 10) * 200;
                //domain.Investments = (int)(domain.Investments * 0.7);
                //domain.Fortifications -= (int)((domain.Fortifications - 5000) * 0.3);
                domain.TurnOfDefeat = int.MinValue;
            }
            _context.SaveChanges();

            var users = _context.Users;
            foreach (var user in users)
            {
                user.PersonId = null;
            }
            _context.SaveChanges();

            _context.RemoveRange(_context.OrganizationEventStories);
            _context.RemoveRange(_context.EventStories);
            //_context.RemoveRange(_context.Turns);
            _context.SaveChanges();

            //var firstTurn = new Turn
            //{
            //    IsActive = true,
            //    Started = DateTime.UtcNow
            //};
            //_context.Add(firstTurn);
            //_context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }        
    }
}
