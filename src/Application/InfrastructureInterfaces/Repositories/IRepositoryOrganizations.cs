using System.Collections.Generic;
using System.Threading.Tasks;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IRepositoryOrganizations
    {
        Task<IReadOnlyCollection<Domain.Factions.Faction>> GetAll();

        Task<Domain.Factions.Faction> Get(int? organizationId);

        Task<Domain.Factions.Faction> GetOrganizationByUser(string userId);
    }
}
