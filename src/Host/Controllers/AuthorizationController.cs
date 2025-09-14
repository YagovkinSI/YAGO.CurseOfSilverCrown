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
        public Task<AuthorizationData> GetAuthorizationData(CancellationToken cancellationToken)
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
        public async Task<AuthorizationData> AutoRegister(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _currentUserService.AutoRegister(cancellationToken);
        }

        [HttpPost("changeRegistration")]
        [Authorize]
        public async Task<AuthorizationData> ChangeRegistration(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _currentUserService.ChangeRegistration(
                User,
                registerRequest.UserName,
                registerRequest.Email,
                registerRequest.Password,
                cancellationToken);
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
        public async Task<AuthorizationData> Logout(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _currentUserService.Logout(cancellationToken);
            return AuthorizationData.NotAuthorized;
        }
    }
}
