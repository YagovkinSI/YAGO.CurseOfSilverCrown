using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Users.Commands.ConvertToPermanent;
using YAGO.World.Application.Users.Commands.CreateTemporary;
using YAGO.World.Application.Users.Commands.Login;
using YAGO.World.Application.Users.Commands.Logout;
using YAGO.World.Application.Users.Commands.Register;
using YAGO.World.Application.Users.Queries.GetMyUser;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Users.Models;
using static YAGO.World.Application.Users.Commands.Logout.LogoutUserCommandHandler;
using LoginRequest = YAGO.World.Host.Controllers.Users.Models.LoginRequest;
using RegisterRequest = YAGO.World.Host.Controllers.Users.Models.RegisterRequest;

namespace YAGO.World.Host.Controllers.Users
{
    [ApiController]
    [Route("api/me/user")]
    public class MyUserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MyUserController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getMyUser")]
        public async Task<ApiResponse<MyUser>> GetMyUser(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return ApiResponse<MyUser>.Empty;

            var userId = User.GetUserId();
            var command = new GetMyUserQuery(userId);
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
            await _mediator.Send(command, cancellationToken);
        }

        [HttpPost("login")]
        public async Task Login(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(
                loginRequest.UserName,
                loginRequest.Password);
            await _mediator.Send(command, cancellationToken);
        }

        [HttpPost("logout")]
        public async Task Logout(CancellationToken cancellationToken)
        {
            if (!User.IsAuthenticated())
                return;

            var command = new LogoutUserCommand();
            await _mediator.Send(command, cancellationToken);
        }

        [HttpPost("createTemporaryUser")]
        public async Task CreateTemporaryUser(CancellationToken cancellationToken)
        {
            var command = new CreateTemporaryUserCommand();
            await _mediator.Send(command, cancellationToken);
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
            await _mediator.Send(command, cancellationToken);
        }
    }
}
