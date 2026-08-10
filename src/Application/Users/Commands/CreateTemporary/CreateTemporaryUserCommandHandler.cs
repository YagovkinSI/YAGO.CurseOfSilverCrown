using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Handlers;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.Users.Commands.CreateTemporary
{
    public class CreateTemporaryUserCommandHandler(
        IIdentityManager identityManager,
        IUserRepository userRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<CreateTemporaryUserCommand, HandlerResultEmpty>
    {
        public async Task<HandlerResultEmpty> Handle(CreateTemporaryUserCommand command, CancellationToken cancellationToken)
        {
            var newUser = User.CreateTemporary();
            await identityManager.CreateTemporaryUser(newUser, cancellationToken);

            var user = await userRepository.FindByName(newUser.UserName, cancellationToken)
                ?? throw new YagoException("Не удалось создать временного пользователя");
            var entities = Colony.CreateNew(user.Id);
            await unitOfWorkRepository.SaveInTransactionAsync(entities, cancellationToken);

            await identityManager.Login(newUser.UserName, password: null, cancellationToken);

            return new HandlerResultEmpty();
        }
    }

    public record CreateTemporaryUserCommand() : IRequest<HandlerResultEmpty>;
}
