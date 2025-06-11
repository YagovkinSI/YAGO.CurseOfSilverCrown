using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using YAGO.World.Infrastructure.APIModels;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Helpers;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/map")]
    public class ApiMapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiMapController(ApplicationDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, MapElement> Index() => KingdomHelper.GetDomainColors(_context, isNewApi: true);
    }
}
