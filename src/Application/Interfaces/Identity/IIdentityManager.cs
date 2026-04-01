using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Users;

namespace YAGO.World.Application.Interfaces.Identity
{
    public interface IIdentityManager
    {
        Task Register(User newUser, string password, CancellationToken cancellationToken);
        Task CreateTemporaryUser(User newUser, CancellationToken cancellationToken);
        Task ConvertToPermanentAccount(User permanentUser, string password, CancellationToken cancellationToken);
        Task Login(string userName, string? password, CancellationToken cancellationToken);
        Task Logout(CancellationToken cancellationToken);
    }
}
