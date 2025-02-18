using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Web.Database;
using YSI.CurseOfSilverCrown.Web.Database.Events;
using YSI.CurseOfSilverCrown.Web.Database.Turns;
using YSI.CurseOfSilverCrown.Web.Database.Units;
using YSI.CurseOfSilverCrown.Web.Helpers.Map.Routes;

namespace YSI.CurseOfSilverCrown.Web.Helpers.Actions
{
    internal class UnitMoveAction : UnitActionBase
    {
        private int MovingTarget { get; set; }
        private bool NeedIntoTarget { get; set; }

        public UnitMoveAction(ApplicationDbContext context, Turn currentTurn, int unitId)
            : base(context, currentTurn, unitId)
        {
        }

        public override bool CheckValidAction()
        {
            var targetExist = SetMoveTarget();

            return targetExist &&
                Unit.PositionDomainId != MovingTarget;
        }

        private bool SetMoveTarget()
        {
            switch (Unit.Type)
            {
                case UnitCommandType.ForDelete:
                    return false;
                case UnitCommandType.CollectTax:
                    NeedIntoTarget = true;
                    MovingTarget = Unit.DomainId;
                    return true;
                case UnitCommandType.War:
                case UnitCommandType.WarSupportAttack:
                case UnitCommandType.WarSupportDefense:
                    if (Unit.TargetDomainId == null)
                        return false;
                    NeedIntoTarget = Unit.Type == UnitCommandType.WarSupportDefense;
                    MovingTarget = Unit.TargetDomainId.Value;
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override bool Execute()
        {
            var reasonMovement = Unit.Type switch
            {
                UnitCommandType.ForDelete => enMovementReason.Retreat,
                UnitCommandType.War => enMovementReason.Atack,
                UnitCommandType.CollectTax => enMovementReason.Defense,
                UnitCommandType.WarSupportDefense => enMovementReason.Defense,
                UnitCommandType.WarSupportAttack => enMovementReason.SupportAttack,
                _ => throw new NotImplementedException(),
            };
            var routeFindParameters = new RouteFindParameters(Unit, reasonMovement, MovingTarget);
            var route = RouteHelper.FindRoute(Context, routeFindParameters);
            var newPosition = route == null || route.Count == 1
                ? Unit.PositionDomainId.Value
                : route[1].Id;
            CreateEvent(newPosition);
            Unit.PositionDomainId = newPosition;
            Context.Update(Unit);
            return true;
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
