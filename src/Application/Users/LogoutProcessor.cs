using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Identity;

namespace YAGO.World.Application.Users
{
    public interface ILogoutProcessor : IProcessor<ProcessorCommandEmpty, ProcessorResultEmpty>;

    public class LogoutProcessor(
        IIdentityManager identityManager)
        : ILogoutProcessor
    {
        public async Task<ProcessorResultEmpty> Execute(ProcessorCommandEmpty command, CancellationToken cancellationToken)
        {
            await identityManager.Logout(cancellationToken);

            return new ProcessorResultEmpty();
        }
    }
}
