using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Users;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Users
{
    public class UserService : IUserService
    {
        public readonly IIdentityManager _identityManager;
        private readonly IUserRepository _userRepository;
        private readonly ILoginUserProcessor _loginUserProcessor;

        public UserService(
            IIdentityManager identityManager,
            IUserRepository currentUserRepository,
            ILoginUserProcessor loginUserProcessor)
        {
            _identityManager = identityManager;
            _userRepository = currentUserRepository;
            _loginUserProcessor = loginUserProcessor;
        }

        public async Task CreateTemporaryUser(CancellationToken cancellationToken)
        {
            var newUser = User.CreateTemporary();
            await _identityManager.CreateTemporaryUser(newUser, cancellationToken);

            var command = new LoginUserCommand(newUser.UserName, Password: null);
            await _loginUserProcessor.Execute(command, cancellationToken);
        }

        public async Task<User> ConvertToPermanentUser(
            long userId,
            string userName,
            string? email,
            string password,
            CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.Find(userId, cancellationToken)
                ?? throw new YagoNotAuthorizedException();

            currentUser.ConvertToPermanentAccount(userName, email);

            var isUserNameTaken = await _userRepository.FindByName(userName, cancellationToken) != null;
            if (isUserNameTaken)
                throw new YagoException("Имя пользователя уже занято");

            return await _identityManager.ConvertToPermanentAccount(
                currentUser,
                password,
                cancellationToken);
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

            var success = currentUser.TryUpdateLastActivityIfNeeded();
            if (success)
                await _userRepository.Update(currentUser, cancellationToken);
        }
    }
}