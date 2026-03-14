using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Users;
using YAGO.World.Domain.Entities.Users;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Infrastructure.Database.Users
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

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            var userEntity = await _databaseContext.Users.FindAsync(user.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(UserEntity), user.Id);

            userEntity.UpdateFromDomain(user);

            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}