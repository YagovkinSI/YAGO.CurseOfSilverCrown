using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Colonies.Companies;
using YAGO.World.Domain.Colonies.Ships;
using YAGO.World.Domain.Decrees;

namespace YAGO.World.Application.Colonies.GetPaginatedColonies
{
    public class PaginatedColoniesProvider : IPaginatedColoniesProvider
    {
        private readonly IColonyRepository _colonyRepository;

        public PaginatedColoniesProvider(IColonyRepository colonyRepository)
        {
            _colonyRepository = colonyRepository;
        }

        public async Task<PaginatedData<ColonyWithDetails>> Get(GetPaginatedColoniesCommand command, CancellationToken cancellationToken)
        {
            var page = command.Page;
            var colonies = await _colonyRepository.GetPaginatedColonies(page, cancellationToken);
            var list = new List<ColonyWithDetails>(colonies.Data.Count);
            foreach (var colony in colonies.Data)
            {
                var policies = colony.Policies;
                var ship = ShipDataset.GetShip(policies.ShipId);
                var colonyStats = colony.Stats;
                var companies = CompanyDataset.GetCompanies(colonyStats.CompanyIds);
                var colonyWithDetails = new ColonyWithDetails(colony, ship, companies);
                list.Add(colonyWithDetails);
            }

            return new PaginatedData<ColonyWithDetails>(
                list,
                colonies.Total,
                colonies.Page,
                colonies.Limit);
        }
    }
}
