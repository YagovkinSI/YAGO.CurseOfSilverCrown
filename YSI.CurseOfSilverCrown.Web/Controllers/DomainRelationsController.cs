using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.GameRelations;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class DomainRelationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DomainRelationsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DomainRelations
        public async Task<IActionResult> Index(int? organizationId)
        {
            if (organizationId == null)
                return NotFound();

            if (!ValidDomain(organizationId.Value, out var domain, out var userDomain))
                return NotFound();

            ViewBag.Domain = domain;

            var ralations = await _context.DomainRelations
                .Where(r => r.SourceDomainId == organizationId)
                .ToListAsync();

            return View(ralations);
        }

        // GET: DomainRelations/Create
        public async Task<IActionResult> CreateOrEditAsync(int? domainId, int? id)
        {
            if (domainId == null)
                return NotFound();

            if (!ValidDomain(domainId.Value, out var domain, out var userDomain))
                return NotFound();

            var domainRelation = id == null
                ? null
                : _context.DomainRelations
                    .SingleOrDefault(r => r.Id == id);

            var targetOrganizations = DomainRelationHelper.GetAvailableTargets(_context, domain.Id).Result.ToList();
            if (domainRelation != null)
            {
                targetOrganizations.Add(await _context.Domains
                    .FindAsync(domainRelation.TargetDomainId));
            }

            ViewBag.TargetOrganizations = targetOrganizations;
            ViewBag.Organization = domain;

            var defaultTargetId = domainRelation != null
                ? domainRelation.TargetDomainId
                : targetOrganizations.FirstOrDefault()?.Id;

            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations.OrderBy(o => o.Name), "Id", "Name", defaultTargetId);
            return View("CreateOrEdit", domainRelation);
        }

        // POST: Commands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(
            [Bind("SourceDomainId,TargetDomainId,IsIncludeVassals,PermissionOfPassage")] DomainRelation domainRelation)
        {
            if (!ValidDomain(domainRelation.SourceDomainId, out var domain, out var userDomain))
                return NotFound();

            if (domainRelation.TargetDomainId == 0)
                return RedirectToAction(nameof(Index), new { organizationId = domain.Id });

            if (ModelState.IsValid)
            {
                _context.Add(domainRelation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { organizationId = domain.Id });
            }

            return RedirectToAction(nameof(Index), new { organizationId = domain.Id });
        }

        // POST: Commands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id, SourceDomainId,TargetDomainId,IsIncludeVassals,PermissionOfPassage")] DomainRelation domainRelation)
        {
            if (id != domainRelation.Id)
            {
                return NotFound();
            }

            if (!ValidDomain(domainRelation.SourceDomainId, out var domain, out var userDomain))
                return NotFound();

            var realDomainRelation = await _context.DomainRelations
                .FirstOrDefaultAsync(o => o.Id == id);

            if (realDomainRelation.SourceDomainId != userDomain.Id)
                return NotFound();

            realDomainRelation.TargetDomainId = domainRelation.TargetDomainId;
            realDomainRelation.IsIncludeVassals = domainRelation.IsIncludeVassals;
            realDomainRelation.PermissionOfPassage = domainRelation.PermissionOfPassage;

            try
            {
                _context.Update(realDomainRelation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index), new { organizationId = realDomainRelation.SourceDomainId });
        }

        // GET: DomainRelations/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var domainRelation = _context.DomainRelations
                .SingleOrDefault(r => r.Id == id);
            if (domainRelation == null)
                return NotFound();

            _context.DomainRelations.Remove(domainRelation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { organizationId = domainRelation.SourceDomainId });
        }

        private bool ValidDomain(int domainId, out Domain domain, out Domain userDomain)
        {
            var currentUser = _userManager.GetCurrentUser(HttpContext.User, _context).Result;
            var domainFromDb = _context.Domains
                .FirstOrDefault(o => o.Id == domainId);
            domain = _context.Domains
                .Find(domainFromDb.Id);
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
                        .Find(d.Id))
                    .First();

            return true;
        }
    }
}
