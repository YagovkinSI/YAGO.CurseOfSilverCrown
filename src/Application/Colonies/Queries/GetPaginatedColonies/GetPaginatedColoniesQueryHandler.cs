using MediatR;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies.Queries.GetPaginatedColonies
{
    public class GetPaginatedColoniesQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetPaginatedColoniesQuery, GetPaginatedColoniesResult>
    {
        public async Task<GetPaginatedColoniesResult> Handle(GetPaginatedColoniesQuery command, CancellationToken cancellationToken)
        {
            var coloniesPaginated = await colonyRepository.GetPaginatedColonies(
                command.Page, 
                PaginatedConstants.ItemsInPage, 
                cancellationToken);
            return new GetPaginatedColoniesResult(coloniesPaginated);
        }
    }

    public record GetPaginatedColoniesQuery(int Page) : IRequest<GetPaginatedColoniesResult>;
    public record GetPaginatedColoniesResult(PaginatedData<Colony> ColoniesPaginated);
}
