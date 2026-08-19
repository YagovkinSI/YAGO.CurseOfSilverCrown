using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Reforms.Queries.GetReform
{
    public class GetReformQueryHandler
        (IColonyRepository colonyRepository,
        IReformRepository reformRepository)
        : IRequestHandler<GetReformQuery, GetReformResult>
    {
        public async Task<GetReformResult> Handle(GetReformQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var reform = await reformRepository.Get(command.ReformCode, cancellationToken);
            var colonyState = colony.State;
            var isAvailable = !reform.Requirements.Any(x => !x.Check(colonyState));
            var reformDto = new ReformDto(reform, isAvailable);
            return new GetReformResult(reformDto, colonyState);
        }
    }
    public record GetReformQuery(long UserId, string ReformCode) : IRequest<GetReformResult>;
    public record GetReformResult(ReformDto ReformDto, ColonyState ColonyState);
}
