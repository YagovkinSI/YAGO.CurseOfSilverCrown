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
using YSI.CurseOfSilverCrown.Core.Parameters;

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
            if (organizationId == null)
                return NotFound();

            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            if (currentUser == null)
                return NotFound();
            if (currentUser.PersonId == null)
                return RedirectToAction("Index", "Organizations");

            if (!UserHelper.ValidDomain(_context, currentUser, organizationId.Value, out var unitDomain, out var userDomain))
                return NotFound();            

            if (!unitDomain.Units.Any(c => c.InitiatorPersonId == userDomain.PersonId))
            {
                CreatorCommandForNewTurn.CreateNewCommandsForOrganizations(_context, userDomain.PersonId, unitDomain);
            }

            var units = _context.Units
                .Include(d => d.Domain)
                .Include(d => d.Target)
                .Include(d => d.Target2)
                .Include(d => d.Position)
                .Include(d => d.PersonInitiator)
                .Where(d => d.DomainId == organizationId.Value && d.InitiatorPersonId == userDomain.PersonId);

            if (units.Count() == 0)
                throw new Exception($"В БД нет юнитов для владения {organizationId.Value} и инициатора {userDomain.PersonId}");

            ViewBag.Budget = new Budget(_context, unitDomain, userDomain.PersonId);

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

            if (unitFrom.DomainId != unitTo.DomainId || unitFrom.PositionDomainId != unitTo.PositionDomainId)
                return NotFound();

            unitTo.Warriors += unitFrom.Warriors;
            unitTo.Coffers += unitFrom.Coffers;

            _context.Update(unitTo);
            _context.Remove(unitFrom);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EditUnit), new { id = unitTo.Id });
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

            if (unit == null || unit.Warriors <= separateCount)
                return NotFound();            

            var newUnit = new Unit
            {
                Warriors = separateCount,
                Coffers = 0,
                InitiatorPersonId = unit.InitiatorPersonId,
                DomainId = unit.DomainId,
                PositionDomainId = unit.PositionDomainId,
                Status = unit.Status,
                Type = enArmyCommandType.WarSupportDefense,
                TypeInt = (int)enArmyCommandType.WarSupportDefense,
                TargetDomainId = unit.DomainId,
                ActionPoints = WarConstants.ActionPointsFullCount
            };
            unit.Warriors -= separateCount;

            _context.Update(unit);
            _context.Add(newUnit);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EditUnit), new { id = newUnit.Id });
        }

        // GET: Commands/EditUnit/5
        [Authorize]
        public async Task<IActionResult> EditUnit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ValidUnit(id.Value, out var unit, out var userDomain))
                return NotFound();

            unit = await _context.Units
                .Include(d => d.Domain)
                .Include(d => d.Target)
                .Include(d => d.Target2)
                .Include(d => d.Position)
                .Include(d => d.PersonInitiator)
                .SingleAsync(d => d.Id == unit.Id);

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

            var commandType = (enArmyCommandType)type;

            ViewBag.Organization = await _context.Domains
                .Include(d => d.Units)
                .Include(d => d.Suzerain)
                .Include(d => d.Vassals)
                .SingleAsync(d => d.Id == unit.DomainId);

            ViewBag.Resourses = await FillResources(unit.DomainId, userDomain.PersonId, unit.Id);

            switch (commandType)
            {
                case enArmyCommandType.CollectTax:
                    return CollectTax(unit);
                case enArmyCommandType.War:
                    return await WarAsync(unit, userDomain.PersonId, unit.DomainId);
                case enArmyCommandType.WarSupportDefense:
                    return await WarSupportDefenseAsync(unit, userDomain.PersonId, unit.DomainId);
                case enArmyCommandType.WarSupportAttack:
                    return await WarSupportAttackAsync(unit, userDomain.PersonId, unit.DomainId);
                default:
                    return NotFound();
            }
        }

        private IActionResult CollectTax(Unit command)
        {
            var editCommand = new CollectTaxCommand(command);
            return View("EditOrCreate", editCommand);
        }        

        private async Task<IActionResult> WarAsync(Unit unit, int initiatorId, int organizationId)
        {
            ViewBag.IsOwnCommand = initiatorId == organizationId;

            var targetOrganizations = await WarHelper.GetAvailableTargets(_context, organizationId, initiatorId, unit);

            ViewBag.TargetOrganizations = targetOrganizations;
            var defaultTargetId = unit != null && unit.TargetDomainId != null
                ? unit.TargetDomainId
                : targetOrganizations.FirstOrDefault()?.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations.OrderBy(o => o.Name), "Id", "Name", defaultTargetId);

            var editCommand = new WarCommand(unit);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> WarSupportAttackAsync(Unit unit, int initiatorId, int organizationId)
        {
            ViewBag.IsOwnCommand = initiatorId == organizationId;

            var targetOrganizations = await WarSupportAttackHelper.GetAvailableTargets(_context, organizationId, initiatorId, unit);
            ViewBag.TargetOrganizations = targetOrganizations;
            var defaultTargetId = unit != null && unit.TargetDomainId != null
                ? unit.TargetDomainId
                : targetOrganizations.FirstOrDefault()?.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations.OrderBy(o => o.Name), "Id", "Name", defaultTargetId);


            var target2Organizations = await WarSupportAttackHelper.GetAvailableTargets2(_context);
            ViewBag.Target2Organizations = target2Organizations;
            var defaultTarget2Id = unit != null && unit.Target2DomainId != null
                ? unit.Target2DomainId
                : target2Organizations.Any(o => o.Id == initiatorId)
                    ? initiatorId
                    : target2Organizations.FirstOrDefault()?.Id;
            ViewData["Target2OrganizationId"] = new SelectList(target2Organizations.OrderBy(o => o.Name), "Id", "Name", defaultTarget2Id);

            var editCommand = new WarSupportAttackCommand(unit);
            return View("EditOrCreate", editCommand);
        }

        private async Task<IActionResult> WarSupportDefenseAsync(Unit unit, int initiatorId, int organizationId)
        {
            ViewBag.IsOwnCommand = initiatorId == organizationId;

            var targetOrganizations = 
                await WarSupportDefenseHelper.GetAvailableTargets(_context, organizationId, initiatorId, unit);            

            ViewBag.TargetOrganizations = targetOrganizations;
            var defaultTargetId = unit != null && unit.TargetDomainId != null
                ? unit.TargetDomainId
                : targetOrganizations.Any(o => o.Id == organizationId)
                    ? organizationId
                    : targetOrganizations.FirstOrDefault()?.Id;
            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations.OrderBy(o => o.Name), "Id", "Name", defaultTargetId);


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

            unit.Type = (enArmyCommandType)unit.TypeInt;

            if (!ValidUnit(id, out var realUnit, out var userDomain))
                return NotFound();

            realUnit.Coffers = unit.Coffers;
            realUnit.Warriors = unit.Warriors;
            realUnit.Type = unit.Type;
            realUnit.TargetDomainId = unit.TargetDomainId;
            realUnit.Target2DomainId = unit.Target2DomainId;

            try
            {
                _context.Update(realUnit);
                await _context.SaveChangesAsync();
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

        private bool ValidUnit(int unitId, out Unit unit, out Domain userDomain)
        {
            var currentUser = _userManager.GetCurrentUser(HttpContext.User, _context).Result;
            unit = _context.Units.Find(unitId);

            if (!UserHelper.ValidDomain(_context, currentUser, unit.DomainId, out _, out userDomain))
                return false;

            if (unit.InitiatorPersonId != userDomain.PersonId)
                return false;

            return true;
        }
    }
}
