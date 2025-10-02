using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Users;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.MyUsers;
using YAGO.World.Host.Controllers.MyUsers.Mappings;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/me/user")]
    public class MyUserController : Controller
    {
        private readonly IUserService _myUserService;

        public MyUserController(
            IUserService currentUserService)
        {
            _myUserService = currentUserService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<MyDataResponse<MyUser>> Get(CancellationToken cancellationToken)
        {
            var currentUser = await _myUserService.GetMyUser(HttpContext.User, cancellationToken);
            return currentUser.ToMyDataResponse();
        }

        [HttpPost]
        [Route("register")]
        public async Task<MyDataResponse<MyUser>> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
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
            var currentUser = await _myUserService.Login(loginRequest.UserName, loginRequest.Password, cancellationToken);
            return currentUser.ToMyDataResponse();
        }

        [HttpPost]
        [Route("logout")]
        public async Task<MyDataResponse<MyUser>> Logout(CancellationToken cancellationToken)
        {
            await _myUserService.Logout(User, cancellationToken);
            return MyDataResponse<MyUser>.NotAuthorized;
        }

        [HttpPost("createTemporaryUser")]
        public async Task<MyDataResponse<MyUser>> CreateTemporaryUser(CancellationToken cancellationToken)
        {
            var currentUser = await _myUserService.CreateTemporaryUser(cancellationToken);
            return currentUser.ToMyDataResponse();
        }

        [HttpPost("convertToPermanentUser")]
        [Authorize]
        public async Task<MyDataResponse<MyUser>> ConvertToPermanentUser(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
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
