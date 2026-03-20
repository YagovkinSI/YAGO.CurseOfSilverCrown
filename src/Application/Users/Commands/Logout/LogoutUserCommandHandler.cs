using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Identity;
using static YAGO.World.Application.Users.Commands.Logout.LogoutUserCommandHandler;

namespace YAGO.World.Application.Users.Commands.Logout
{
    public class LogoutUserCommandHandler(
        IIdentityManager identityManager)
        : IRequestHandler<LogoutUserCommand, ProcessorResultEmpty>
    {
        public async Task<ProcessorResultEmpty> Handle(LogoutUserCommand command, CancellationToken cancellationToken)
        {
            await identityManager.Logout(cancellationToken);

            return new ProcessorResultEmpty();
        }

        public record LogoutUserCommand() : IRequest<ProcessorResultEmpty>;
    }
}
