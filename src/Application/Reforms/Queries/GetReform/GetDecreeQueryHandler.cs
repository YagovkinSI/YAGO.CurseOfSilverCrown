using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Reforms.Queries.GetReform
{
    public class GetReformQueryHandler
        (IColonyRepository colonyRepository)
        : IRequestHandler<GetReformQuery, GetReformResult>
    {
        public async Task<GetReformResult> Handle(GetReformQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var colonyState = colony.State;
            var reform = colonyState.GetReform(command.ReformId);
            var isAvailable = !reform.Requirements.Any(x => !x.Check(colonyState));
            var reformDto = new ReformDto(reform.Id, isAvailable);
            return new GetReformResult(reformDto, colonyState);
        }
    }
    public record GetReformQuery(long UserId, long ReformId) : IRequest<GetReformResult>;
    public record GetReformResult(ReformDto ReformDto, ColonyState ColonyState);
}
