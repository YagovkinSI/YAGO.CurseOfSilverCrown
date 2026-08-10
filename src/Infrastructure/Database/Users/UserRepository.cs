using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Users;

namespace YAGO.World.Infrastructure.Database.Users
{
    internal class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _databaseContext;
        private readonly UserUpdateConfiguration _userUpdateConfiguration = new();

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
            var source = user.ToEntity();
            var target = await _databaseContext.Users.FindAsync(user.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(UserEntity), user.Id.ToString());

            EntityUpdater.Update(source, target, _userUpdateConfiguration);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}