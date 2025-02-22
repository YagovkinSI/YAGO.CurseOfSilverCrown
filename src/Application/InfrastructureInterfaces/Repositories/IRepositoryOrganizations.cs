using System.Collections.Generic;
using YAGO.World.Domain.Organizations;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IRepositoryOrganizations
    {
        public IReadOnlyCollection<Organization> GetAll();
    }
}
