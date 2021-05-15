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
using YSI.CurseOfSilverCrown.Core.EndOfTurn;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.ViewModels;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Interfaces;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class UnitsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public UnitsController(ApplicationDbContext context, UserManager<User> userManager, ILogger<HomeController> logger)
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

            if (!_context.Units.Any(c => c.DomainId == organizationId &&
                    c.InitiatorDomainId == currentUser.DomainId))
            {
                CreatorCommandForNewTurn.CreateNewCommandsForOrganizations(_context, currentUser.DomainId, organization);
            }

            var units = await _context.Units
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .Include(c => c.Target2)
                .Where(c => c.DomainId == organizationId &&
                    c.InitiatorDomainId == currentUser.DomainId)
                .ToListAsync();

            var allCommands = await _context.Commands
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .Include(c => c.Target2)
                .Where(c => c.DomainId == organizationId && 
                    c.InitiatorDomainId == currentUser.DomainId)
                .Cast<ICommand>()
                .ToListAsync();

            allCommands.AddRange(units.Cast<ICommand>());

            var currentTurn = await _context.Turns
                .SingleAsync(t => t.IsActive);

            ViewBag.Budget = new Budget(organization, allCommands, currentTurn);
            ViewBag.InitiatorId = currentUser.DomainId;

            return View(units);
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
            var organization = await _context.Domains.FindAsync(organizationId);
            ViewBag.Organization = new OrganizationInfo(organization);

            ViewBag.Resourses = await FillResources(organizationId.Value, currentUser.DomainId.Value);

            switch ((enArmyCommandType)type)
            {
                case enArmyCommandType.War:
                    return await WarAsync(null, currentUser.DomainId.Value, organizationId.Value);
                case enArmyCommandType.WarSupportDefense:
                    return await WarSupportDefenseAsync(null, currentUser.DomainId.Value, organizationId.Value);
                case enArmyCommandType.WarSupportAttack:
                    return await WarSupportAttackAsync(null, currentUser.DomainId.Value, organizationId.Value);
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
            "Coffers,Warriors,DomainId")] Unit command)
        {
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            if (currentUser == null)
                return NotFound();
            if (currentUser.DomainId == null)
                return NotFound();

            command.Type = (enArmyCommandType)command.TypeInt;
            if (new [] { enArmyCommandType.War, enArmyCommandType.WarSupportDefense, enArmyCommandType.WarSupportAttack }
                .Contains(command.Type) && 
                command.TargetDomainId == null)
                return RedirectToAction("Index", "Units");

            if (new[] { enArmyCommandType.WarSupportAttack }.Contains(command.Type) &&
                command.Target2DomainId == null)
                return RedirectToAction("Index", "Units");

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
            var command = await _context.Units
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
                case enArmyCommandType.CollectTax:
                    return CollectTax(command);
                case enArmyCommandType.War:
                    return await WarAsync(command, currentUser.DomainId.Value, command.DomainId);
                case enArmyCommandType.WarSupportDefense:
                    return await WarSupportDefenseAsync(command, currentUser.DomainId.Value, command.DomainId);
                case enArmyCommandType.Rebellion:
                    return await RebellionAsync(command, currentUser.DomainId.Value);
                case enArmyCommandType.WarSupportAttack:
                    return await WarSupportAttackAsync(command, currentUser.DomainId.Value, command.DomainId);
                default:
                    return NotFound();
            }
        }        

        private IActionResult CollectTax(Unit command)
        {
            if (command == null || command.Type != enArmyCommandType.CollectTax)
            {
                return NotFound();
            }

            var editCommand = new CollectTaxCommand(command);
            return View("EditOrCreate", editCommand);
        }        

        private async Task<IActionResult> WarAsync(Unit command, int userOrganizationId, int organizationId)
        {
            if (command != null && command.Type != enArmyCommandType.War)
                return NotFound();

            ViewBag.IsOwnCommand = userOrganizationId == organizationId;

            var targetOrganizations = await WarHelper.GetAvailableTargets(_context, organizationId, userOrganizationId, command);

            ViewBag.TargetOrganizations = OrganizationInfo.GetOrganizationInfoList(targetOrganizations);
            var defaultTargetId = command != null
                ? command.TargetDomainId
                : targetOrganizations.FirstOrDefault()?.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations.OrderBy(o => o.Name), "Id", "Name", defaultTargetId);
            return View("War", command);
        }

        private async Task<IActionResult> RebellionAsync(Unit command, int userOrganizationId)
        {
            if (command != null && command.Type != enArmyCommandType.Rebellion)
                return NotFound();

            var userOrganization = await _context.Domains
                .FindAsync(userOrganizationId);

            var currentTurn = await _context.Turns
                .SingleAsync(t => t.IsActive);

            var targetOrganizations = new List<Domain>();
            if (userOrganization.SuzerainId != null)
            {
                var suzerain = await _context.Domains.FindAsync(userOrganization.SuzerainId);
                targetOrganizations.Add(suzerain);
            }

            ViewBag.TargetOrganizations = OrganizationInfo.GetOrganizationInfoList(targetOrganizations);
            ViewBag.TurnCountBeforeRebelion = userOrganization.TurnOfDefeat + RebelionHelper.TurnCountWithoutRebelion < currentTurn.Id
                ? 0
                : userOrganization.TurnOfDefeat + RebelionHelper.TurnCountWithoutRebelion - currentTurn.Id + 1;
            return View("Rebelion", command);
        }

        private async Task<IActionResult> WarSupportAttackAsync(Unit command, int userOrganizationId, int organizationId)
        {
            if (command != null && command.Type != enArmyCommandType.WarSupportAttack)
            {
                return NotFound();
            }

            ViewBag.IsOwnCommand = userOrganizationId == organizationId;

            var targetOrganizations = await WarSupportAttackHelper.GetAvailableTargets(_context, organizationId, userOrganizationId, command);
            ViewBag.TargetOrganizations = OrganizationInfo.GetOrganizationInfoList(targetOrganizations);
            var defaultTargetId = command != null
                ? command.TargetDomainId
                : targetOrganizations.FirstOrDefault()?.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations.OrderBy(o => o.Name), "Id", "Name", defaultTargetId);


            var target2Organizations = await WarSupportAttackHelper.GetAvailableTargets2(_context, organizationId, command);
            ViewBag.Target2Organizations = OrganizationInfo.GetOrganizationInfoList(targetOrganizations);
            var defaultTarget2Id = command != null
                ? command.Target2DomainId
                : target2Organizations.Any(o => o.Id == userOrganizationId)
                    ? userOrganizationId
                    : target2Organizations.FirstOrDefault()?.Id;
            ViewData["Target2OrganizationId"] = new SelectList(target2Organizations.OrderBy(o => o.Name), "Id", "Name", defaultTarget2Id);

            var editCommand = new WarSupportAttackCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> WarSupportDefenseAsync(Unit command, int userOrganizationId, int organizationId)
        {
            if (command != null && command.Type != enArmyCommandType.WarSupportDefense)
            {
                return NotFound();
            }

            ViewBag.IsOwnCommand = userOrganizationId == organizationId;

            var targetOrganizations = await WarSupportDefenseHelper.GetAvailableTargets(_context, organizationId, userOrganizationId, command);            

            ViewBag.TargetOrganizations = OrganizationInfo.GetOrganizationInfoList(targetOrganizations);
            var defaultTargetId = command != null
                ? command.TargetDomainId
                : targetOrganizations.Any(o => o.Id == organizationId)
                    ? organizationId
                    : targetOrganizations.FirstOrDefault()?.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations.OrderBy(o => o.Name), "Id", "Name", defaultTargetId);


            var editCommand = new WarSupportDefenseCommand(command);
            return View("EditOrCreate", editCommand);
        }        

        // POST: Commands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeInt,TargetDomainId,Target2DomainId," +
            "Coffers,Warriors,DomainId")] Unit command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            command.Type = (enArmyCommandType)command.TypeInt;
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            var realCommand = await _context.Units
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

            var command = await _context.Units
                .Include(c => c.Domain)
                .Include(c => c.Target)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (command == null || command.InitiatorDomainId != currentUser.DomainId)
            {
                return NotFound();
            }

            _context.Units.Remove(command);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { organizationId = command.DomainId });
        }

        private bool CommandExists(int id)
        {
            return _context.Units.Any(e => e.Id == id);
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
            dictionary.Add("Воины", new List<int>(3)
            {
                organization.Warriors,
                busyWarriors,
                organization.Warriors - busyWarriors
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
