using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Exceptions;
using static YAGO.World.Application.Decrees.Queries.GetDecrees.GetDecreeQueryHandler;

namespace YAGO.World.Application.Decrees.Queries.GetDecrees
{
    public class GetDecreeQueryHandler : IRequestHandler<GetDecreeQuery, GetDecreeResult>
    {
        public Task<GetDecreeResult> Handle(GetDecreeQuery command, CancellationToken cancellationToken)
        {
            var result = DecreeDataset.Get()
                .FirstOrDefault(x => x.Id == command.DecreeId)
                ?? throw new YagoNotFoundException(nameof(Decree), command.DecreeId);
            return Task.FromResult(new GetDecreeResult(result));
        }

        public record GetDecreeQuery(long DecreeId) : IRequest<GetDecreeResult>;
        public record GetDecreeResult(Decree Decree);
    }
}
