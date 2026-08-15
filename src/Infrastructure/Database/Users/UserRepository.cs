using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies.Resources;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Users;

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

        public async Task UpdateLastActivity(User user, CancellationToken cancellationToken)
        {
            var source = user.ToEntity();
            var target = await _databaseContext.Users.FindAsync(user.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(UserEntity), user.Id.ToString());

            target.UpdateLastActivity(source.LastActivityAtUtc);
            _databaseContext.Update(target);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}