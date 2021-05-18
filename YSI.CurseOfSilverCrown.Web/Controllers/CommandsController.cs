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
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            if (currentUser == null)
                return NotFound();
            if (currentUser.DomainId == null)
                return RedirectToAction("Index", "Organizations");

            if (organizationId == null)
                organizationId = currentUser.DomainId;

            var organization = await _context.Domains
                .Include(c => c.Vassals)
                .Include(c => c.Suzerain)
                .SingleAsync(o => o.Id == organizationId);

            if (!_context.Commands.Any(c => c.DomainId == organizationId &&
                    c.InitiatorDomainId == currentUser.DomainId))
            {
                CreatorCommandForNewTurn.CreateNewCommandsForOrganizations(_context, currentUser.DomainId.Value, organization);
            }

            var commands = await _context.Commands
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .Include(c => c.Target2)
                .Where(c => c.DomainId == organizationId &&
                    c.InitiatorDomainId == currentUser.DomainId)
                .ToListAsync();

            var allCommands = commands
                .Cast<ICommand>()
                .ToList();
            var units = await _context.Units
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .Include(c => c.Target2)
                .Where(c => c.DomainId == organizationId &&
                    c.InitiatorDomainId == currentUser.DomainId)
                .Cast<ICommand>()
                .ToListAsync();
            allCommands.AddRange(units);

            ViewBag.Budget = new Budget(_context, organization, allCommands);
            ViewBag.InitiatorId = currentUser.DomainId;

            return View(commands);
        }

        // GET: Commands/Create
        public async Task<IActionResult> CreateAsync(int type, int? organizationId)
        {
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            if (currentUser == null)
                return NotFound();
            if (currentUser.DomainId == null)
                return NotFound();

            if (organizationId == null)
                organizationId = currentUser.DomainId;
            var domain = await _context.Domains.FindAsync(organizationId);

            var allWarriors = DomainHelper.GetWarriorCount(_context, domain.Id);
            ViewBag.Organization = new OrganizationInfo(domain);

            ViewBag.Resourses = await FillResources(organizationId.Value, currentUser.DomainId.Value);

            switch ((enCommandType)type)
            {
                case enCommandType.VassalTransfer:
                    return await VassalTransferAsync(null, currentUser.DomainId.Value, organizationId.Value);
                case enCommandType.GoldTransfer:
                    return await GoldTransferAsync(null, currentUser.DomainId.Value, organizationId.Value);
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
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            if (currentUser == null)
                return NotFound();
            if (currentUser.DomainId == null)
                return NotFound();

            command.Type = (enCommandType)command.TypeInt;
            if (new [] { enCommandType.VassalTransfer, enCommandType.GoldTransfer }.Contains(command.Type) && 
                command.TargetDomainId == null)
                return RedirectToAction("Index", "Commands");

            if (new[] { enCommandType.VassalTransfer }.Contains(command.Type) &&
                command.Target2DomainId == null)
                return RedirectToAction("Index", "Commands");

            if (command.DomainId == 0)
                command.DomainId = currentUser.DomainId.Value;
            command.InitiatorDomainId = currentUser.DomainId.Value;
            command.Status = command.DomainId == currentUser.DomainId
                ? enCommandStatus.ReadyToRun
                : enCommandStatus.ReadyToSend;

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
        public async Task<IActionResult> Edit(int? id, bool optimizeIdleness = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            var command = await _context.Commands
                .FirstOrDefaultAsync(o => o.Id == id);

            if (currentUser == null)
                return NotFound();
            if (currentUser.DomainId == null)
                return NotFound();
            if (command == null)
                return NotFound();
            if (command.InitiatorDomainId != currentUser.DomainId)
                return NotFound();


            var organization = await _context.Domains.FindAsync(command.DomainId);
            ViewBag.Organization = new OrganizationInfo(organization);

            ViewBag.Resourses = await FillResources(command.DomainId, currentUser.DomainId.Value, command.Id);

            switch (command.Type)
            {
                case enCommandType.Idleness:
                    return Idleness(command, optimizeIdleness);
                case enCommandType.Growth:
                    return Growth(command);
                case enCommandType.Investments:
                    return Investments(command);
                case enCommandType.Fortifications:
                    return Fortifications(command);
                case enCommandType.VassalTransfer:
                    return await VassalTransferAsync(command, currentUser.DomainId.Value, command.DomainId);
                case enCommandType.GoldTransfer:
                    return await GoldTransferAsync(command, currentUser.DomainId.Value, command.DomainId);
                default:
                    return NotFound();
            }
        }

        private IActionResult Idleness(Command command, bool optimizeIdleness)
        {
            if (command == null || command.Type != enCommandType.Idleness)
            {
                return NotFound();
            }

            if (optimizeIdleness)
            {
                command.Coffers = IdlenessHelper.GetOptimizedCoffers();
                _context.Update(command);
                _context.SaveChangesAsync();
            }

            ViewBag.Optimized = IdlenessHelper.IsOptimized(command.Coffers);
            return View("Idleness", command);
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

            ViewBag.TargetOrganizations = OrganizationInfo.GetOrganizationInfoList(targetOrganizations);
            var defaultTargetId = command != null
                ? command.TargetDomainId
                : targetOrganizations.FirstOrDefault()?.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations.OrderBy(o => o.Name), "Id", "Name", defaultTargetId);

            var editCommand = new GoldTransferCommand(command);
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

            ViewBag.TargetOrganizations = OrganizationInfo.GetOrganizationInfoList(targetOrganizations);
            ViewBag.Target2Organizations = OrganizationInfo.GetOrganizationInfoList(target2Organizations);

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

            command.Type = (enCommandType)command.TypeInt;
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            var realCommand = await _context.Commands
                .FirstOrDefaultAsync(o => o.Id == id);

            if (currentUser == null)
                return NotFound();
            if (currentUser.DomainId == null)
                return NotFound();
            if (realCommand == null)
                return NotFound();
            if (realCommand.InitiatorDomainId != currentUser.DomainId)
                return NotFound();

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

            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            var command = await _context.Commands
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (command == null || command.InitiatorDomainId != currentUser.DomainId)
            {
                return NotFound();
            }

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
                .Where(c => c.Type != enCommandType.Idleness)
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
    }
}
