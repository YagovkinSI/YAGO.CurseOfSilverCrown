using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Identity;

namespace YAGO.World.Application.Users.Commands.Login
{
    public class LoginUserHandler(
        IIdentityManager identityManager)
        : IRequestHandler<LoginUserCommand, ProcessorResultEmpty>
    {
        public async Task<ProcessorResultEmpty> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            await identityManager.Login(command.UserName, command.Password, cancellationToken);

            return new ProcessorResultEmpty();
        }
    }

    public record LoginUserCommand(string UserName, string? Password) : IRequest<ProcessorResultEmpty>;
}
