using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.CurrentUsers;
using YAGO.World.Infrastructure.Database.Models.Users;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class CurrentUserRepository : ICurrentUserRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public CurrentUserRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<CurrentUser?> FindAsync(long userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _databaseContext.Users.FindAsync(new object[] { userId }, cancellationToken);
            return user?.ToDomainCurrentUser();
        }

        public async Task<CurrentUserWithStoryNode> FindCurrentUserWithStoryNode(long userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _databaseContext.Users
                .Include(u => u.StoryDatas)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            return user?.ToCurrentUserWithStoryNode();
        }

        public async Task<CurrentUser?> FindByUserName(string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userInDb = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
            return userInDb?.ToDomainCurrentUser();
        }

        public async Task<CurrentUserWithStoryNode> FindCurrentUserWithStoryNodeByUserName(string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _databaseContext.Users
            .Include(u => u.StoryDatas)
                .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);

            return user?.ToCurrentUserWithStoryNode();
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