using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUser;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Units.Enums;

namespace YAGO.World.Application.Units
{
    public class UnitService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepositoryUnits _repositoryUnits;

        public UnitService(
            ICurrentUserService currentUserService,
            IRepositoryUnits repositoryUnits)
        {
            _currentUserService = currentUserService;
            _repositoryUnits = repositoryUnits;
        }

        public async Task SetUnitCommand(
            int unitId,
            UnitCommandType commandType,
            int? targetDomainId,
            int? target2DomainId,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            await VerifyUnitOwnership(unitId, claimsPrincipal, cancellationToken);

            await _repositoryUnits.SetCommand(unitId, commandType, targetDomainId, target2DomainId, cancellationToken);
        }

        private async Task VerifyUnitOwnership(int unitId, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.FindCurrentUser(claimsPrincipal);
            if (currentUser == null)
                throw new YagoNotAuthorizedException();

            var unitWithFaction = await _repositoryUnits.FindUnitWithFaction(unitId, cancellationToken);
            if (unitWithFaction == null)
                throw new YagoNotFoundException("Unit", unitId);

            if (unitWithFaction.Faction.UserId != currentUser.Id)
                throw new YagoNotVerifyOwnershipException("Unit", unitId);
        }
    }
}
