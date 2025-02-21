using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Host.APIModels;
using YAGO.World.Host.APIModels.BudgetModels;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Units;
using YAGO.World.Host.Database.Users;
using YAGO.World.Host.Helpers;
using YAGO.World.Host.Helpers.Commands;
using YAGO.World.Host.Helpers.Commands.UnitCommands;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Controllers
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
            var currentUser = organizationId == null
                ? await UserHelper.GetCurrentUser(_userManager, HttpContext.User, _context)
                : await UserHelper.AccessСheckAndGetCurrentUser(_context, _userManager, HttpContext.User, organizationId);

            if (currentUser == null)
                return RedirectToAction("Index", "Organizations");

            organizationId ??= currentUser.Domains.SingleOrDefault()?.Id;

            CommandHelper.CheckAndFix(_context, organizationId.Value);

            var units = _context.Units
                .Include(d => d.Position)
                .Where(d => d.DomainId == organizationId.Value);

            var domain = await _context.Domains.FindAsync(organizationId.Value);
            ViewBag.Budget = new Budget(_context, domain);

            return View(units);
        }

        // GET: Commands
        [Authorize]
        public async Task<IActionResult> Union(int? id, int? toUnitId)
        {
            if (id == null || toUnitId == null || id == toUnitId)
            {
                return NotFound();
            }

            if (!ValidUnit(id.Value, out var unitFrom, out var userDomainFrom))
                return NotFound();
            if (!ValidUnit(toUnitId.Value, out var unitTo, out var userDomainTo))
                return NotFound();

            if (userDomainFrom.Id != userDomainTo.Id)
                return NotFound();

            var success = await UnitHelper.TryUnion(unitTo, unitFrom, _context);

            return !success ? NotFound() : RedirectToAction(nameof(EditUnit), new { id = unitTo.Id });
        }

        // POST: Commands/Separate
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Separate(int unitId, int separateCount)
        {
            if (separateCount < 1)
                return NotFound();

            if (!ValidUnit(unitId, out var unit, out var userDomain))
                return NotFound();

            var (success, newUnit) = await UnitHelper.TrySeparate(unit, separateCount, _context);

            return !success ? NotFound() : RedirectToAction(nameof(EditUnit), new { id = newUnit.Id });
        }

        // GET: Commands/EditUnit/5
        [Authorize]
        public async Task<IActionResult> EditUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ValidUnit(id.Value, out var unit, out _))
                return NotFound();

            unit = await _context.Units
                .FindAsync(unit.Id);

            var unitEditor = new UnitEditor(unit, _context);
            return View("EditUnit", unitEditor);
        }

        // GET: Commands/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id, int type)
        {
            if (id == null || type == 0)
            {
                return NotFound();
            }

            if (!ValidUnit(id.Value, out var unit, out var userDomain))
                return NotFound();

            var commandType = (UnitCommandType)type;

            ViewBag.Organization = await _context.Domains
                .FindAsync(unit.DomainId);

            ViewBag.Resourses = await FillResources(unit.DomainId, unit.Id);

            return commandType switch
            {
                UnitCommandType.CollectTax => CollectTax(unit),
                UnitCommandType.War => await WarAsync(unit, unit.DomainId),
                UnitCommandType.WarSupportDefense => await WarSupportDefenseAsync(unit, unit.DomainId),
                UnitCommandType.WarSupportAttack => await WarSupportAttackAsync(unit, unit.DomainId),
                _ => NotFound(),
            };
        }

        private IActionResult CollectTax(Unit command)
        {
            var editCommand = new CollectTaxCommand(command);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> WarAsync(Unit unit, int organizationId)
        {
            var targetOrganizations =
                await WarBaseHelper.GetAvailableTargets(_context, organizationId, unit, UnitCommandType.War);

            ViewBag.TargetOrganizations = targetOrganizations;
            var defaultTargetId = unit != null && unit.TargetDomainId != null
                ? unit.TargetDomainId
                : targetOrganizations.FirstOrDefault()?.TargetDomain.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations, "TargetDomain.Id", "RouteName", defaultTargetId);

            var editCommand = new WarCommand(unit);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> WarSupportAttackAsync(Unit unit, int organizationId)
        {
            var targetOrganizations =
                await WarBaseHelper.GetAvailableTargets(_context, organizationId, unit, UnitCommandType.WarSupportAttack);
            ViewBag.TargetOrganizations = targetOrganizations;
            var defaultTargetId = unit != null && unit.TargetDomainId != null
                ? unit.TargetDomainId
                : targetOrganizations.FirstOrDefault()?.TargetDomain.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations, "TargetDomain.Id", "RouteName", defaultTargetId);


            var target2Organizations = await WarSupportAttackHelper.GetAvailableTargets2(_context);
            ViewBag.Target2Organizations = target2Organizations;
            var defaultTarget2Id = unit != null && unit.Target2DomainId != null
                ? unit.Target2DomainId
                : target2Organizations.Any(o => o.Id == unit.DomainId)
                    ? unit.DomainId
                    : target2Organizations.FirstOrDefault()?.Id;
            ViewData["Target2OrganizationId"] = new SelectList(target2Organizations.OrderBy(o => o.Name), "Id", "Name", defaultTarget2Id);

            var editCommand = new WarSupportAttackCommand(unit);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> WarSupportDefenseAsync(Unit unit, int organizationId)
        {
            var targetOrganizations =
                await WarBaseHelper.GetAvailableTargets(_context, organizationId, unit, UnitCommandType.WarSupportDefense);

            ViewBag.TargetOrganizations = targetOrganizations;
            var defaultTargetId = unit != null && unit.TargetDomainId != null
                ? unit.TargetDomainId
                : targetOrganizations.Any(o => o.TargetDomain.Id == organizationId)
                    ? organizationId
                    : targetOrganizations.FirstOrDefault()?.TargetDomain.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations, "TargetDomain.Id", "RouteName", defaultTargetId);


            var editCommand = new WarSupportDefenseCommand(unit);
            return View("EditOrCreate", editCommand);
        }

        // POST: Commands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeInt,TargetDomainId,Target2DomainId," +
            "Coffers,Warriors,DomainId")] Unit unit)
        {
            if (id != unit.Id)
            {
                return NotFound();
            }

            unit.Type = (UnitCommandType)unit.TypeInt;

            if (!ValidUnit(id, out var realUnit, out var userDomain))
                return NotFound();

            realUnit.Gold = unit.Gold;
            realUnit.Warriors = unit.Warriors;
            realUnit.Type = unit.Type;
            realUnit.TargetDomainId = unit.TargetDomainId;
            realUnit.Target2DomainId = unit.Target2DomainId;

            try
            {
                _ = _context.Update(realUnit);
                _ = await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandExists(realUnit.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index), new { organizationId = realUnit.DomainId });
        }

        private bool CommandExists(int id) => _context.Units.Any(e => e.Id == id);

        private async Task<Dictionary<string, List<int>>> FillResources(int organizationId, int initiatorId, int? withoutCommandId = null)
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

        private bool ValidUnit(int unitId, out Unit unit, out Organization userDomain)
        {
            userDomain = null;
            unit = _context.Units.Find(unitId);

            var currentUser = _userManager.GetCurrentUser(HttpContext.User, _context).Result;
            if (currentUser == null)
                return false;

            var domain = unit.Domain;
            if (domain.UserId != currentUser.Id)
                return false;

            userDomain = domain;
            return true;
        }
    }
}
