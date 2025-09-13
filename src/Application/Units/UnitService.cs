using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Units.Enums;

namespace YAGO.World.Application.Units
{
    public class UnitService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepositoryUnits _repositoryUnits;
        private readonly IRepositoryTurns _repositoryTurns;

        public UnitService(
            ICurrentUserService currentUserService,
            IRepositoryUnits repositoryUnits,
            IRepositoryTurns repositoryTurns)
        {
            _currentUserService = currentUserService;
            _repositoryUnits = repositoryUnits;
            _repositoryTurns = repositoryTurns;
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

            var task = commandType switch
            {
                UnitCommandType.Disbandment => DisbandmentUnit(unitId, cancellationToken),
                _ => _repositoryUnits.SetCommand(unitId, commandType, targetDomainId, target2DomainId, cancellationToken)
            };
            await task;
        }

        private async Task DisbandmentUnit(int unitId, CancellationToken cancellationToken)
        {
            var currentTurnId = await _repositoryTurns.GetCurrentTurnId();
            await _repositoryUnits.DisbandmentUnit(unitId, currentTurnId, cancellationToken);
        }

        private async Task VerifyUnitOwnership(int unitId, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var authorizationData = await _currentUserService.GetAuthorizationData(claimsPrincipal, cancellationToken);
            if (!authorizationData.IsAuthorized)
                throw new YagoNotAuthorizedException();

            var unitWithFaction = await _repositoryUnits.FindUnitWithFaction(unitId, cancellationToken);
            if (unitWithFaction == null)
                throw new YagoNotFoundException("Unit", unitId);

            if (unitWithFaction.Faction.UserId != authorizationData.User!.Id)
                throw new YagoNotVerifyOwnershipException("Unit", unitId);
        }
    }
}
