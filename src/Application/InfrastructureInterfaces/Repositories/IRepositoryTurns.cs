using System.Threading.Tasks;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IRepositoryTurns
    {
        public Task<int> GetCurrentTurnId();
    }
}
