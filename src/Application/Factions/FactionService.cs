using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Factions.Enums;

namespace YAGO.World.Application.Factions
{
    public class FactionService
    {
        private readonly IRepositoryFactions _repositoryOrganizations;

        public FactionService(IRepositoryFactions repositoryOrganizations)
        {
            _repositoryOrganizations = repositoryOrganizations;
        }

        public Task<ListData> GetFactionList(int page, FactionOrderBy factionOrderBy) =>
            _repositoryOrganizations.GetFactionList(page, factionOrderBy);
    }
}
