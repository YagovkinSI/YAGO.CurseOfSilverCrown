using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Helpers.Events;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiEventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiEventController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getEvents")]
        public async Task<ActionResult<List<List<string>>>> GetEventsAsync()
        {
            try
            {
                var events = await EventHelper.GetTopHistory(_context);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
