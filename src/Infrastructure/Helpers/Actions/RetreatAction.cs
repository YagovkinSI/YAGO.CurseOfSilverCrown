using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Units.Enums;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Commands;
using YAGO.World.Infrastructure.Database.Models.Events;
using YAGO.World.Infrastructure.Database.Models.Turns;
using YAGO.World.Infrastructure.Database.Models.Units;
using YAGO.World.Infrastructure.Helpers;
using YAGO.World.Infrastructure.Helpers.Events;
using YAGO.World.Infrastructure.Helpers.Map.Routes;
using YAGO.World.Infrastructure.Parameters;

namespace YAGO.World.Infrastructure.Helpers.Actions
{
    internal class RetreatAction : UnitActionBase
    {
        private int MovingTarget { get; set; }

        public RetreatAction(ApplicationDbContext context, Turn currentTurn, int unitId)
            : base(context, currentTurn, unitId)
        {
            MovingTarget = Unit.DomainId;
        }

        public override bool CheckValidAction()
        {
            return Unit.Status == CommandStatus.Retreat;
        }

        //TODO: Big method
        protected override bool Execute()
        {
            var unitDomain = Context.Domains.Find(Unit.DomainId);
            var currentPositionDomain = Context.Domains.Find(Unit.PositionDomainId);
            if (Context.Domains.IsSameKingdoms(unitDomain, currentPositionDomain) ||
                DomainRelationsHelper.HasPermissionOfPassage(Context, unitDomain.Id, currentPositionDomain.Id))
            {
                Unit.Status = CommandStatus.Complited;
                Unit.Type = UnitCommandType.WarSupportDefense;
                Unit.TargetDomainId = Unit.DomainId;
                Unit.Target2DomainId = null;
                Unit.Status = CommandStatus.Complited;
                Context.Update(Unit);
                return true;
            }

            var routeFindParameters = new RouteFindParameters(Unit, enMovementReason.Retreat, MovingTarget);
            var route = RouteHelper.FindRoute(Context, routeFindParameters);
            if (route == null || route.Count == 1)
            {
                CreateEventDestroyed(Unit);
                Unit.Status = CommandStatus.Destroyed;
                Unit.Warriors = 0;
                Context.Update(Unit);
                return true;
            }

            var newPositionId = route[1].Id;
            CreateEvent(newPositionId);
            Unit.PositionDomainId = newPositionId;
            Unit.Type = UnitCommandType.WarSupportDefense;
            Unit.TargetDomainId = Unit.DomainId;
            Unit.Target2DomainId = null;
            Unit.Status = CommandStatus.Complited;
            Context.Update(Unit);
            return true;
        }

        private void CreateEventDestroyed(Unit unit)
        {
            var eventStoryResult = new EventJson();
            var allDomainUnits = Context.Units
                .Where(u => u.DomainId == unit.DomainId)
                .Sum(u => u.Warriors);
            var temp = new List<EventParticipantParameterChange>
            {
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.WarriorInWar, unit.Warriors, 0),
                EventJsonParametrChangeHelper.Create(EventParticipantParameterType.Warrior, allDomainUnits, allDomainUnits - unit.Warriors)
            };
            eventStoryResult.AddEventOrganization(Unit.Domain.Id, EventParticipantType.Main, temp);
            eventStoryResult.AddEventOrganization(Unit.PositionDomainId.Value, EventParticipantType.Target, new List<EventParticipantParameterChange>());
            CreateEventStory(eventStoryResult,
                new Dictionary<int, int> { { Unit.DomainId, Unit.Warriors * WarriorParameters.Price * 2 } },
                EventType.DestroyedUnit);
        }

        private void CreateEvent(int newPostionId)
        {
            var unitMoving = Unit.PositionDomainId != newPostionId;
            var eventStoryResult = new EventJson();
            eventStoryResult.AddEventOrganization(Unit.Domain.Id, EventParticipantType.Main, new List<EventParticipantParameterChange>());
            eventStoryResult.AddEventOrganization(Unit.PositionDomainId.Value, EventParticipantType.Vasal, new List<EventParticipantParameterChange>());
            eventStoryResult.AddEventOrganization(unitMoving ? newPostionId : Unit.TargetDomainId.Value, EventParticipantType.Target, new List<EventParticipantParameterChange>());

            var type = unitMoving
                ? EventType.UnitMove
                : EventType.UnitCantMove;
            CreateEventStory(eventStoryResult, new Dictionary<int, int> { { Unit.DomainId, unitMoving ? 100 : 500 } }, type);
        }
    }
}
