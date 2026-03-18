using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Users;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Users;
using LoginRequest = YAGO.World.Host.Controllers.Users.LoginRequest;
using RegisterRequest = YAGO.World.Host.Controllers.Users.RegisterRequest;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/me/user")]
    public class MyUserController : ControllerBase
    {
        private readonly IGetMyUserProcessor _getMyUserProcessor;
        private readonly ILoginUserProcessor _loginUserProcessor;
        private readonly IRegisterUserProcessor _registerUserProcessor;
        private readonly IUserService _userService;

        public MyUserController(
            IGetMyUserProcessor getMyUserProcessor,
            IUserService currentUserService,
            ILoginUserProcessor loginUserProcessor,
            IRegisterUserProcessor registerUserProcessor)
        {
            _getMyUserProcessor = getMyUserProcessor;
            _userService = currentUserService;
            _loginUserProcessor = loginUserProcessor;
            _registerUserProcessor = registerUserProcessor;
        }

        [HttpGet]
        [Route("get")]
        public async Task<ApiResponse<MyUser>> Get(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return ApiResponse<MyUser>.Empty;

            var userId = User.GetUserId();
            var command = new GetMyUserCommand(userId);
            var result = await _getMyUserProcessor.Execute(command, cancellationToken);
            return result.User.ToMyDataResponse();
        }

        [HttpPost]
        [Route("register")]
        public async Task Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(
                registerRequest.UserName,
                registerRequest.Password,
                registerRequest.Email);
            await _registerUserProcessor.Execute(command, cancellationToken);
        }

        [HttpPost]
        [Route("login")]
        public async Task Login(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(
                loginRequest.UserName,
                loginRequest.Password);
            await _loginUserProcessor.Execute(command, cancellationToken);
        }

        [HttpPost]
        [Route("logout")]
        public async Task Logout(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return;

            await _userService.Logout(cancellationToken);
        }

        [HttpPost("createTemporaryUser")]
        public async Task CreateTemporaryUser(CancellationToken cancellationToken)
        {
            await _userService.CreateTemporaryUser(cancellationToken);
        }

        [HttpPost("convertToPermanentUser")]
        [Authorize]
        public async Task ConvertToPermanentUser(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            _ = await _userService.ConvertToPermanentUser(
                userId,
                registerRequest.UserName,
                registerRequest.Email,
                registerRequest.Password,
                cancellationToken);
        }
    }
}
