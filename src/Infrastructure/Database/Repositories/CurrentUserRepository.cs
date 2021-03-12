using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.CurrentUsers;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class CurrentUserRepository : ICurrentUserRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public CurrentUserRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Domain.CurrentUsers.User?> Find(long userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _databaseContext.Users.FindAsync(new object[] { userId }, cancellationToken);
            return user?.ToDomainCurrentUser();
        }

        public async Task<Domain.CurrentUsers.User?> FindByUserName(string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userInDb = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
            return userInDb?.ToDomainCurrentUser();
        }

        public async Task UpdateLastActivity(long userId, DateTime lastActivity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _databaseContext.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user == null)
                throw new YagoException("Пользователь не найден в базе данных");

            cancellationToken.ThrowIfCancellationRequested();
            user.LastActivityTime = lastActivity;
            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }
    }
}