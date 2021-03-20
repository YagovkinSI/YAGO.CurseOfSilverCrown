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
    public class ProvincesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public string GetCurrentUserId()
        {
            var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public ProvincesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Provinces
        public async Task<IActionResult> Index()
        {
            var currentUser = (User)null;
            var currentUserId = GetCurrentUserId();
            if (currentUserId != null)
            {
                currentUser = _context.Users
                    .Include(u => u.Organization)
                    .FirstOrDefault(u => u.Id == currentUserId);
            }

            ViewBag.CanTake = currentUser != null && currentUser.Organization == null;
            return View(await _context.Provinces
                .Include(p => p.Organizations)
                .Include("Organizations.User")
                .ToListAsync());
        }

        // GET: Provinces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Provinces
                .Include(p => p.Organizations)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (province == null)
            {
                return NotFound();
            }

            var organization = province.Organizations
                .FirstOrDefault(o => o.OrganizationType == Enums.enOrganizationType.Lord);
            if (organization == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Organizations", new { id = organization.Id });
        }

        // GET: Provinces/Take/5
        public async Task<IActionResult> Take(int? id)
        {
            if (id == null)
                return NotFound();

            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
                return NotFound();

            var currentUser = _context.Users
                    .Include(u => u.Organization)
                    .FirstOrDefault(u => u.Id == currentUserId);
            if (currentUser == null)
                return NotFound();

            if (currentUser.Organization != null)
                return NotFound();

            var organizationLord = _context.Organizations
                .Single(o => 
                    o.ProvinceId == id.Value &&
                    o.OrganizationType == Enums.enOrganizationType.Lord);

            currentUser.Organization = organizationLord;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
