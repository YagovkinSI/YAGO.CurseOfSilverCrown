using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Identity;

namespace YAGO.World.Application.Users.Commands.Logout
{
    public class LogoutUserCommandHandler(
        IIdentityManager identityManager)
        : IRequestHandler<LogoutUserCommand, Unit>
    {
        public async Task<Unit> Handle(LogoutUserCommand command, CancellationToken cancellationToken)
        {
            await identityManager.Logout(cancellationToken);

            return new Unit();
        }
    }

    public record LogoutUserCommand() : IRequest<Unit>;
}
