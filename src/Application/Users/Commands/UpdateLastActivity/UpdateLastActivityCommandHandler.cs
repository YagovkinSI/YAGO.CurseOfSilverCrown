using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Handlers;
using YAGO.World.Application.Interfaces.Repository;

namespace YAGO.World.Application.Users.Commands.UpdateLastActivity
{
    public class UpdateLastActivityCommandHandler(
        IUserRepository userRepository)
        : IRequestHandler<UpdateLastActivityCommand, HandlerResultEmpty>
    {
        public async Task<HandlerResultEmpty> Handle(UpdateLastActivityCommand command, CancellationToken cancellationToken)
        {
            var currentUser = await userRepository.Find(command.UserId, cancellationToken);
            if (currentUser == null)
                return new HandlerResultEmpty();

            var success = currentUser.TryUpdateLastActivityIfNeeded();
            if (success)
                await userRepository.Update(currentUser, cancellationToken);

            return new HandlerResultEmpty();
        }
    }

    public record UpdateLastActivityCommand(long UserId) : IRequest<HandlerResultEmpty>;
}
