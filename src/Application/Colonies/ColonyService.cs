using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Buildings;
using YAGO.World.Application.Users;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Application.Colonies
{
    public class ColonyService : IColonyService
    {
        public readonly IUserService _userService;
        private readonly IColonyRepository _colonyRepository;
        private readonly IBuildingRepository _buildingRepository;

        public ColonyService(
            IUserService userService,
            IColonyRepository colonyRepository,
            IBuildingRepository buildingRepository)
        {
            _userService = userService;
            _colonyRepository = colonyRepository;
            _buildingRepository = buildingRepository;
        }

        public async Task<Colony?> GetMyColony(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var myUser = await _userService.GetMyUser(userClaimsPrincipal, cancellationToken)
                ?? throw new YagoNotAuthorizedException();

            return await _colonyRepository.FindByUserId(myUser.Id, cancellationToken);
        }

        public async Task<ColonyWithShipAndBuildingsDto?> GetMyColonyWithShipAndBuildings(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var myUser = await _userService.GetMyUser(userClaimsPrincipal, cancellationToken)
                ?? throw new YagoNotAuthorizedException();

            var colony = await _colonyRepository.FindByUserId(myUser.Id, cancellationToken);
            if (colony == null)
                return null;

            return await GetColonyWithShipAndBuildingsDtoInner(colony.Id, cancellationToken);
        }

        public async Task<ColonyWithShipAndBuildingsDto> CreateColony(ClaimsPrincipal userClaimsPrincipal, string name, ColonyPresetType presetType, CancellationToken cancellationToken)
        {
            var myUser = await _userService.GetMyUser(userClaimsPrincipal, cancellationToken)
                ?? throw new YagoNotAuthorizedException();

            var userColony = await _colonyRepository.FindByUserId(myUser.Id, cancellationToken);
            if (userColony != null)
                throw new YagoException(string.Format("Пользователь '{0}' уже имеет колонию '{1}'.", myUser.UserName, userColony.Name));

            var colonyWithName = await _colonyRepository.FindByName(name, cancellationToken);
            if (colonyWithName != null)
                throw new YagoException(string.Format("Название колонии '{0}' уже занято.", name));

            var createColonyDto = new CreateColonyDto(
                myUser.Id,
                name,
                presetType);
            var colony = await _colonyRepository.CreateColomy(createColonyDto, cancellationToken);

            return await GetColonyWithShipAndBuildingsDtoInner(colony.Id, cancellationToken);
        }

        private async Task<ColonyWithShipAndBuildingsDto> GetColonyWithShipAndBuildingsDtoInner(long colonyId, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.Find(colonyId, cancellationToken);
            if (colony == null)
                throw new YagoNotFoundException(nameof(Colony), colonyId);

            var ship = Ship.GetDefaultShip();

            var buildingsTasks = colony.BuildingIds
                .Select(x => _buildingRepository.Find(x, cancellationToken))
                .ToArray();
            var buildings = await Task.WhenAll(buildingsTasks);
            if (buildings.Any(x => x == null))
                throw new YagoException("Не найдена одна из построек в списке.");

            return new ColonyWithShipAndBuildingsDto(
                colony,
                ship,
                buildings,
                colony.CalculateSolarIncome(buildings, ship),
                colony.CalculateReputation(buildings),
                colony.CalculatePopulation(buildings),
                colony.CalculateZonesOccupied(buildings));
        }
    }
}
