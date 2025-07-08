using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Provinces;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/provinces")]
    public class ProvincesController : Controller
    {
        private readonly IRepositoryProvince _repositoryProvince;

        public ProvincesController(IRepositoryProvince repositoryProvince)
        {
            _repositoryProvince = repositoryProvince;
        }

        public async Task<ProvinceWithUser> GetProvinceWithUser(int? id, CancellationToken cancellationToken)
        {
            return id == null
                ? throw new YagoException("Некорректная ссылка. Отсутствует ID провинции.")
                : await _repositoryProvince.GetProvinceWithUser(id.Value, cancellationToken);
        }
    }
}
