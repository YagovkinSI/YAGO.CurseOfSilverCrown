using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly EndOfTurnService _endOfTurnService;
        private readonly IConfiguration _configuration;

        public AdminController(ApplicationDbContext context, UserManager<User> userManager, ILogger<HomeController> logger, IConfiguration configuration, EndOfTurnService endOfTurnService)
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
            await _endOfTurnService.Execute();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CheckTurn(string id)
        {
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            await _endOfTurnService.Execute();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Update1(string id)
        {
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            var currentTurn = await _context.Turns
                .SingleAsync(t => t.IsActive);
            var commandsForDelete = _context.Commands
                .Where(c => c.TurnId != currentTurn.Id);
            _context.RemoveRange(commandsForDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }



        public async Task<IActionResult> Update2(string id)
        {
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            var organizations = await _context.Organizations.ToListAsync();
            if (organizations.Any(o => o.Power > 0))
            {
                foreach (var organization in organizations)
                {
                    var power = organization.Power;
                    organization.Warriors = power / 2000;
                    organization.Coffers = power / 100 + 5000;
                    organization.Power = 0;
                    _context.Update(organization);
                }
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Update3(string id)
        {
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            //var organizations = await _context.Organizations.ToListAsync();
            //if (organizations.Any(o => o.Power > 0))
            //{
            //    foreach (var organization in organizations)
            //    {
            //        var power = organization.Power;
            //        organization.Warriors = power / 2000;
            //        organization.Coffers = power / 100 + 5000;
            //        organization.Power = 0;
            //        _context.Update(organization);
            //    }
            //    _context.SaveChanges();
            //}

            var currentTurn = await _context.Turns
                .SingleAsync(t => t.IsActive);
            var commandsForDelete = _context.Commands
                .Where(c => c.TurnId != currentTurn.Id);
            _context.RemoveRange(commandsForDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
