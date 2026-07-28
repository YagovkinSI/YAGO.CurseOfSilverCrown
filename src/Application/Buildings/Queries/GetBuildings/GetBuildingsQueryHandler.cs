using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Colonies.Buildings;
using YAGO.World.Domain.Exceptions;
using static YAGO.World.Application.Buildings.Queries.GetBuildings.GetBuildingsQueryHandler;

namespace YAGO.World.Application.Buildings.Queries.GetBuildings
{
    public class GetBuildingsQueryHandler
        (IColonyRepository colonyRepository)
        : IRequestHandler<GetBuildingsQuery, GetBuildingsResult>
    {
        public async Task<GetBuildingsResult> Handle(GetBuildingsQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            return new GetBuildingsResult(colony.State.Buildings.Values.ToList(), colony.State);
        }

        public record GetBuildingsQuery(long UserId) : IRequest<GetBuildingsResult>;
        public record GetBuildingsResult(IReadOnlyList<ColonyBuilding> Buildings, ColonyState ColonyState);
    }
}
