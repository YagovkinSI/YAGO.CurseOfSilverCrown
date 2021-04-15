using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Actions;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

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
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
                return NotFound();
            if (string.IsNullOrEmpty(currentUser.OrganizationId))
                return RedirectToAction("Index", "Provinces");

            var applicationDbContext = _context.Commands
                .Include(c => c.Organization)
                .Include(c => c.Target)
                .Where(c => c.OrganizationId == currentUser.OrganizationId);

            ViewBag.Resourses = await FillResources(currentUser.OrganizationId);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Commands/Create
        public async Task<IActionResult> CreateAsync(int type)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
                return NotFound();
            if (string.IsNullOrEmpty(currentUser.OrganizationId))
                return NotFound();

            ViewBag.Resourses = await FillResources(currentUser.OrganizationId);

            ViewBag.ClosedCommands = await GetClosedCommands(currentUser.OrganizationId);

            switch ((Enums.enCommandType)type)
            {
                case Enums.enCommandType.War:
                    return await CreateWarAsync(currentUser);
                default:
                    return NotFound();
            }
        }

        private async Task<IActionResult> CreateWarAsync(User currentUser)
        {
            var allOrganizations = await _context.Organizations
                .Include(o => o.Province)
                .Include(o => o.Vassals)
                .Where(o => o.OrganizationType == Enums.enOrganizationType.Lord)
                .ToListAsync();

            var userOrganization = allOrganizations.First(o => o.Id == currentUser.OrganizationId);
            var targetOrganizations = userOrganization.SuzerainId == null
                ? allOrganizations.Where(o => o.Id != currentUser.OrganizationId && !userOrganization.Vassals.Any(v => v.Id == o.Id))
                : allOrganizations.Where(o => o.Id == userOrganization.SuzerainId);

            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations, "Id", "Province.Name");

            return View("CreateWar");
        }

        // POST: Commands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Type,TargetOrganizationId,Coffers,Warriors")] Command command)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
                return NotFound();
            if (string.IsNullOrEmpty(currentUser.OrganizationId))
                return NotFound();

            command.OrganizationId = currentUser.OrganizationId;
            command.Id = Guid.NewGuid().ToString();

            if (ModelState.IsValid)
            {
                _context.Add(command);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var allOrganizations = await _context.Organizations
                .Include(o => o.Province)
                .Include(o => o.Vassals)
                .Where(o => o.OrganizationType == Enums.enOrganizationType.Lord)
                .ToListAsync();

            var userOrganization = allOrganizations.First(o => o.Id == currentUser.OrganizationId);
            var targetOrganizations = userOrganization.SuzerainId == null
                ? allOrganizations.Where(o => o.Id != currentUser.OrganizationId && !userOrganization.Vassals.Any(v => v.Id == o.Id))
                : allOrganizations.Where(o => o.Id == userOrganization.SuzerainId);

            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations, "Id", "Province.Name");
            return View(command);
        }



        // GET: Commands/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id, bool optimizeIdleness = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var command = await _context.Commands
                .FirstOrDefaultAsync(o => o.Id == id);

            if (currentUser == null)
                return NotFound();
            if (string.IsNullOrEmpty(currentUser.OrganizationId))
                return NotFound();
            if (command == null)
                return NotFound();
            if (command.OrganizationId != currentUser.OrganizationId)
                return NotFound();


            ViewBag.Resourses = await FillResources(currentUser.OrganizationId, command.Id);
            ViewBag.ClosedCommands = await GetClosedCommands(currentUser.OrganizationId, command.Id);

            switch (command.Type)
            {
                case Enums.enCommandType.Idleness:
                    return Idleness(command, optimizeIdleness);
                case Enums.enCommandType.Growth:
                    return Growth(command);
                case Enums.enCommandType.CollectTax:
                    return CollectTax(command);
                case Enums.enCommandType.War:
                    return await WarAsync(currentUser, command);
                default:
                    return NotFound();
            }
        }

        private IActionResult Idleness(Command command, bool optimizeIdleness)
        {
            if (command == null || command.Type != Enums.enCommandType.Idleness)
            {
                return NotFound();
            }

            if (optimizeIdleness)
            {
                command.Coffers = IdlenessAction.GetOptimizedCoffers();
                _context.Update(command);
                _context.SaveChangesAsync();
            }

            ViewBag.Optimized = IdlenessAction.IsOptimized(command.Coffers);
            return View("Idleness", command);
        }

        private IActionResult Growth(Command command)
        {
            if (command == null || command.Type != Enums.enCommandType.Growth)
            {
                return NotFound();
            }

            return View("Growth", command);
        }

        private IActionResult CollectTax(Command command)
        {
            if (command == null || command.Type != Enums.enCommandType.CollectTax)
            {
                return NotFound();
            }

            return View("CollectTax", command);
        }

        private async Task<IActionResult> WarAsync(User currentUser, Command command)
        {
            if (command == null || command.Type != Enums.enCommandType.War)
            {
                return NotFound();
            }


            var allOrganizations = await _context.Organizations
                .Include(o => o.Province)
                .Include(o => o.Vassals)
                .Where(o => o.OrganizationType == Enums.enOrganizationType.Lord)
                .ToListAsync();

            var userOrganization = allOrganizations.First(o => o.Id == currentUser.OrganizationId);
            var targetOrganizations = userOrganization.SuzerainId == null
                ? allOrganizations.Where(o => o.Id != currentUser.OrganizationId && !userOrganization.Vassals.Any(v => v.Id == o.Id))
                : allOrganizations.Where(o => o.Id == userOrganization.SuzerainId);

            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations, "Id", "Province.Name", command.TargetOrganizationId);
            return View("War", command);
        }

        // POST: Commands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Type,TargetOrganizationId,Coffers,Warriors")] Command command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var realCommand = await _context.Commands
                .FirstOrDefaultAsync(o => o.Id == id);

            if (currentUser == null)
                return NotFound();
            if (string.IsNullOrEmpty(currentUser.OrganizationId))
                return NotFound();
            if (realCommand == null)
                return NotFound();
            if (realCommand.OrganizationId != currentUser.OrganizationId)
                return NotFound();

            realCommand.Coffers = command.Coffers;
            realCommand.Warriors = command.Warriors;
            realCommand.Type = command.Type;
            realCommand.TargetOrganizationId = command.TargetOrganizationId;

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
            return RedirectToAction("Index");
        }

        // GET: Commands/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var command = await _context.Commands
                .Include(c => c.Organization)
                .Include(c => c.Target)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (command == null || command.OrganizationId != currentUser.OrganizationId)
            {
                return NotFound();
            }

            return View(command);
        }

        // POST: Commands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var command = await _context.Commands.FindAsync(id);
            _context.Commands.Remove(command);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommandExists(string id)
        {
            return _context.Commands.Any(e => e.Id == id);
        }
        
        private async Task<Dictionary<string, List<int>>> FillResources(string organizationId, string withoutCommandId = null)
        {
            var organization = await _context.Organizations
                .Include(o => o.Commands)
                .SingleAsync(o => o.Id == organizationId);

            var dictionary = new Dictionary<string, List<int>>();
            var busyCoffers = organization.Commands
                .Where(c => string.IsNullOrEmpty(withoutCommandId) || c.Id != withoutCommandId)
                .Where(c => c.Type != Enums.enCommandType.Idleness)
                .Sum(c => c.Coffers);
            var busyWarriors = organization.Commands
                .Where(c => string.IsNullOrEmpty(withoutCommandId) || c.Id != withoutCommandId)
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
            return dictionary;
        }

        private async Task<string[]> GetClosedCommands(string organizationId, string withoutCommandId = null)
        {
            var organization = await _context.Organizations
                .Include(o => o.Commands)
                .SingleAsync(o => o.Id == organizationId);

            var multyCommandTypes = new[]
            {
                Enums.enCommandType.War
            };

            var closedCoomands = _context.Commands
                .Where(c => c.OrganizationId == organizationId)
                .Where(c => string.IsNullOrEmpty(withoutCommandId) || c.Id != withoutCommandId)
                .Where(c => !multyCommandTypes.Contains(c.Type));
            
            var types = closedCoomands.Select(c => ((int)c.Type).ToString()).ToArray();
            return types;
        }
    }
}
