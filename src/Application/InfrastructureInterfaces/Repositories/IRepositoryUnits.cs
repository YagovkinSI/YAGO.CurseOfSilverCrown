using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Units;
using YAGO.World.Domain.Units.Enums;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IRepositoryUnits
    {
        Task DisbandmentUnit(int unitId, int turnId, CancellationToken cancellationToken);
        Task<UnitWithFaction?> FindUnitWithFaction(int unitId, CancellationToken cancellationToken);
        Task SetCommand(int unitId, UnitCommandType commandType, int? targetDomainId, int? target2DomainId, CancellationToken cancellationToken);
    }
}
