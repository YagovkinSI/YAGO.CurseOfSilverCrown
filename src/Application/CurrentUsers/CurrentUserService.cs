using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.InfrastructureInterfaces;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.CurrentUsers
{
    public class CurrentUserService : ICurrentUserService
    {
        public readonly IIdentityManager _identityManager;
        private readonly IUserRepository _currentUserRepository;

        private readonly TimeSpan timeSpanBetweenUpdateLastActivity = TimeSpan.FromSeconds(30);

        public CurrentUserService(
            IIdentityManager identityManager,
            IUserRepository currentUserRepository)
        {
            _identityManager = identityManager;
            _currentUserRepository = currentUserRepository;
        }

        public async Task<User?> GetCurrentUser(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _identityManager.GetCurrentUser(userClaimsPrincipal, cancellationToken);
            if (currentUser == null)
                return null;

            cancellationToken.ThrowIfCancellationRequested();
            await UpdateLastActivity(currentUser.Id, cancellationToken);
            return currentUser;
        }

        public async Task<User> Register(
            string userName,
            string password,
            string? email,
            CancellationToken cancellationToken)
        {
            await _identityManager.Register(userName, password, email, cancellationToken);

            return await Login(userName, password, cancellationToken);
        }

        public async Task<User> CreateTemporaryUser(CancellationToken cancellationToken)
        {
            var user = await _identityManager.CreateTemporaryUser(cancellationToken);

            return await Login(user.UserName, password: null, cancellationToken);
        }

        public async Task<User> ConvertToPermanentAccount(
            ClaimsPrincipal userClaimsPrincipal,
            string userName,
            string? email,
            string password,
            CancellationToken cancellationToken)
        {
            return await _identityManager.ConvertToPermanentAccount(
                userClaimsPrincipal, 
                userName, 
                password, 
                email, 
                cancellationToken);
        }

        public async Task<User> Login(
            string userName,
            string? password,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _identityManager.Login(userName, password, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _currentUserRepository.FindByName(userName, cancellationToken);
            return currentUser!;
        }

        public async Task Logout(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _identityManager.Logout(cancellationToken);
        }

        public async Task UpdateLastActivity(long userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _currentUserRepository.Find(userId, cancellationToken);
            if (currentUser == null)
                return;

            if (currentUser.LastActivityAtUtc > DateTime.UtcNow - timeSpanBetweenUpdateLastActivity)
                return;

            cancellationToken.ThrowIfCancellationRequested();
            await _currentUserRepository.UpdateLastActivity(userId, cancellationToken);
        }
    }
}