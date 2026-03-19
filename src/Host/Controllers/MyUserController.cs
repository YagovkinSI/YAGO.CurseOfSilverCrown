using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Users;
using YAGO.World.Application.Users.GetMyUser;
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
        private readonly IMediator _mediator;
        private readonly ILoginUserProcessor _loginUserProcessor;
        private readonly IRegisterUserProcessor _registerUserProcessor;
        private readonly ICreateTemporaryUserProcessor _createTemporaryUserProcessor;
        private readonly IConvertToPermanentUserProcessor _convertToPermanentUserProcessor;
        private readonly ILogoutProcessor _logoutProcessor;

        public MyUserController(
            IMediator mediator,
            ILoginUserProcessor loginUserProcessor,
            IRegisterUserProcessor registerUserProcessor,
            ICreateTemporaryUserProcessor createTemporaryUserProcessor,
            IConvertToPermanentUserProcessor convertToPermanentUserProcessor,
            ILogoutProcessor logoutProcessor)
        {
            _mediator = mediator;
            _loginUserProcessor = loginUserProcessor;
            _registerUserProcessor = registerUserProcessor;
            _createTemporaryUserProcessor = createTemporaryUserProcessor;
            _convertToPermanentUserProcessor = convertToPermanentUserProcessor;
            _logoutProcessor = logoutProcessor;
        }

        [HttpGet("getMyUser")]
        public async Task<ApiResponse<MyUser>> GetMyUser(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return ApiResponse<MyUser>.Empty;

            var userId = User.GetUserId();
            var command = new GetMyUserCommand(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.User.ToMyDataResponse();
        }

        [HttpPost("register")]
        public async Task Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(
                registerRequest.UserName,
                registerRequest.Password,
                registerRequest.Email);
            await _registerUserProcessor.Execute(command, cancellationToken);
        }

        [HttpPost("login")]
        public async Task Login(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(
                loginRequest.UserName,
                loginRequest.Password);
            await _loginUserProcessor.Execute(command, cancellationToken);
        }

        [HttpPost("logout")]
        public async Task Logout(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return;

            var command = new ProcessorCommandEmpty();
            await _logoutProcessor.Execute(command, cancellationToken);
        }

        [HttpPost("createTemporaryUser")]
        public async Task CreateTemporaryUser(CancellationToken cancellationToken)
        {
            var command = new ProcessorCommandEmpty();
            await _createTemporaryUserProcessor.Execute(command, cancellationToken);
        }

        [HttpPost("convertToPermanentUser")]
        [Authorize]
        public async Task ConvertToPermanentUser(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new ConvertToPermanentUserCommand(
                userId,
                registerRequest.UserName,
                registerRequest.Password,
                registerRequest.Email);
            await _convertToPermanentUserProcessor.Execute(command, cancellationToken);
        }
    }
}
