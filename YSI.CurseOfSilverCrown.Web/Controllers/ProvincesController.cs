using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class ProvincesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;

        public ProvincesController(ApplicationDbContext context, UserManager<User> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Provinces
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.CanTake = currentUser != null && currentUser.OrganizationId == null;
            return View(await _context.Provinces
                .Include(p => p.Organizations)
                .Include("Organizations.Suzerain")
                .Include("Organizations.Vassals")
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

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
                return NotFound();

            if (currentUser.OrganizationId != null)
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
