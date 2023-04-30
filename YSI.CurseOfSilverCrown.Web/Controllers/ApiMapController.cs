using Microsoft.AspNetCore.Mvc;
using System;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiMapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiMapController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getMap")]
        public IActionResult GetMap()
        {
            try
            {
                var map = MapHelper.GetMap(_context);
                return Ok(map);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
