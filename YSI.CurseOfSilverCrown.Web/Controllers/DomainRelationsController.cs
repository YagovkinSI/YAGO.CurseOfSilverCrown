using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Core.BL.Models;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;

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
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            if (currentUser == null)
                return NotFound();
            if (currentUser.DomainId == null)
                return RedirectToAction("Index", "Organizations");

            if (organizationId == null)
                organizationId = currentUser.DomainId;

            var ralations = await _context.DomainRelations
                .Include(r => r.TargetDomain)
                .Where(r => r.SourceDomainId == organizationId)
                .ToListAsync();

            return View(ralations);
        }

        // GET: DomainRelations/Create
        public async Task<IActionResult> CreateOrEditAsync(int? id)
        {
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            if (currentUser == null)
                return NotFound();
            if (currentUser.DomainId == null)
                return RedirectToAction("Index", "Organizations");

            var domainRelation = id == null
                ? null
                : _context.DomainRelations
                    .SingleOrDefault(r => r.Id == id);

            var organizationId = currentUser.DomainId.Value;

            var organization = await _context.Domains.FindAsync(organizationId);

            var targetOrganizations = DomainRelationHelper.GetAvailableTargets(_context, organizationId).Result.ToList();
            if (domainRelation != null)
                targetOrganizations.Add(await _context.GetDomainMin(domainRelation.TargetDomainId));

            ViewBag.TargetOrganizations = targetOrganizations;
            ViewBag.Organization = organization;

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
            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);

            if (currentUser == null)
                return NotFound();
            if (currentUser.DomainId == null)
                return NotFound();

            if (domainRelation.SourceDomainId == 0)
                domainRelation.SourceDomainId = currentUser.DomainId.Value;
            if (domainRelation.TargetDomainId == 0)
                return RedirectToAction(nameof(Index), new { organizationId = currentUser.DomainId.Value });

            if (ModelState.IsValid)
            {
                _context.Add(domainRelation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { organizationId = currentUser.DomainId.Value });
            }

            return RedirectToAction(nameof(Index), new { organizationId = currentUser.DomainId.Value });
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

            var currentUser = await _userManager.GetCurrentUser(HttpContext.User, _context);
            var realDomainRelation = await _context.DomainRelations
                .FirstOrDefaultAsync(o => o.Id == id);

            if (currentUser == null)
                return NotFound();
            if (currentUser.DomainId == null)
                return NotFound();
            if (realDomainRelation == null)
                return NotFound();
            if (realDomainRelation.SourceDomainId != currentUser.DomainId)
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
            return RedirectToAction(nameof(Index));
        }
    }
}
