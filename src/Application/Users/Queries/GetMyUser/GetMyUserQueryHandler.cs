using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Users;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Users.Queries.GetMyUser
{
    public class GetMyUserQueryHandler(
        IUserRepository userRepository)
        : IRequestHandler<GetMyUserQuery, GetMyUserResult>
    {
        public async Task<GetMyUserResult> Handle(GetMyUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await userRepository.Find(request.UserId, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(User), request.UserId.ToString());
            return new GetMyUserResult(currentUser);
        }
    }

    public record GetMyUserQuery(long UserId) : IRequest<GetMyUserResult>;
    public record GetMyUserResult(User User);
}
