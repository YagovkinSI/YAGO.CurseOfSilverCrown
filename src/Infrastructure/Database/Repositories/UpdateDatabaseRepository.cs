using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class UpdateDatabaseRepository : IUpdateDatabaseRepository
    {
        private readonly ApplicationDbContext _context;

        public UpdateDatabaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Update(CancellationToken cancellationToken)
        {
            var someChanges = UpdateContext();

            if (someChanges)
                await _context.SaveChangesAsync();
        }

        private bool UpdateContext()
        {
            var someChanges = false;

            //Выполнение работ

            return someChanges;
        }
    }
}
