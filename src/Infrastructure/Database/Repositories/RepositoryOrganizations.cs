using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Infrastructure.Database.Models.Domains;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    public class RepositoryOrganizations : IRepositoryOrganizations
    {
        private readonly ApplicationDbContext _context;

        public RepositoryOrganizations(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Domain.Factions.Faction>> GetAll()
        {
            return await _context.Domains
                .Select(d => d.ToDomain())
                .ToListAsync();
        }

        public async Task<Domain.Factions.Faction> Get(int? organizationId)
        {
            var dbOrganization = await _context.Domains.FindAsync(organizationId);
            return dbOrganization?.ToDomain();
        }

        public async Task<Domain.Factions.Faction> GetOrganizationByUser(string userId)
        {
            var dbOrganization = await _context.Domains
                .SingleOrDefaultAsync(o => o.UserId == userId);

            return dbOrganization?.ToDomain();
        }
    }
}
