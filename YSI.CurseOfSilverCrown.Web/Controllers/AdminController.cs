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

            var organizations = _context.Organizations
                .Include(o => o.Commands);
            foreach (var organization in organizations)
            {
                var tax = organization.Commands
                    .Single(c => c.Type == Enums.enCommandType.CollectTax);
                var hasDefense = organization.Commands
                    .Any(c => c.Type == Enums.enCommandType.WarSupportDefense);

                if (tax.Warriors > 30 && !hasDefense)
                {
                    var command = new Command
                    {
                        Id = Guid.NewGuid().ToString(),
                        Warriors = tax.Warriors - 30,
                        OrganizationId = organization.Id,
                        Type = Enums.enCommandType.WarSupportDefense
                    };
                    _context.Add(command);

                    tax.Warriors = 30;
                    _context.Update(tax);
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
