using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Factions.Enums;

namespace YAGO.World.Application.Factions
{
    public class FactionService
    {
        private readonly IRepositoryFactions _repositoryFactions;

        public FactionService(IRepositoryFactions repositoryFactions)
        {
            _repositoryFactions = repositoryFactions;
        }

        public Task<ListData> GetFactionList(int page, FactionOrderBy factionOrderBy) =>
            _repositoryFactions.GetFactionList(page, factionOrderBy);
    }
}
