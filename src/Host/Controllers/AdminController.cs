using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.EndOfTurn.Interfaces;

namespace YAGO.World.Host.Controllers
{
    public class AdminController : Controller
    {
        private readonly IEndOfTurnProcess _endOfTurnService;
        private readonly IConfiguration _configuration;

        public AdminController(
            IConfiguration configuration,
            IEndOfTurnProcess endOfTurnService)
        {
            _configuration = configuration;
            _endOfTurnService = endOfTurnService;
        }

        public async Task<IActionResult> CheckTurnAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var realCode = _configuration.GetValue<string>("EndOfTurnCode");
            if (id != realCode)
                return NotFound();

            cancellationToken.ThrowIfCancellationRequested();
            await _endOfTurnService.Execute();

            return RedirectToAction("Index", "Home");
        }
    }
}
