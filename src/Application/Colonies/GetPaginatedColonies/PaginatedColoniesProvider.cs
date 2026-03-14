using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Application.Colonies.GetPaginatedColonies
{
    public class PaginatedColoniesProvider : IPaginatedColoniesProvider
    {
        private readonly IColonyRepository _colonyRepository;

        public PaginatedColoniesProvider(IColonyRepository colonyRepository)
        {
            _colonyRepository = colonyRepository;
        }

        public async Task<PaginatedData<Colony>> Get(GetPaginatedColoniesCommand command, CancellationToken cancellationToken)
        {
            var page = command.Page;
            return await _colonyRepository.GetPaginatedColonies(page, cancellationToken);
        }
    }
}
