using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUser;
using YAGO.World.Domain.CurrentUser;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/currentUser")]
    public class CurrentUserController : Controller
    {
        private readonly ICurrentUserService _currentUserService;

        public CurrentUserController(
            ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<AuthorizationData> Index(CancellationToken cancellationToken) =>
            await _currentUserService.GetAuthorizationData(User, cancellationToken);
    }
}
