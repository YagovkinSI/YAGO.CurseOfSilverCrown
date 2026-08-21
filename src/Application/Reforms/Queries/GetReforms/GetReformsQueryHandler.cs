using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Reforms.Queries.GetReforms
{
    public class GetReformsQueryHandler
        (IColonyRepository colonyRepository,
        IReformRepository reformRepository)
        : IRequestHandler<GetReformsQuery, GetReformsResult>
    {
        public async Task<GetReformsResult> Handle(GetReformsQuery command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var reforms = await reformRepository.GetAll(cancellationToken);
            var reformDtos = new List<ReformDto>(reforms.Count);
            var colonyState = colony.State;
            foreach (var reform in reforms)
            {
                var isAvailable = !reform.Action.Requirements.Any(x => !x.Check(colonyState));
                var reformDto = new ReformDto(reform, isAvailable);
                reformDtos.Add(reformDto);
            }
            return new GetReformsResult(reformDtos);
        }
    }
    public record GetReformsQuery(long UserId) : IRequest<GetReformsResult>;
    public record GetReformsResult(IReadOnlyList<ReformDto> ReformDtos);
}
