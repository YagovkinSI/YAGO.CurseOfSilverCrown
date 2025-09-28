using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Users;
using YAGO.World.Infrastructure.Database.Models.Users;
using YAGO.World.Infrastructure.Database.Models.Users.Mappings;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public UserRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<User?> Find(long userId, CancellationToken cancellationToken)
        {
            var userEntity = await _databaseContext.Users
                .FindAsync([userId], cancellationToken);
            return userEntity?.ToDomain();
        }

        public async Task<User?> FindByName(string userName, CancellationToken cancellationToken)
        {
            var userEntity = await _databaseContext.Users
                .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
            return userEntity?.ToDomain();
        }

        public async Task UpdateLastActivity(long userId, CancellationToken cancellationToken)
        {
            var userEntity = await _databaseContext.Users
                .FindAsync([userId], cancellationToken)
                ?? throw new YagoNotFoundException(nameof(UserEntity), userId);

            userEntity.UpdateLastActivity();
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}