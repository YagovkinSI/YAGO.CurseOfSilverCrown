using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YSI.CurseOfSilverCrown.EndOfTurn;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.ViewModels;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Interfaces;

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
            if (organizationId == null)
                return NotFound();

            if (!ValidDomain(organizationId.Value, out var domain, out var userDomain))
                return NotFound();

            if (!_context.Commands.Any(c => c.DomainId == organizationId &&
                    c.InitiatorDomainId == userDomain.Id))
            {
                CreatorCommandForNewTurn.CreateNewCommandsForOrganizations(_context, userDomain.Id, domain);
            }

            var commands = await _context.Commands
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .Include(c => c.Target2)
                .Where(c => c.DomainId == organizationId &&
                    c.InitiatorDomainId == userDomain.Id)
                .ToListAsync();

            var allCommands = commands
                .Cast<ICommand>()
                .ToList();
            var units = await _context.Units
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .Include(c => c.Target2)
                .Where(c => c.DomainId == organizationId &&
                    c.InitiatorDomainId == userDomain.Id)
                .Cast<ICommand>()
                .ToListAsync();
            allCommands.AddRange(units);

            ViewBag.Budget = new Budget(_context, domain, allCommands);
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
            if (organizationId == null)
                return NotFound();

            if (!ValidDomain(organizationId.Value, out var domain, out var userDomain))
                return NotFound();

            ViewBag.Organization = domain;

            ViewBag.Resourses = await FillResources(organizationId.Value, userDomain.Id);

            switch ((enCommandType)type)
            {
                case enCommandType.VassalTransfer:
                    return await VassalTransferAsync(null, userDomain.Id, organizationId.Value);
                case enCommandType.GoldTransfer:
                    return await GoldTransferAsync(null, userDomain.Id, organizationId.Value);
                case enCommandType.Rebellion:
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
            if (!ValidDomain(command.DomainId, out var domain, out var userDomain))
                return NotFound();

            command.Type = (enCommandType)command.TypeInt;
            if (new [] { enCommandType.VassalTransfer, enCommandType.GoldTransfer }.Contains(command.Type) && 
                command.TargetDomainId == null)
                return RedirectToAction("Index", "Commands");

            if (new[] { enCommandType.VassalTransfer }.Contains(command.Type) &&
                command.Target2DomainId == null)
                return RedirectToAction("Index", "Commands");

            command.InitiatorDomainId = userDomain.Id;
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
                .Include(d => d.Units)
                .Include(d => d.Suzerain)
                .Include(d => d.Vassals)
                .SingleAsync(d => d.Id == command.DomainId);

            ViewBag.Resourses = await FillResources(command.DomainId, userDomain.Id, command.Id);

            switch (command.Type)
            {
                case enCommandType.Growth:
                    return Growth(command);
                case enCommandType.Investments:
                    return Investments(command);
                case enCommandType.Fortifications:
                    return Fortifications(command);
                case enCommandType.VassalTransfer:
                    return await VassalTransferAsync(command, userDomain.Id, command.DomainId);
                case enCommandType.GoldTransfer:
                    return await GoldTransferAsync(command, userDomain.Id, command.DomainId);
                case enCommandType.Rebellion:
                    return Rebellion(command, userDomain.Id, command.DomainId);
                default:
                    return NotFound();
            }
        }

        private IActionResult Growth(Command command)
        {
            if (command == null || command.Type != enCommandType.Growth)
            {
                return NotFound();
            }

            var editCommand = new GrowthCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> GoldTransferAsync(Command command, int userOrganizationId, int organizationId)
        {
            if (command != null && command.Type != enCommandType.GoldTransfer)
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
            if (command != null && command.Type != enCommandType.Rebellion)
            {
                return NotFound();
            }

            ViewBag.IsOwnCommand = userOrganizationId == organizationId;

            var editCommand = new RebelionCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> VassalTransferAsync(Command command, int userOrganizationId, int organizationId)
        {
            if (command != null && command.Type != enCommandType.VassalTransfer)
            {
                return NotFound();
            }

            ViewBag.IsOwnCommand = userOrganizationId == organizationId;

            var organization = await _context.Domains.FindAsync(organizationId);

            var targetOrganizations = await VassalTransferHelper.GetAvailableTargets(_context, organizationId, userOrganizationId, command);
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
            return View("VassalTransfer", command);
        }

        private IActionResult Investments(Command command)
        {
            if (command == null || command.Type != enCommandType.Investments)
            {
                return NotFound();
            }

            return View("Investments", command);
        }

        private IActionResult Fortifications(Command command)
        {
            if (command == null || command.Type != enCommandType.Fortifications)
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

            command.Type = (enCommandType)command.TypeInt;            

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
                .Include(o => o.Commands)
                .Include(o => o.Units)
                .SingleAsync(o => o.Id == organizationId);

            var dictionary = new Dictionary<string, List<int>>();
            var busyCoffers = organization.Commands
                .Where(c => withoutCommandId == null || c.Id != withoutCommandId)
                .Where(c => c.InitiatorDomainId == initiatorId)
                .Sum(c => c.Coffers);
            var busyWarriors = organization.Units
                .Where(c => withoutCommandId == null || c.Id != withoutCommandId)
                .Where(c => c.InitiatorDomainId == initiatorId)
                .Sum(c => c.Warriors);
            dictionary.Add("Казна", new List<int>(3)
            { 
                organization.Coffers,
                busyCoffers,
                organization.Coffers - busyCoffers
            });
            dictionary.Add("Инвестиции", new List<int>(3)
            {
                organization.Investments,
                0,
                organization.Investments
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
            var commandFromDb = _context.Commands
                .FirstOrDefault(o => o.Id == commandId);
            command = commandFromDb;
            userDomain = null;

            if (currentUser == null)
                return false;
            if (currentUser.PersonId == null)
                return false;

            var unitDomain = _context.Domains
                .Include(d => d.Units)
                .Include(d => d.Suzerain)
                .Include(d => d.Vassals)
                .Single(d => d.Id == commandFromDb.DomainId);
            userDomain = unitDomain.PersonId == currentUser.PersonId
                ? unitDomain
                : _context.Domains
                    .Where(d => d.Id == unitDomain.SuzerainId && d.PersonId == currentUser.PersonId)
                    .Select(d => _context.Domains
                        .Include(d => d.Units)
                        .Include(d => d.Suzerain)
                        .Include(d => d.Vassals)
                        .Single(d2 => d2.Id == d.Id))
                    .First();

            if (command.InitiatorDomainId != userDomain.Id)
                return false;

            return true;
        }

        private bool ValidDomain(int domainId, out Domain domain, out Domain userDomain)
        {
            var currentUser = _userManager.GetCurrentUser(HttpContext.User, _context).Result;
            var domainFromDb = _context.Domains
                .FirstOrDefault(o => o.Id == domainId);
            domain = _context.Domains
                .Include(d => d.Units)
                .Include(d => d.Suzerain)
                .Include(d => d.Vassals)
                .SingleAsync(d => d.Id == domainFromDb.Id)
                .Result;
            userDomain = null;

            if (currentUser == null)
                return false;
            if (currentUser.PersonId == null)
                return false;

            userDomain = domain.PersonId == currentUser.PersonId
                ? domain
                : _context.Domains
                    .Where(d => d.Id == domainFromDb.SuzerainId && d.PersonId == currentUser.PersonId)
                    .Select(d => _context.Domains
                        .Include(d => d.Units)
                        .Include(d => d.Suzerain)
                        .Include(d => d.Vassals)
                        .Single(d2 => d2.Id == d.Id))                        
                    .First();

            return true;
        }
    }
}
