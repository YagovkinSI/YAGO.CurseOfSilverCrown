using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Game.Map.Routes;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands;
using YSI.CurseOfSilverCrown.Core.MainModels.Commands.UnitCommands;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;
using YSI.CurseOfSilverCrown.Core.MainModels.Events;
using YSI.CurseOfSilverCrown.Core.MainModels.Turns;
using YSI.CurseOfSilverCrown.Core.MainModels.Units;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.Actions
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
            return Unit.Status == enCommandStatus.Retreat;
        }

        //TODO: Big method
        protected override bool Execute()
        {
            var unitDomain = Context.Domains.Find(Unit.DomainId);
            var currentPositionDomain = Context.Domains.Find(Unit.PositionDomainId);
            if (Context.Domains.IsSameKingdoms(unitDomain, currentPositionDomain) ||
                DomainRelationsHelper.HasPermissionOfPassage(Context, unitDomain.Id, currentPositionDomain.Id))
            {
                Unit.Status = enCommandStatus.Complited;
                Unit.Type = enUnitCommandType.WarSupportDefense;
                Unit.TargetDomainId = Unit.DomainId;
                Unit.Target2DomainId = null;
                Unit.Status = enCommandStatus.Complited;
                Context.Update(Unit);
                return true;
            }

            var routeFindParameters = new RouteFindParameters(Unit, enMovementReason.Retreat, MovingTarget);
            var route = RouteHelper.FindRoute(Context, routeFindParameters);
            if (route == null || route.Count == 1)
            {
                CreateEventDestroyed(Unit);
                Unit.Status = enCommandStatus.Destroyed;
                Unit.Warriors = 0;
                Context.Update(Unit);
                return true;
            }

            var newPositionId = route[1].Id;
            CreateEvent(newPositionId);
            Unit.PositionDomainId = newPositionId;
            Unit.Type = enUnitCommandType.WarSupportDefense;
            Unit.TargetDomainId = Unit.DomainId;
            Unit.Target2DomainId = null;
            Unit.Status = enCommandStatus.Complited;
            Context.Update(Unit);
            return true;
        }

        private void CreateEventDestroyed(Unit unit)
        {
            var eventStoryResult = new EventJson(enEventType.DestroyedUnit);
            var allDomainUnits = Context.Units
                .Where(u => u.DomainId == unit.DomainId &&
                    u.InitiatorPersonId == unit.Domain.PersonId)
                .Sum(u => u.Warriors);
            var temp = new List<EventJsonParametrChange>
            {
                EventJsonParametrChangeHelper.Create(enEventParameterType.WarriorInWar, unit.Warriors, 0),
                EventJsonParametrChangeHelper.Create(enEventParameterType.Warrior, allDomainUnits, allDomainUnits - unit.Warriors)
            };
            eventStoryResult.AddEventOrganization(Unit.Domain.Id, enEventDomainType.Main, temp);
            eventStoryResult.AddEventOrganization(Unit.PositionDomainId.Value, enEventDomainType.Target, new List<EventJsonParametrChange>());
            CreateEventStory(eventStoryResult,
                new Dictionary<int, int> { { Unit.DomainId, Unit.Warriors * WarriorParameters.Price * 2 } });
        }

        private void CreateEvent(int newPostionId)
        {
            var unitMoving = Unit.PositionDomainId != newPostionId;
            var type = unitMoving
                ? enEventType.UnitMove
                : enEventType.UnitCantMove;
            var eventStoryResult = new EventJson(type);
            eventStoryResult.AddEventOrganization(Unit.Domain.Id, enEventDomainType.Main, new List<EventJsonParametrChange>());
            eventStoryResult.AddEventOrganization(Unit.PositionDomainId.Value, enEventDomainType.Vasal, new List<EventJsonParametrChange>());
            eventStoryResult.AddEventOrganization(unitMoving ? newPostionId : Unit.TargetDomainId.Value, enEventDomainType.Target, new List<EventJsonParametrChange>());
            CreateEventStory(eventStoryResult, new Dictionary<int, int> { { Unit.DomainId, unitMoving ? 100 : 500 } });
        }
    }
}
