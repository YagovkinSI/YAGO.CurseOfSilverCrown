using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Exceptions;
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

        public async Task<Domain.Users.User?> Find(long userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _databaseContext.Users.FindAsync(new object[] { userId }, cancellationToken);
            return user?.ToDomain();
        }

        public async Task<Domain.Users.User?> FindByName(string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userInDb = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
            return userInDb?.ToDomain();
        }

        public async Task UpdateLastActivity(long userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _databaseContext.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user == null)
                throw new YagoException("Пользователь не найден в базе данных");

            cancellationToken.ThrowIfCancellationRequested();
            user.UpdateLastActivity();
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}