using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.Users.Commands
{
    public class CreateTemporaryUserCommandHandler(
        ILogger<CreateTemporaryUserCommandHandler> logger,
        IIdentityManager identityManager)
        : IRequestHandler<CreateTemporaryUserCommand, Unit>
    {
        public async Task<Unit> Handle(CreateTemporaryUserCommand command, CancellationToken cancellationToken)
        {
            var newUser = User.CreateTemporary();
            await identityManager.CreateTemporaryUser(newUser, cancellationToken);

            await TryLogin(newUser.UserName, cancellationToken);

            return new Unit();
        }

        private async Task TryLogin(string userName, CancellationToken cancellationToken)
        {
            try
            {
                await identityManager.Login(userName, password: null, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Не удалось выполнить авторизацию после успешной регистраиции.");
            }
        }
    }

    public record CreateTemporaryUserCommand() : IRequest<Unit>;
}
