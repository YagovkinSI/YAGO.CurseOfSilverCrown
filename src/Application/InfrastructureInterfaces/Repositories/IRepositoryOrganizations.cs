using System.Collections.Generic;
using System.Threading.Tasks;
using YAGO.World.Domain.Organizations;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IRepositoryOrganizations
    {
        Task<IReadOnlyCollection<Organization>> GetAll();

        Task<Organization> Get(int? organizationId);

        Task<Organization> GetOrganizationByUser(string userId);
    }
}
