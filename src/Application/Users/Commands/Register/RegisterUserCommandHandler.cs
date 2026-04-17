using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Handlers;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Users;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Users.Commands.Register
{
    public class RegisterUserCommandHandler(
        IIdentityManager identityManager,
        IUserRepository userRepository,
        IColonyRepository colonyRepository)
        : IRequestHandler<RegisterUserCommand, HandlerResultEmpty>
    {
        public async Task<HandlerResultEmpty> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var newUser = User.CreateNew(command.UserName, command.Email);
            await identityManager.Register(newUser, command.Password, cancellationToken);

            var user = await userRepository.FindByName(newUser.UserName, cancellationToken)
                ?? throw new YagoException("Не удалось создать временного пользователя");
            var colony = Colony.CreateNew(user.Id);
            await colonyRepository.Add(colony, cancellationToken);

            await identityManager.Login(command.UserName, command.Password, cancellationToken);

            return new HandlerResultEmpty();
        }
    }

    public record RegisterUserCommand(string UserName, string Password, string? Email) : IRequest<HandlerResultEmpty>;
}
