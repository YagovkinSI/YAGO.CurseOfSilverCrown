using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Domain.Entities.Users;

namespace YAGO.World.Application.Users
{
    public interface IRegisterUserProcessor : IProcessor<RegisterUserCommand, ProcessorResultEmpty>;

    public class RegisterUserProcessor(
        IIdentityManager identityManager)
        : IRegisterUserProcessor
    {
        public async Task<ProcessorResultEmpty> Execute(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var newUser = User.CreateNew(command.UserName, command.Email);
            await identityManager.Register(newUser, command.Password, cancellationToken);

            await identityManager.Login(command.UserName, command.Password, cancellationToken);

            return new ProcessorResultEmpty();
        }
    }

    public record RegisterUserCommand(string UserName, string Password, string? Email) : IProcessorCommand;
}
