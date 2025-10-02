using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Users.Interfaces;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.Users
{
    public class UserService : IUserService
    {
        private const int TimeoutBetweenUpdateLastActivityInSeconds = 30;

        public readonly IIdentityManager _identityManager;
        private readonly IUserRepository _currentUserRepository;

        public UserService(
            IIdentityManager identityManager,
            IUserRepository currentUserRepository)
        {
            _identityManager = identityManager;
            _currentUserRepository = currentUserRepository;
        }

        public async Task<User?> GetMyUser(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var currentUser = await _identityManager.GetCurrentUser(userClaimsPrincipal, cancellationToken);
            if (currentUser == null)
                return null;

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

        public async Task<User> ConvertToPermanentUser(
            ClaimsPrincipal userClaimsPrincipal,
            string userName,
            string? email,
            string password,
            CancellationToken cancellationToken)
        {
            var permanentUser = await _identityManager.ConvertToPermanentAccount(
                userClaimsPrincipal,
                userName,
                password,
                email,
                cancellationToken);

            await UpdateLastActivity(permanentUser.Id, cancellationToken);
            return permanentUser;
        }

        public async Task<User> Login(
            string userName,
            string? password,
            CancellationToken cancellationToken)
        {
            await _identityManager.Login(userName, password, cancellationToken);

            var currentUser = await _currentUserRepository.FindByName(userName, cancellationToken);

            await UpdateLastActivity(currentUser!.Id, cancellationToken);
            return currentUser;
        }

        public async Task Logout(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var myUser = GetMyUser(userClaimsPrincipal, cancellationToken);
            if (myUser == null)
                return;

            await _identityManager.Logout(cancellationToken);
        }

        public async Task UpdateLastActivity(long userId, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserRepository.Find(userId, cancellationToken);
            if (currentUser == null)
                return;

            var coolDown = TimeSpan.FromSeconds(TimeoutBetweenUpdateLastActivityInSeconds);
            if (currentUser.LastActivityAtUtc > DateTime.UtcNow - coolDown)
                return;

            await _currentUserRepository.UpdateLastActivity(userId, cancellationToken);
        }
    }
}