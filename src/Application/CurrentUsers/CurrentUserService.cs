using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using YAGO.World.Domain.CurrentUsers;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.InfrastructureInterfaces;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.CurrentUsers
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IIdentityManager _identityManager;
        private readonly IRepositoryCurrentUser _currentUserRepository;
        private readonly IRepositoryFactions _factionsRepository;

        private readonly TimeSpan timeSpanBetweenUpdateLastActivity = TimeSpan.FromSeconds(30);

        public CurrentUserService(
            IIdentityManager identityManager,
            IRepositoryCurrentUser currentUserRepository,
            IRepositoryFactions factionsRepository)
        {
            _identityManager = identityManager;
            _currentUserRepository = currentUserRepository;
            _factionsRepository = factionsRepository;
        }

        public async Task<AuthorizationData> GetAuthorizationData(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentUser = await _identityManager.GetCurrentUser(userClaimsPrincipal, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            if (currentUser != null)
                await UpdateLastActivity(currentUser.Id, cancellationToken);

            var faction = currentUser == null
                ? null
                : await _factionsRepository.GetOrganizationByUser(currentUser.Id);

            return new AuthorizationData(currentUser, faction);
        }

        public async Task<AuthorizationData> Register(
            string userName,
            string email,
            string password,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var newUser = new CurrentUser(Guid.NewGuid().ToString(), userName, email, DateTime.UtcNow);
            await _identityManager.Register(newUser, password, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            return await Login(userName, password, cancellationToken);
        }

        public async Task<AuthorizationData> AutoRegister(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userName = $"User_{new Random().Next(0, 99999999)}";
            var password = $"TMP_{Guid.NewGuid().ToString()[..8]}";
            return await Register(userName, email: string.Empty, password, cancellationToken);
        }

        public async Task<AuthorizationData> ChangeRegistration(
            ClaimsPrincipal userClaimsPrincipal,
            string userName,
            string email,
            string password,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _identityManager.ChangeRegistration(userClaimsPrincipal, userName, email, password, cancellationToken);

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

            var faction = currentUser == null
                ? null
                : await _factionsRepository.GetOrganizationByUser(currentUser.Id);

            return new AuthorizationData(currentUser, faction);
        }

        public async Task Logout(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _identityManager.Logout(cancellationToken);
        }

        public async Task UpdateLastActivity(string userId, CancellationToken cancellationToken)
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