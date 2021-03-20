using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class CommandsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommandsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetCurrentUserId()
        {
            var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        // GET: Commands/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
                return NotFound();

            var currentUser = _context.Users
                    .FirstOrDefault(u => u.Id == currentUserId);
            if (currentUser == null)
                return NotFound();

            if (string.IsNullOrEmpty(currentUser.OrganizationId))
                return NotFound();

            var command = await _context.Commands
                .Include(o => o.Turn)
                .FirstOrDefaultAsync(o => o.Id == id && o.OrganizationId == currentUser.OrganizationId);

            if (command == null || command.Turn.IsActive == false)
            {
                return NotFound();
            }

            var allOrganizations = _context.Organizations
                .Include(o => o.Province)
                .Where(o => o.OrganizationType == Enums.enOrganizationType.Lord)
                .ToList();
            var userOrganization = allOrganizations.First(o => o.Id == currentUser.OrganizationId);

            var targetOrganizations = userOrganization.SuzerainId == null
                ? allOrganizations.Where(o => o.Id != currentUser.OrganizationId)
                : allOrganizations.Where(o => o.Id == userOrganization.SuzerainId);

            ViewData["TargetOrganizationId"] = new SelectList(targetOrganizations, "Id", "Province.Name", command.TargetOrganizationId);
            return View(command);
        }

        // POST: Commands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,TurnId,OrganizationId,Type,TargetOrganizationId,Result")] Command command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
                return NotFound();

            var currentUser = _context.Users
                    .FirstOrDefault(u => u.Id == currentUserId);
            if (currentUser == null)
                return NotFound();

            if (string.IsNullOrEmpty(currentUser.OrganizationId))
                return NotFound();   
            
            var realCommand = await _context.Commands
                .Include(o => o.Turn)
                .FirstOrDefaultAsync(o => o.Id == id && o.OrganizationId == currentUser.OrganizationId);

            if (realCommand == null)
            {
                return NotFound();
            }

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
            return RedirectToAction("My", "Organizations");
        }

        private bool CommandExists(string id)
        {
            return _context.Commands.Any(e => e.Id == id);
        }
    }
}
