using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class RepositoryTurns : IRepositoryTurns
    {
        private readonly ApplicationDbContext _context;

        public RepositoryTurns(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCurrentTurnId()
        {
            var turn = await _context.Turns
                .SingleAsync(t => t.IsActive);
            return turn.Id;
        }
    }
}
