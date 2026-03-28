using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies.Queries.GetPaginatedColonies
{
    public class GetPaginatedColoniesCommandHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetPaginatedColoniesCommand, GetPaginatedColoniesResult>
    {
        public async Task<GetPaginatedColoniesResult> Handle(GetPaginatedColoniesCommand command, CancellationToken cancellationToken)
        {
            var coloniesPaginated = await colonyRepository.GetPaginatedColonies(
                command.Page, 
                PaginatedConstants.ItemsInPage, 
                cancellationToken);
            return new GetPaginatedColoniesResult(coloniesPaginated);
        }
    }

    public record GetPaginatedColoniesCommand(int Page) : IRequest<GetPaginatedColoniesResult>;
    public record GetPaginatedColoniesResult(PaginatedData<Colony> ColoniesPaginated);
}
