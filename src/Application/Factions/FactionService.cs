using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Factions.Enums;

namespace YAGO.World.Application.Factions
{
    public class FactionService
    {
        private readonly IRepositoryOrganizations _repositoryOrganizations;

        public FactionService(IRepositoryOrganizations repositoryOrganizations)
        {
            _repositoryOrganizations = repositoryOrganizations;
        }

        public Task<ListItem[]> GetFactionList(int page, int pageSize, FactionOrderBy factionOrderBy, bool useOrderByDescending) =>
            _repositoryOrganizations.GetFactionList(page, pageSize, factionOrderBy, useOrderByDescending);
    }
}
