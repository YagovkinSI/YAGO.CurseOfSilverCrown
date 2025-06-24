using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Units;
using YAGO.World.Domain.Units.Enums;
using YAGO.World.Infrastructure.Database.Models.Units.Extensions;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class RepositoryUnits : IRepositoryUnits
    {
        private readonly ApplicationDbContext _context;

        public RepositoryUnits(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UnitWithFaction?> FindUnitWithFaction(int unitId, CancellationToken cancellationToken)
        {
            var unitDb = await _context.Units.FindAsync(unitId, cancellationToken);
            return unitDb?.ToUnitWithFaction();
        }

        public async Task SetCommand(int unitId, UnitCommandType commandType, int? targetDomainId, int? target2DomainId, CancellationToken cancellationToken)
        {
            var unitDb = await _context.Units.FindAsync(unitId, cancellationToken);
            if (unitDb == null)
                throw new YagoNotFoundException("Unit", unitId);

            unitDb.Type = commandType;
            unitDb.TargetDomainId = targetDomainId;
            unitDb.Target2DomainId = target2DomainId;

            _context.Update(unitDb);
            await _context.SaveChangesAsync();
        }
    }
}
