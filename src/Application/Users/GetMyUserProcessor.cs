using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Entities.Users;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Users
{
    public interface IGetMyUserProcessor : IProcessor<GetMyUserCommand, GetMyUserResult>;

    public class GetMyUserProcessor(
        IUserRepository userRepository)
        : IGetMyUserProcessor
    {
        public async Task<GetMyUserResult> Execute(GetMyUserCommand command, CancellationToken cancellationToken)
        {
            var currentUser = await userRepository.Find(command.UserId, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(User), command.UserId);
            return new GetMyUserResult(currentUser);
        }
    }

    public record GetMyUserCommand(long UserId) : IProcessorCommand;
    public record GetMyUserResult(User User) : IProcessorResult;
}
