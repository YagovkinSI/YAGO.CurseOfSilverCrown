using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Users.Commands.ConvertToPermanent
{
    public class ConvertToPermanentUserCommandHandler(
        IUserRepository userRepository,
        IIdentityManager identityManager)
        : IRequestHandler<ConvertToPermanentUserCommand, Unit>
    {
        public async Task<Unit> Handle(ConvertToPermanentUserCommand command, CancellationToken cancellationToken)
        {
            var isUserNameTaken = await userRepository.FindByName(command.UserName, cancellationToken) != null;
            if (isUserNameTaken)
                throw new YagoException("Имя пользователя уже занято");

            var currentUser = await userRepository.Find(command.UserId, cancellationToken)
                ?? throw new YagoNotAuthorizedException();

            currentUser.ConvertToPermanentAccount(command.UserName, command.Email);

            await identityManager.ConvertToPermanentAccount(
                currentUser,
                command.Password,
                cancellationToken);

            return new Unit();
        }
    }

    public record ConvertToPermanentUserCommand(long UserId, string UserName, string Password, string? Email) : IRequest<Unit>;
}
