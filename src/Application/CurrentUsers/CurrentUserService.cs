using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using YAGO.World.Domain.CurrentUsers;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.InfrastructureInterfaces;

namespace YAGO.World.Application.CurrentUsers
{
    public class CurrentUserService : ICurrentUserService
    {
        public readonly IIdentityManager _identityManager;
        private readonly ICurrentUserRepository _currentUserRepository;

        private readonly TimeSpan timeSpanBetweenUpdateLastActivity = TimeSpan.FromSeconds(30);

        public CurrentUserService(
            IIdentityManager identityManager,
            ICurrentUserRepository currentUserRepository)
        {
            _identityManager = identityManager;
            _currentUserRepository = currentUserRepository;
        }

        public async Task<AuthorizationData> GetAuthorizationData(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _identityManager.GetCurrentUser(userClaimsPrincipal, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            if (currentUser != null)
                await UpdateLastActivity(currentUser.Id, cancellationToken);

            return new AuthorizationData(currentUser);
        }

        public async Task<AuthorizationData> Register(
            string userName,
            string email,
            string password,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var newUser = new CurrentUser(default, userName, email, DateTime.UtcNow, DateTime.UtcNow);
            await _identityManager.Register(newUser, password, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            return await Login(userName, password, cancellationToken);
        }

        public async Task<AuthorizationData> Login(
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _identityManager.Login(userName, password, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _currentUserRepository.FindByUserName(userName, cancellationToken);
            return new AuthorizationData(currentUser);
        }

        public async Task Logout(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _identityManager.Logout(cancellationToken);
        }

        public async Task UpdateLastActivity(long userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _currentUserRepository.FindAsync(userId, cancellationToken);
            if (currentUser == null)
                return;

            if (currentUser.LastActivity > DateTime.UtcNow - timeSpanBetweenUpdateLastActivity)
                return;

            cancellationToken.ThrowIfCancellationRequested();
            await _currentUserRepository.UpdateLastActivity(userId, DateTime.UtcNow, cancellationToken);
        }
    }
}