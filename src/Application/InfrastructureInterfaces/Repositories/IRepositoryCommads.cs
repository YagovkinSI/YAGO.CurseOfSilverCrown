using System.Threading.Tasks;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IRepositoryCommads
    {
        public Task CheckAndFix(int organizationId);

        public Task CheckAndFixForAll();
    }
}
