using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Handlers;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Domain.Entities.Users;

namespace YAGO.World.Application.Users.Commands.CreateTemporary
{
    public class CreateTemporaryUserCommandHandler(
        IIdentityManager identityManager)
        : IRequestHandler<CreateTemporaryUserCommand, HandlerResultEmpty>
    {
        public async Task<HandlerResultEmpty> Handle(CreateTemporaryUserCommand command, CancellationToken cancellationToken)
        {
            var newUser = User.CreateTemporary();
            await identityManager.CreateTemporaryUser(newUser, cancellationToken);

            await identityManager.Login(newUser.UserName, password: null, cancellationToken);

            return new HandlerResultEmpty();
        }
    }

    public record CreateTemporaryUserCommand() : IRequest<HandlerResultEmpty>;
}
