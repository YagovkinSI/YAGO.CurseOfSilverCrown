using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Domain.Entities.Users;

namespace YAGO.World.Application.Users.Commands.Register
{
    public class RegisterUserHandler(
        IIdentityManager identityManager)
        : IRequestHandler<RegisterUserCommand, ProcessorResultEmpty>
    {
        public async Task<ProcessorResultEmpty> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var newUser = User.CreateNew(command.UserName, command.Email);
            await identityManager.Register(newUser, command.Password, cancellationToken);

            await identityManager.Login(command.UserName, command.Password, cancellationToken);

            return new ProcessorResultEmpty();
        }
    }

    public record RegisterUserCommand(string UserName, string Password, string? Email) : IRequest<ProcessorResultEmpty>;
}
