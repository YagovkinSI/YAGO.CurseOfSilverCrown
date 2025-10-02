using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Models;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Host.Controllers.Models.CurrentUsers;
using YAGO.World.Host.Controllers.Models.CurrentUsers.Mappings;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/me/user")]
    public class MyUserController : Controller
    {
        private readonly IMyUserService _myUserService;

        public MyUserController(
            IMyUserService currentUserService)
        {
            _myUserService = currentUserService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<MyDataResponse<MyUser>> Get(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _myUserService.GetMyUser(HttpContext.User, cancellationToken);
            return currentUser.ToMyDataResponse();
        }

        [HttpPost]
        [Route("register")]
        public async Task<MyDataResponse<MyUser>> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _myUserService.Register(
                registerRequest.UserName,
                registerRequest.Password,
                registerRequest.Email,
                cancellationToken);
            return currentUser.ToMyDataResponse();
        }

        [HttpPost]
        [Route("login")]
        public async Task<MyDataResponse<MyUser>> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _myUserService.Login(loginRequest.UserName, loginRequest.Password, cancellationToken);
            return currentUser.ToMyDataResponse();
        }

        [HttpPost]
        [Route("logout")]
        public async Task<MyDataResponse<MyUser>> Logout(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _myUserService.Logout(cancellationToken);
            return MyDataResponse<MyUser>.NotAuthorized;
        }

        [HttpPost("createTemporaryUser")]
        public async Task<MyDataResponse<MyUser>> CreateTemporaryUser(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _myUserService.CreateTemporaryUser(cancellationToken);
            return currentUser.ToMyDataResponse();
        }

        [HttpPost("convertToPermanentUser")]
        [Authorize]
        public async Task<MyDataResponse<MyUser>> ConvertToPermanentUser(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _myUserService.ConvertToPermanentUser(
                User,
                registerRequest.UserName,
                registerRequest.Email,
                registerRequest.Password,
                cancellationToken);
            return currentUser.ToMyDataResponse();
        }
    }
}
