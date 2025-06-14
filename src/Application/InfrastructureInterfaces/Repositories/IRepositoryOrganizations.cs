using System.Collections.Generic;
using System.Threading.Tasks;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Factions.Enums;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IRepositoryOrganizations
    {
        Task<IReadOnlyCollection<Domain.Factions.Faction>> GetAll();
        Task<ListItem[]> GetFactionList(int page, int pageSize, FactionOrderBy factionOrderBy, bool useOrderByDescending);

        Task<Domain.Factions.Faction> Get(int? organizationId);

        Task<Domain.Factions.Faction> GetOrganizationByUser(string userId);
    }
}
