using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.Users
{
    public interface ILoginUserProcessor : IProcessor<LoginUserCommand, ProcessorResultEmpty>;

    public class LoginUserProcessor(
        IIdentityManager identityManager)
        : ILoginUserProcessor
    {
        public async Task<ProcessorResultEmpty> Execute(LoginUserCommand command, CancellationToken cancellationToken)
        {
            await identityManager.Login(command.UserName, command.Password, cancellationToken);

            return new ProcessorResultEmpty();
        }
    }

    public record LoginUserCommand(string UserName, string? Password) : IProcessorCommand;
}
