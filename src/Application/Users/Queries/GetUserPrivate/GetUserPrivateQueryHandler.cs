using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.Users.Queries.GetUserPrivate
{
    public class GetUserPrivateQueryHandler(
        IUserRepository userRepository)
        : IRequestHandler<GetUserPrivateQuery, GetUserPrivateResult>
    {
        public async Task<GetUserPrivateResult> Handle(GetUserPrivateQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.Find(request.UserId, cancellationToken);
            return new GetUserPrivateResult(user);
        }
    }

    public record GetUserPrivateQuery(long UserId) : IRequest<GetUserPrivateResult>;
    public record GetUserPrivateResult(User? User);
}
