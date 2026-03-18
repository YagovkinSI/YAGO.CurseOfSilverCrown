using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Entities.Users;

namespace YAGO.World.Application.Users
{
    public interface IRegisterUserProcessor : IProcessor<RegisterUserCommand, ProcessorResultEmpty>;

    public class RegisterUserProcessor(
        IIdentityManager identityManager,
        ILoginUserProcessor loginUserProcessor)
        : IRegisterUserProcessor
    {
        public async Task<ProcessorResultEmpty> Execute(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var newUser = User.CreateNew(command.UserName, command.Email);
            await identityManager.Register(newUser, command.Password, cancellationToken);

            await Login(command.UserName, command.Password, cancellationToken);
            return new ProcessorResultEmpty();
        }

        private async Task Login(string userName, string password, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(userName, password);
            _ = await loginUserProcessor.Execute(command, cancellationToken);
        }
    }

    public record RegisterUserCommand(string UserName, string Password, string? Email) : IProcessorCommand;
}
