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
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Infrastructure.APIModels.BudgetModels;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Database.Models.Users;
using YAGO.World.Infrastructure.Helpers;
using YAGO.World.Infrastructure.Helpers.Commands;
using YAGO.World.Infrastructure.Helpers.Commands.DomainCommands;

namespace YAGO.World.Host.Controllers
{
    public class CommandsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositoryCommads _repositoryCommads;

        public CommandsController(ApplicationDbContext context, 
            UserManager<User> userManager, 
            ILogger<HomeController> logger,
            IRepositoryCommads repositoryCommads)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _repositoryCommads = repositoryCommads;
        }

        // GET: Commands
        [Authorize]
        public async Task<IActionResult> Index(int? organizationId)
        {
            var currentUser = organizationId == null
                ? await UserHelper.GetCurrentUser(_userManager, HttpContext.User, _context)
                : await UserHelper.AccessСheckAndGetCurrentUser(_context, _userManager, HttpContext.User, organizationId);

            if (currentUser == null)
                return RedirectToAction("Index", "Organizations");

            organizationId ??= currentUser.Domains.Single().Id;

            await _repositoryCommads.CheckAndFix(organizationId.Value);

            var commands = await _context.Commands
                .Where(c => c.DomainId == organizationId)
                .ToListAsync();

            var domain = await _context.Domains.FindAsync(organizationId.Value);
            ViewBag.Budget = new Budget(_context, domain);

            var userDomain = await _context.Domains.SingleAsync(d => d.UserId == currentUser.Id);
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

            var userDomain = await _context.Domains.SingleAsync(d => d.UserId == currentUser.Id);
            ViewBag.Resourses = await FillResources(organizationId.Value);

            return (CommandType)type switch
            {
                CommandType.VassalTransfer => await VassalTransferAsync(null, organizationId.Value),
                CommandType.GoldTransfer => await GoldTransferAsync(null, organizationId.Value),
                CommandType.Rebellion => Rebellion(null),
                _ => NotFound(),
            };
        }

        // POST: Commands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("TypeInt,TargetDomainId,Target2DomainId," +
            "Gold,Warriors,DomainId")] Command command)
        {
            var currentUser =
                await UserHelper.AccessСheckAndGetCurrentUser(_context, _userManager, HttpContext.User, command.DomainId);
            if (currentUser == null)
                return RedirectToAction("Index", "Organizations");

            command.Type = (CommandType)command.TypeInt;
            if (new[] { CommandType.VassalTransfer, CommandType.GoldTransfer }.Contains(command.Type) &&
                command.TargetDomainId == null)
            {
                return RedirectToAction("Index", "Commands");
            }

            if (new[] { CommandType.VassalTransfer }.Contains(command.Type) &&
                command.Target2DomainId == null)
            {
                return RedirectToAction("Index", "Commands");
            }

            command.Status = CommandStatus.ReadyToMove;

            if (ModelState.IsValid)
            {
                _ = _context.Add(command);
                _ = await _context.SaveChangesAsync();
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

            ViewBag.Resourses = await FillResources(command.DomainId, command.Id);

            return command.Type switch
            {
                CommandType.Growth => Growth(command),
                CommandType.Investments => Investments(command),
                CommandType.Fortifications => Fortifications(command),
                CommandType.VassalTransfer => await VassalTransferAsync(command, command.DomainId),
                CommandType.GoldTransfer => await GoldTransferAsync(command, command.DomainId),
                CommandType.Rebellion => Rebellion(command),
                _ => NotFound(),
            };
        }

        private IActionResult Growth(Command command)
        {
            if (command == null || command.Type != CommandType.Growth)
            {
                return NotFound();
            }

            var editCommand = new GrowthCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> GoldTransferAsync(Command command, int organizationId)
        {
            if (command != null && command.Type != CommandType.GoldTransfer)
                return NotFound();

            var targetOrganizations = await GoldTransferHelper.GetAvailableTargets(_context, organizationId, command);

            ViewBag.TargetOrganizations = targetOrganizations;
            var defaultTargetId = command != null
                ? command.TargetDomainId
                : targetOrganizations.FirstOrDefault()?.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations.OrderBy(o => o.Name), "Id", "Name", defaultTargetId);

            var editCommand = new GoldTransferCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private IActionResult Rebellion(Command command)
        {
            if (command != null && command.Type != CommandType.Rebellion)
            {
                return NotFound();
            }

            var editCommand = new RebelionCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> VassalTransferAsync(Command command, int organizationId)
        {
            if (command != null && command.Type != CommandType.VassalTransfer)
            {
                return NotFound();
            }

            var organization = await _context.Domains.FindAsync(organizationId);

            var targetOrganizations = await VassalTransferHelper.GetAvailableTargets(_context, organizationId, command);
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
            if (command == null || command.Type != CommandType.Investments)
            {
                return NotFound();
            }

            var editCommand = new InvestmentsCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private IActionResult Fortifications(Command command)
        {
            if (command == null || command.Type != CommandType.Fortifications)
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
            "Gold,Warriors,DomainId")] Command command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            if (!ValidCommand(id, out var realCommand, out var userDomain))
                return NotFound();

            command.Type = (CommandType)command.TypeInt;

            realCommand.Gold = command.Gold;
            realCommand.Warriors = command.Warriors;
            realCommand.Type = command.Type;
            realCommand.TargetDomainId = command.TargetDomainId;
            realCommand.Target2DomainId = command.Target2DomainId;

            try
            {
                _ = _context.Update(realCommand);
                _ = await _context.SaveChangesAsync();
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

            _ = _context.Commands.Remove(command);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { organizationId = command.DomainId });
        }

        private bool CommandExists(int id) => _context.Commands.Any(e => e.Id == id);

        private async Task<Dictionary<string, List<int>>> FillResources(int organizationId, int? withoutCommandId = null)
        {
            var organization = await _context.Domains
                .FindAsync(organizationId);

            var dictionary = new Dictionary<string, List<int>>();
            var busyCoffers = organization.Commands
                .Where(c => withoutCommandId == null || c.Id != withoutCommandId)
                .Sum(c => c.Gold);
            var busyWarriors = organization.Units
                .Where(c => withoutCommandId == null || c.Id != withoutCommandId)
                .Sum(c => c.Warriors);
            dictionary.Add("Казна", new List<int>(3)
            {
                organization.Gold,
                busyCoffers,
                organization.Gold - busyCoffers
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

        private bool ValidCommand(int commandId, out Command command, out Organization userDomain)
        {
            userDomain = null;
            command = _context.Commands.Find(commandId);

            var currentUser = _userManager.GetCurrentUser(HttpContext.User, _context).Result;
            if (currentUser == null)
                return false;

            var domain = command.GetDomain(_context);
            if (domain.UserId != currentUser.Id)            
                return false;
            
            userDomain = domain;
            return true;
        }
    }
}
