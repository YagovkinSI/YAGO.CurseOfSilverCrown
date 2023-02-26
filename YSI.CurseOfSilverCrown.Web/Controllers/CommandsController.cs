using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.GameCommands;
using YSI.CurseOfSilverCrown.Core.MainModels.GameCommands.DomainCommands;
using YSI.CurseOfSilverCrown.Core.ViewModels;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class CommandsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public CommandsController(ApplicationDbContext context, UserManager<User> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Commands
        [Authorize]
        public async Task<IActionResult> Index(int? organizationId)
        {
            var currentUser =
                await UserHelper.AccessСheckAndGetCurrentUser(_context, _userManager, HttpContext.User, organizationId);
            if (currentUser == null)
                return RedirectToAction("Index", "Organizations");

            GameErrorHelper.CheckAndFix(_context, organizationId.Value, currentUser.PersonId.Value);

            var commands = await _context.Commands
                .Where(c => c.DomainId == organizationId &&
                    c.InitiatorPersonId == currentUser.PersonId)
                .ToListAsync();

            var domain = await _context.Domains.FindAsync(organizationId.Value);
            ViewBag.Budget = new Budget(_context, domain, currentUser.PersonId.Value);

            var userDomain = await _context.Domains.SingleAsync(d => d.PersonId == currentUser.PersonId.Value);
            ViewBag.InitiatorId = userDomain.Id;

            var currentTurn = _context.Turns.Single(t => t.IsActive);
            ViewBag.TurnCountBeforeRebelion = domain.TurnOfDefeat + RebelionHelper.TurnCountWithoutRebelion < currentTurn.Id
                ? 0
                : domain.TurnOfDefeat + RebelionHelper.TurnCountWithoutRebelion - currentTurn.Id + 1;

            return View(commands);
        }

        // GET: Commands/Create
        public async Task<IActionResult> CreateAsync(int type, int? organizationId)
        {
            var currentUser =
                await UserHelper.AccessСheckAndGetCurrentUser(_context, _userManager, HttpContext.User, organizationId);
            if (currentUser == null)
                return RedirectToAction("Index", "Organizations");

            var domain = await _context.Domains.FindAsync(organizationId.Value);
            ViewBag.Organization = domain;

            var userDomain = await _context.Domains.SingleAsync(d => d.PersonId == currentUser.PersonId.Value);
            ViewBag.Resourses = await FillResources(organizationId.Value, userDomain.PersonId);

            switch ((enDomainCommandType)type)
            {
                case enDomainCommandType.VassalTransfer:
                    return await VassalTransferAsync(null, userDomain.PersonId, organizationId.Value);
                case enDomainCommandType.GoldTransfer:
                    return await GoldTransferAsync(null, userDomain.Id, organizationId.Value);
                case enDomainCommandType.Rebellion:
                    return Rebellion(null, userDomain.Id, organizationId.Value);
                default:
                    return NotFound();
            }
        }

        // POST: Commands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("TypeInt,TargetDomainId,Target2DomainId," +
            "Coffers,Warriors,DomainId")] Command command)
        {
            var currentUser =
                await UserHelper.AccessСheckAndGetCurrentUser(_context, _userManager, HttpContext.User, command.DomainId);
            if (currentUser == null)
                return RedirectToAction("Index", "Organizations");

            command.Type = (enDomainCommandType)command.TypeInt;
            if (new[] { enDomainCommandType.VassalTransfer, enDomainCommandType.GoldTransfer }.Contains(command.Type) &&
                command.TargetDomainId == null)
            {
                return RedirectToAction("Index", "Commands");
            }

            if (new[] { enDomainCommandType.VassalTransfer }.Contains(command.Type) &&
                command.Target2DomainId == null)
            {
                return RedirectToAction("Index", "Commands");
            }

            command.InitiatorPersonId = currentUser.PersonId.Value;
            command.Status = enCommandStatus.ReadyToMove;

            if (ModelState.IsValid)
            {
                _context.Add(command);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { organizationId = command.DomainId });
            }

            return RedirectToAction(nameof(Index), new { organizationId = command.DomainId });
        }

        // GET: Commands/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ValidCommand(id.Value, out var command, out var userDomain))
                return NotFound();

            ViewBag.Organization = await _context.Domains
                .FindAsync(command.DomainId);

            ViewBag.Resourses = await FillResources(command.DomainId, userDomain.PersonId, command.Id);

            switch (command.Type)
            {
                case enDomainCommandType.Growth:
                    return Growth(command);
                case enDomainCommandType.Investments:
                    return Investments(command);
                case enDomainCommandType.Fortifications:
                    return Fortifications(command);
                case enDomainCommandType.VassalTransfer:
                    return await VassalTransferAsync(command, userDomain.PersonId, command.DomainId);
                case enDomainCommandType.GoldTransfer:
                    return await GoldTransferAsync(command, userDomain.Id, command.DomainId);
                case enDomainCommandType.Rebellion:
                    return Rebellion(command, userDomain.Id, command.DomainId);
                default:
                    return NotFound();
            }
        }

        private IActionResult Growth(Command command)
        {
            if (command == null || command.Type != enDomainCommandType.Growth)
            {
                return NotFound();
            }

            var editCommand = new GrowthCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> GoldTransferAsync(Command command, int userOrganizationId, int organizationId)
        {
            if (command != null && command.Type != enDomainCommandType.GoldTransfer)
                return NotFound();

            ViewBag.IsOwnCommand = userOrganizationId == organizationId;

            var targetOrganizations = await GoldTransferHelper.GetAvailableTargets(_context, organizationId, command);

            ViewBag.TargetOrganizations = targetOrganizations;
            var defaultTargetId = command != null
                ? command.TargetDomainId
                : targetOrganizations.FirstOrDefault()?.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations.OrderBy(o => o.Name), "Id", "Name", defaultTargetId);

            var editCommand = new GoldTransferCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private IActionResult Rebellion(Command command, int userOrganizationId, int organizationId)
        {
            if (command != null && command.Type != enDomainCommandType.Rebellion)
            {
                return NotFound();
            }

            ViewBag.IsOwnCommand = userOrganizationId == organizationId;

            var editCommand = new RebelionCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> VassalTransferAsync(Command command, int initiatorId, int organizationId)
        {
            if (command != null && command.Type != enDomainCommandType.VassalTransfer)
            {
                return NotFound();
            }

            ViewBag.IsOwnCommand = initiatorId == organizationId;

            var organization = await _context.Domains.FindAsync(organizationId);

            var targetOrganizations = await VassalTransferHelper.GetAvailableTargets(_context, organizationId, initiatorId, command);
            var target2Organizations = await VassalTransferHelper.GetAvailableTargets2(_context, organizationId, command);

            ViewBag.TargetOrganizations = targetOrganizations;
            ViewBag.Target2Organizations = target2Organizations;

            var defaultTargetId = command != null
                ? command.TargetDomainId
                : targetOrganizations.FirstOrDefault()?.Id;

            var defaultTarget2Id = command != null
                ? command.Target2DomainId
                : organization.SuzerainId != null
                    ? organization.SuzerainId
                    : targetOrganizations.FirstOrDefault()?.Id;

            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations.OrderBy(o => o.Name), "Id", "Name", defaultTargetId);
            ViewData["Target2OrganizationId"] = new SelectList(target2Organizations.OrderBy(o => o.Name), "Id", "Name", defaultTarget2Id);

            var editCommand = new VassalTransferCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private IActionResult Investments(Command command)
        {
            if (command == null || command.Type != enDomainCommandType.Investments)
            {
                return NotFound();
            }

            var editCommand = new InvestmentsCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private IActionResult Fortifications(Command command)
        {
            if (command == null || command.Type != enDomainCommandType.Fortifications)
            {
                return NotFound();
            }

            var editCommand = new FortificationsCommand(command);
            return View("EditOrCreate", editCommand);
        }

        // POST: Commands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeInt,TargetDomainId,Target2DomainId," +
            "Coffers,Warriors,DomainId")] Command command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            if (!ValidCommand(id, out var realCommand, out var userDomain))
                return NotFound();

            command.Type = (enDomainCommandType)command.TypeInt;

            realCommand.Coffers = command.Coffers;
            realCommand.Warriors = command.Warriors;
            realCommand.Type = command.Type;
            realCommand.TargetDomainId = command.TargetDomainId;
            realCommand.Target2DomainId = command.Target2DomainId;

            try
            {
                _context.Update(realCommand);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandExists(realCommand.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index), new { organizationId = realCommand.DomainId });
        }

        // GET: Commands/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ValidCommand(id.Value, out var command, out var userDomain))
                return NotFound();

            _context.Commands.Remove(command);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { organizationId = command.DomainId });
        }

        private bool CommandExists(int id)
        {
            return _context.Commands.Any(e => e.Id == id);
        }

        private async Task<Dictionary<string, List<int>>> FillResources(int organizationId, int initiatorId, int? withoutCommandId = null)
        {
            var organization = await _context.Domains
                .FindAsync(organizationId);

            var dictionary = new Dictionary<string, List<int>>();
            var busyCoffers = organization.Commands
                .Where(c => withoutCommandId == null || c.Id != withoutCommandId)
                .Where(c => c.InitiatorPersonId == initiatorId)
                .Sum(c => c.Coffers);
            var busyWarriors = organization.Units
                .Where(c => withoutCommandId == null || c.Id != withoutCommandId)
                .Where(c => c.InitiatorPersonId == initiatorId)
                .Sum(c => c.Warriors);
            dictionary.Add("Казна", new List<int>(3)
            {
                organization.Coffers,
                busyCoffers,
                organization.Coffers - busyCoffers
            });
            dictionary.Add("Инвестиции", new List<int>(3)
            {
                organization.InvestmentsShowed,
                0,
                organization.InvestmentsShowed
            });
            dictionary.Add("Укрепления", new List<int>(3)
            {
                organization.Fortifications,
                0,
                organization.Fortifications
            });
            return dictionary;
        }

        private bool ValidCommand(int commandId, out Command command, out Domain userDomain)
        {
            var currentUser = _userManager.GetCurrentUser(HttpContext.User, _context).Result;
            command = _context.Commands.Find(commandId);

            if (!UserHelper.ValidDomain(_context, currentUser, command.DomainId, out _, out userDomain))
                return false;

            if (command.InitiatorPersonId != userDomain.PersonId)
                return false;

            return true;
        }
    }
}
