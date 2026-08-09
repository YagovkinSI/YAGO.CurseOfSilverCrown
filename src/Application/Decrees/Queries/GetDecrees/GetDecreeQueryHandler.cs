using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Decrees.Queries.GetDecrees
{
    public class GetDecreeQueryHandler
        (IColonyRepository colonyRepository)
        : IRequestHandler<GetDecreeQuery, GetDecreeResult>
    {
        public async Task<GetDecreeResult> Handle(GetDecreeQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var colonyState = colony.State;
            var reform = colonyState.GetReform(command.DecreeId);
            var isAvailable = !reform.Requirements.Any(x => !x.Check(colonyState));
            var reformDto = new ReformDto(reform.Id, isAvailable);
            return new GetDecreeResult(reformDto, colonyState);
        }
    }
    public record GetDecreeQuery(long UserId, long DecreeId) : IRequest<GetDecreeResult>;
    public record GetDecreeResult(ReformDto ReformDto, ColonyState ColonyState);
}
