using Microsoft.AspNetCore.Mvc;
using System;
using YSI.CurseOfSilverCrown.Core.APIModels;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Helpers;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiDomainController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiDomainController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getDomainPublic")]
        public ActionResult<DomainPublic> GetDomainPublic(int domainId)
        {
            try
            {
                var domainPublic = DomainHelper.GetDomainPublic(_context, domainId);
                return Ok(domainPublic);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
