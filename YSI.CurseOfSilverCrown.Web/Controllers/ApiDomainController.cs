using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [Route("getDomainList")]
        public async Task<ActionResult<List<DomainPublic2>>> GetDomainList(int column = 0)
        {
            try
            {
                var domains = await DomainHelper.GetDomainPublic2OrderByColumn(_context, column);
                return Ok(domains);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
