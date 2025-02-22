using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Infrastructure.AI;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Users;
using YAGO.World.Infrastructure.Helpers;
using YAGO.World.Infrastructure.Helpers.Commands;

namespace YAGO.World.Host.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly TurnRunNextTask _endOfTurnService;
        private readonly IConfiguration _configuration;
        private readonly IRepositoryOrganizations _repositoryOrganizations;

        public AdminController(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<HomeController> logger,
            IConfiguration configuration,
            TurnRunNextTask endOfTurnService,
            IRepositoryOrganizations repositoryOrganizations)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _endOfTurnService = endOfTurnService;
            _repositoryOrganizations = repositoryOrganizations;
        }

        public IActionResult CheckTurn(string id)
        {
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            var domains = _repositoryOrganizations.GetAll();
            foreach (var domain in domains)
            {
                CommandHelper.CheckAndFix(_context, domain.Id);
            }

            AIHelper.AICommandsPrepare(_context);

            _endOfTurnService.Execute();
            return RedirectToAction("Index", "Home");
        }
    }
}
