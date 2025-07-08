using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Provinces;
using YAGO.World.Infrastructure.Database.Models.Domains;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class RepositoryProvince : IRepositoryProvince
    {
        private readonly ApplicationDbContext _context;

        public RepositoryProvince(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProvinceWithUser> GetProvinceWithUser(int id, CancellationToken cancellationToken)
        {
            var domainDb = await _context.Domains
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

            return domainDb.ToProvinceWithUser();
        }
    }
}
