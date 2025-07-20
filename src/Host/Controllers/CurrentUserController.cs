using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Domain.CurrentUsers;
using YAGO.World.Host.Controllers.Models.CurrentUsers;

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

        [HttpGet]
        [Route("getCurrentUser")]
        public Task<AuthorizationData> GetCurrentUser(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _currentUserService.GetAuthorizationData(HttpContext.User, cancellationToken);
        }

        [HttpPost]
        [Route("register")]
        public Task<AuthorizationData> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _currentUserService.Register(registerRequest.UserName, registerRequest.Email, registerRequest.Password, cancellationToken);
        }

        [HttpPost("autoRegister")]
        public Task<AuthorizationData> AutoRegister(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _currentUserService.AutoRegister(cancellationToken);
        }

        [HttpPost("changeRegistration")]
        [Authorize]
        public Task<AuthorizationData> ChangeRegistration(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _currentUserService.UpgradeRegister(User, registerRequest.UserName, registerRequest.Email, registerRequest.Password, cancellationToken);
        }

        [HttpPost]
        [Route("login")]
        public Task<AuthorizationData> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _currentUserService.Login(loginRequest.UserName, loginRequest.Password, cancellationToken);
        }

        [HttpPost]
        [Route("logout")]
        public Task Logout(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _currentUserService.Logout(cancellationToken);
        }
    }
}
