using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.Users.Commands.Register
{
    public class RegisterUserCommandHandler(
        ILogger<RegisterUserCommandHandler> logger,
        IIdentityManager identityManager,
        IUserRepository userRepository)
        : IRequestHandler<RegisterUserCommand, Unit>
    {
        public async Task<Unit> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            if (await IsUserNameExist(command.UserName, cancellationToken))
                throw new YagoException("Пользователь с таким именем уже существует.");

            var newUser = User.CreateNew(command.UserName, command.Email);
            await identityManager.Register(newUser, command.Password, cancellationToken);

            await TryLogin(command.UserName, command.Password, cancellationToken);

            return new Unit();
        }

        private async Task<bool> IsUserNameExist(string userName, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindByName(userName, cancellationToken);
            return user != null;
        }

        private async Task TryLogin(string userName, string password, CancellationToken cancellationToken)
        {
            try
            {
                await identityManager.Login(userName, password, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Не удалось выполнить авторизацию после успешной регистраиции.");
            }
        }
    }

    public record RegisterUserCommand(string UserName, string Password, string? Email) : IRequest<Unit>;
}
