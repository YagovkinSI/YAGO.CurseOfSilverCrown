using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Domain.Entities.Users;

namespace YAGO.World.Application.Users
{
    public interface ICreateTemporaryUserProcessor : IProcessor<ProcessorCommandEmpty, ProcessorResultEmpty>;

    public class CreateTemporaryUserProcessor(
        IIdentityManager identityManager)
        : ICreateTemporaryUserProcessor
    {
        public async Task<ProcessorResultEmpty> Execute(ProcessorCommandEmpty command, CancellationToken cancellationToken)
        {
            var newUser = User.CreateTemporary();
            await identityManager.CreateTemporaryUser(newUser, cancellationToken);

            await identityManager.Login(newUser.UserName, password: null, cancellationToken);

            return new ProcessorResultEmpty();
        }
    }
}
