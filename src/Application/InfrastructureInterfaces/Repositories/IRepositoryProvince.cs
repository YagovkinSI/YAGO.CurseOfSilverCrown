using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Provinces;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IRepositoryProvince
    {
        Task<ProvinceWithUser> GetProvinceWithUser(int id, CancellationToken cancellationToken);
    }
}
