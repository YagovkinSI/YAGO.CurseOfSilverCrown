using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Users;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Users.GetMyUser
{
    public class GetMyUserProcessor(
        IUserRepository userRepository)
        : IRequestHandler<GetMyUserCommand, GetMyUserResult>
    {
        public async Task<GetMyUserResult> Handle(GetMyUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await userRepository.Find(request.UserId, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(User), request.UserId);
            return new GetMyUserResult(currentUser);
        }
    }

    public record GetMyUserCommand(long UserId) : IRequest<GetMyUserResult>;
    public record GetMyUserResult(User User);
}
