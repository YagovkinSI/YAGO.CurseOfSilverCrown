using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Infrastructure.Helpers.Commands;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class RepositoryCommads : IRepositoryCommads
    {
        private readonly ApplicationDbContext _context;

        public RepositoryCommads(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CheckAndFix(int organizationId)
        {
            var organizationHasCommans = await _context.Commands.AnyAsync(c => c.DomainId == organizationId);

            if (!organizationHasCommans)
            {
                var domain = await _context.Domains.FindAsync(organizationId);
                CommandCreateForNewTurnHelper.CreateNewCommandsForOrganizations(_context, domain);
            }
        }

        public async Task CheckAndFixForAll()
        {
            var organizations = await _context.Domains.ToListAsync();
            foreach (var organization in organizations)
            {
                await CheckAndFix(organization.Id);
            }
        }
    }
}
