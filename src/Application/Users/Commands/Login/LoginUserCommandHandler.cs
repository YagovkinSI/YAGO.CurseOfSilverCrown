using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Identity;

namespace YAGO.World.Application.Users.Commands.Login
{
    public class LoginUserCommandHandler(
        IIdentityManager identityManager)
        : IRequestHandler<LoginUserCommand, Unit>
    {
        public async Task<Unit> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            await identityManager.Login(command.UserName, command.Password, cancellationToken);

            return new Unit();
        }
    }

    public record LoginUserCommand(string UserName, string? Password) : IRequest<Unit>;
}
