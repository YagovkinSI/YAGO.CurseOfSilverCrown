using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Organizations;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    public class RepositoryOrganizations : IRepositoryOrganizations
    {
        private readonly ApplicationDbContext _context;

        public RepositoryOrganizations(ApplicationDbContext context)
        {
            _context = context;
        }

        public IReadOnlyCollection<Organization> GetAll()
        {
            return _context.Domains
                .Select(d => new Organization(d.Id))
                .ToList();
        }
    }
}
