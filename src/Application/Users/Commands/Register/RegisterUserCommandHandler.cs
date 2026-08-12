using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.Users.Commands.Register
{
    public class RegisterUserCommandHandler(
        ILogger<RegisterUserCommandHandler> logger,
        IIdentityManager identityManager,
        IUserRepository userRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<RegisterUserCommand, Unit>
    {
        public async Task<Unit> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            if (await IsUserNameExist(command, cancellationToken))
                throw new YagoException("Пользователь с таким именем уже существует.");

            var newUser = User.CreateNew(command.UserName, command.Email);
            //TODO: Риск! Если регистрация пройдёт, а создание колонии нет, то пользователь останется без колонии.
            await identityManager.Register(newUser, command.Password, cancellationToken);

            var user = await userRepository.FindByName(newUser.UserName, cancellationToken)
                ?? throw new YagoException("Не удалось выполнить регистрацию.");
            var entities = Colony.CreateNew(user.Id);
            await unitOfWorkRepository.SaveInTransactionAsync(entities, cancellationToken);

            await TryLogin(command, cancellationToken);

            return new Unit();
        }

        private async Task TryLogin(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                await identityManager.Login(command.UserName, command.Password, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Не удалось выполнить авторизацию после успешной регистраиции.");
            }
        }

        private async Task<bool> IsUserNameExist(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindByName(command.UserName, cancellationToken);
            return user != null;
        }
    }

    public record RegisterUserCommand(string UserName, string Password, string? Email) : IRequest<Unit>;
}
