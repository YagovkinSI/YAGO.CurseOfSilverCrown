using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Units;
using YAGO.World.Domain.Units.Enums;
using YAGO.World.Infrastructure.Database.Models.EventDomains;
using YAGO.World.Infrastructure.Database.Models.Events;
using YAGO.World.Infrastructure.Database.Models.Units;
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

        public async Task DisbandmentUnit(int unitId, int turnId, CancellationToken cancellationToken)
        {
            var unitDb = await GetUnit(unitId, cancellationToken);

            var historyEvent = await CreateDisbandmentUnitHistoryEvent(turnId, unitDb.DomainId, unitDb.Warriors, cancellationToken);

            var eventObject = new EventObject()
            {
                DomainId = unitDb.DomainId,
                TurnId = turnId,
                Importance = unitDb.Warriors,
                EventStory = historyEvent,
            };

            _context.Events.Add(historyEvent);
            _context.EventObjects.Add(eventObject);
            _context.Units.Remove(unitDb);
            await _context.SaveChangesAsync();
        }

        public async Task<UnitWithFaction?> FindUnitWithFaction(int unitId, CancellationToken cancellationToken)
        {
            var unitDb = await _context.Units.FindAsync(new object[] { unitId }, cancellationToken);
            return unitDb?.ToUnitWithFaction();
        }

        public async Task SetCommand(int unitId, UnitCommandType commandType, int? targetDomainId, int? target2DomainId, CancellationToken cancellationToken)
        {
            var unitDb = await GetUnit(unitId, cancellationToken);

            unitDb.Type = commandType;
            unitDb.TargetDomainId = targetDomainId;
            unitDb.Target2DomainId = target2DomainId;

            _context.Update(unitDb);
            await _context.SaveChangesAsync();
        }

        private async Task<Event> CreateDisbandmentUnitHistoryEvent(int turnId, int domainId, int unitWarriorCount, CancellationToken cancellationToken)
        {
            var eventJson = await CreateDisbandmentUnitEventJson(domainId, unitWarriorCount, cancellationToken);

            var historyEvent = new Models.Events.Event()
            {
                TurnId = turnId,
                Type = Models.Events.EventType.DisbandmentUnit,
                EventJson = eventJson.ToJson(),
            };
            return historyEvent;
        }

        private async Task<EventJson> CreateDisbandmentUnitEventJson(int domainId, int unitWarriorCount, CancellationToken cancellationToken)
        {
            var warriorCount = await _context.Units
                .Where(u => u.DomainId == domainId)
                .SumAsync(u => u.Warriors, cancellationToken);

            return new EventJson()
            {
                Organizations = new System.Collections.Generic.List<EventParticipant>()
                {
                    new(domainId, EventParticipantType.Target)
                    {
                        EventOrganizationChanges = new System.Collections.Generic.List<EventParticipantParameterChange>()
                        {
                            new()
                            {
                                Type = EventParticipantParameterType.WarriorInDomain,
                                Before = warriorCount,
                                After = warriorCount - unitWarriorCount
                            }
                        }
                    }
                }
            };
        }

        private async Task<Unit> GetUnit(int unitId, CancellationToken cancellationToken)
        {
            var unitDb = await _context.Units.FindAsync(new object[] { unitId }, cancellationToken);
            return unitDb == null ? throw new YagoNotFoundException("Unit", unitId) : unitDb;
        }
    }
}
