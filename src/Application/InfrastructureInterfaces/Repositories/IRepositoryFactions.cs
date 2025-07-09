using System.Collections.Generic;
using System.Threading.Tasks;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Factions.Enums;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IRepositoryFactions
    {
        Task<IReadOnlyCollection<Domain.Factions.Faction>> GetAll();
        Task<ListData> GetFactionList(int page, FactionOrderBy factionOrderBy);

        Task<Domain.Factions.Faction> Get(int? organizationId);

        Task<Domain.Factions.Faction> GetOrganizationByUser(string userId);
    }
}
