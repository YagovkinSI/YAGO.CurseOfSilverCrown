using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Users
{
    public interface IConvertToPermanentUserProcessor : IProcessor<ConvertToPermanentUserCommand, ProcessorResultEmpty>;

    public class ConvertToPermanentUserProcessor(
        IUserRepository userRepository,
        IIdentityManager identityManager)
        : IConvertToPermanentUserProcessor
    {
        public async Task<ProcessorResultEmpty> Execute(ConvertToPermanentUserCommand command, CancellationToken cancellationToken)
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

            return new ProcessorResultEmpty();
        }
    }

    public record ConvertToPermanentUserCommand(long UserId, string UserName, string Password, string? Email) : IProcessorCommand;
}
