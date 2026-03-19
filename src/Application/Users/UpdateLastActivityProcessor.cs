using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Application.Interfaces.Repository;

namespace YAGO.World.Application.Users
{
    public interface IUpdateLastActivityProcessor : IProcessor<UpdateLastActivityCommand, ProcessorResultEmpty>;

    public class UpdateLastActivityProcessor(
        IUserRepository userRepository)
        : IUpdateLastActivityProcessor
    {
        public async Task<ProcessorResultEmpty> Execute(UpdateLastActivityCommand command, CancellationToken cancellationToken)
        {
            var currentUser = await userRepository.Find(command.UserId, cancellationToken);
            if (currentUser == null)
                return new ProcessorResultEmpty();

            var success = currentUser.TryUpdateLastActivityIfNeeded();
            if (success)
                await userRepository.Update(currentUser, cancellationToken);

            return new ProcessorResultEmpty();
        }
    }

    public record UpdateLastActivityCommand(long UserId) : IProcessorCommand;
}
