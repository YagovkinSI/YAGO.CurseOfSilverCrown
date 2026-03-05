using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Users;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Users;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/me/user")]
    public class MyUserController : ControllerBase
    {
        private readonly IUserService _userService;

        public MyUserController(
            IUserService currentUserService)
        {
            _userService = currentUserService;
        }

        [HttpGet]
        [Route("get")]
        public async Task<MyDataResponse<MyUser>> Get(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return MyDataResponse<MyUser>.NotAuthorized;

            var userId = User.GetUserId();
            var currentUser = await _userService.GetMyUser(userId, cancellationToken);
            return currentUser.ToMyDataResponse();
        }

        [HttpPost]
        [Route("register")]
        public async Task<ApiResponse<MyDataResponse<MyUser>>> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            var currentUser = await _userService.Register(
                registerRequest.UserName,
                registerRequest.Password,
                registerRequest.Email,
                cancellationToken);
            var data = currentUser.ToMyDataResponse();
            return ApiResponse<MyDataResponse<MyUser>>.CreateSuccess(data);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ApiResponse<MyDataResponse<MyUser>>> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var currentUser = await _userService.Login(loginRequest.UserName, loginRequest.Password, cancellationToken);
            var data = currentUser.ToMyDataResponse();
            return ApiResponse<MyDataResponse<MyUser>>.CreateSuccess(data);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<ApiResponse<MyDataResponse<MyUser>>> Logout(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return await Task.FromResult(ApiResponse<MyDataResponse<MyUser>>.CreateSuccess(MyDataResponse<MyUser>.NotAuthorized));

            await _userService.Logout(cancellationToken);
            return ApiResponse<MyDataResponse<MyUser>>.CreateSuccess(MyDataResponse<MyUser>.NotAuthorized);
        }

        [HttpPost("createTemporaryUser")]
        public async Task<ApiResponse<MyDataResponse<MyUser>>> CreateTemporaryUser(CancellationToken cancellationToken)
        {
            var currentUser = await _userService.CreateTemporaryUser(cancellationToken);
            var data = currentUser.ToMyDataResponse();
            return ApiResponse<MyDataResponse<MyUser>>.CreateSuccess(data);
        }

        [HttpPost("convertToPermanentUser")]
        [Authorize]
        public async Task<ApiResponse<MyDataResponse<MyUser>>> ConvertToPermanentUser(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var currentUser = await _userService.ConvertToPermanentUser(
                userId,
                registerRequest.UserName,
                registerRequest.Email,
                registerRequest.Password,
                cancellationToken);
            var data = currentUser.ToMyDataResponse();
            return ApiResponse<MyDataResponse<MyUser>>.CreateSuccess(data);
        }
    }
}
