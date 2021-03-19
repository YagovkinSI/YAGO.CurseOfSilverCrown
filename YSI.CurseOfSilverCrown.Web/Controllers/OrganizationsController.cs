using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Web.Data;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrganizationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetCurrentUserId()
        {
            var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        // GET: Provinces/My
        public async Task<IActionResult> My()
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
                return NotFound();

            var currentUser = _context.Users
                    .FirstOrDefault(u => u.Id == currentUserId);
            if (currentUser == null)
                return NotFound();

            if (string.IsNullOrEmpty(currentUser.OrganizationId))
                return NotFound();

            var organisation = await _context.Organizations
                .Include(o => o.Province)
                .Include(o => o.Suzerain)
                .Include("Suzerain.Province")
                .Include(o => o.Vassals)
                .Include("Vassals.Province")
                .SingleAsync(o => o.Id == currentUser.OrganizationId);

            return View(organisation);
        }

        // GET: Provinces/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var organisation = await _context.Organizations
                .Include(o => o.User)
                .Include(o => o.Province)
                .Include(o => o.Suzerain)
                .Include("Suzerain.Province")
                .Include(o => o.Vassals)
                .Include("Vassals.Province")
                .SingleAsync(o => o.Id == id);

            return View(organisation);
        }
    }
}
