using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class RepositoryForUpdateData : IRepositoryForUpdateData
    {
        private readonly ApplicationDbContext _context;

        public RepositoryForUpdateData(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Update(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
