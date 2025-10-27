using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Users;

namespace YAGO.World.Application.Users
{
    public class UserService : IUserService
    {
        private const int TimeoutBetweenUpdateLastActivityInSeconds = 30;

        public readonly IIdentityManager _identityManager;
        private readonly IUserRepository _userRepository;

        public UserService(
            IIdentityManager identityManager,
            IUserRepository currentUserRepository)
        {
            _identityManager = identityManager;
            _userRepository = currentUserRepository;
        }

        public async Task<User?> GetMyUser(long userId, CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.Find(userId, cancellationToken);
            return currentUser;
        }

        public async Task<User> Register(
            string userName,
            string password,
            string? email,
            CancellationToken cancellationToken)
        {
            var newUser = User.CreateNew(userName, email);
            await _identityManager.Register(newUser, password, cancellationToken);

            return await Login(userName, password, cancellationToken);
        }

        public async Task<User> CreateTemporaryUser(CancellationToken cancellationToken)
        {
            var newUser = User.CreateTemporary();
            await _identityManager.CreateTemporaryUser(newUser, cancellationToken);

            return await Login(newUser.UserName, password: null, cancellationToken);
        }

        public async Task<User> ConvertToPermanentUser(
            long userId,
            string userName,
            string? email,
            string password,
            CancellationToken cancellationToken)
        {
            return await _identityManager.ConvertToPermanentAccount(
                userId,
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
            await _identityManager.Login(userName, password, cancellationToken);

            return await _userRepository.FindByName(userName, cancellationToken)
                ?? throw new YagoException($"Не удалось найти пользователя по имени '{userName}'");
        }

        public async Task Logout(CancellationToken cancellationToken)
        {
            await _identityManager.Logout(cancellationToken);
        }

        public async Task UpdateLastActivity(long userId, CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.Find(userId, cancellationToken);
            if (currentUser == null)
                return;

            var coolDown = TimeSpan.FromSeconds(TimeoutBetweenUpdateLastActivityInSeconds);
            if (currentUser.LastActivityAtUtc > DateTime.UtcNow - coolDown)
                return;

            await _userRepository.UpdateLastActivity(userId, cancellationToken);
        }
    }
}