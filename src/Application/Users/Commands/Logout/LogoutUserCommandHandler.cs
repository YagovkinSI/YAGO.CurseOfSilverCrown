using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Handlers;
using YAGO.World.Application.Interfaces.Identity;
using static YAGO.World.Application.Users.Commands.Logout.LogoutUserCommandHandler;

namespace YAGO.World.Application.Users.Commands.Logout
{
    public class LogoutUserCommandHandler(
        IIdentityManager identityManager)
        : IRequestHandler<LogoutUserCommand, HandlerResultEmpty>
    {
        public async Task<HandlerResultEmpty> Handle(LogoutUserCommand command, CancellationToken cancellationToken)
        {
            await identityManager.Logout(cancellationToken);

            return new HandlerResultEmpty();
        }

        public record LogoutUserCommand() : IRequest<HandlerResultEmpty>;
    }
}
