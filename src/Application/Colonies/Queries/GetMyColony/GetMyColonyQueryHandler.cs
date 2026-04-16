using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Services;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies.Queries.GetMyColony
{
    public class GetMyColonyQueryHandler(
        ICurrentColonyProvider currentColonyProvider)
        : IRequestHandler<GetMyColonyQuery, GetMyColonyResult>
    {
        public async Task<GetMyColonyResult> Handle(GetMyColonyQuery command, CancellationToken cancellationToken)
        {
            var colony = await currentColonyProvider.Get(command.UserId, cancellationToken);

            return new GetMyColonyResult(colony);
        }
    }

    public record GetMyColonyQuery(long UserId) : IRequest<GetMyColonyResult>;
    public record GetMyColonyResult(Colony? Colony);
}
