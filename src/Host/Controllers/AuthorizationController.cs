using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cities.Interfaces;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Host.Controllers.Models.CurrentUsers;
using YAGO.World.Host.Models.Authorization;
using YAGO.World.Host.Models.Authorization.Mappings;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/authorization")]
    public class AuthorizationController : Controller
    {
        private readonly ICurrentUserService _currentUserService;

        public AuthorizationController(
            ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [Route("getAuthorizationData")]
        public async Task<AuthorizationData> GetAuthorizationData(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _currentUserService.GetCurrentUser(HttpContext.User, cancellationToken);
            return currentUser.ToAuthorizationData();
        }

        [HttpPost]
        [Route("register")]
        public async Task<AuthorizationData> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _currentUserService.Register(
                registerRequest.UserName,
                registerRequest.Password,
                registerRequest.Email,
                cancellationToken);
            return currentUser.ToAuthorizationData();
        }

        [HttpPost("autoRegister")]
        public async Task<AuthorizationData> AutoRegister(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _currentUserService.AutoRegister(cancellationToken);
            return currentUser.ToAuthorizationData();
        }

        [HttpPost("changeRegistration")]
        [Authorize]
        public async Task<AuthorizationData> ChangeRegistration(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _currentUserService.ChangeRegistration(
                User,
                registerRequest.UserName,
                registerRequest.Email,
                registerRequest.Password,
                cancellationToken);
            return currentUser.ToAuthorizationData();
        }

        [HttpPost]
        [Route("login")]
        public async Task<AuthorizationData> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _currentUserService.Login(loginRequest.UserName, loginRequest.Password, cancellationToken);
            return currentUser.ToAuthorizationData();
        }

        [HttpPost]
        [Route("logout")]
        public async Task<AuthorizationData> Logout(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _currentUserService.Logout(cancellationToken);
            return AuthorizationData.NotAuthorized;
        }
    }
}
