using System;
using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands.UnitCommands;
using YSI.CurseOfSilverCrown.Core.Database.EventDomains;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Database.Turns;
using YSI.CurseOfSilverCrown.Core.Helpers.Map.Routes;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Actions
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
                case enUnitCommandType.ForDelete:
                    return false;
                case enUnitCommandType.CollectTax:
                    NeedIntoTarget = true;
                    MovingTarget = Unit.DomainId;
                    return true;
                case enUnitCommandType.War:
                case enUnitCommandType.WarSupportAttack:
                case enUnitCommandType.WarSupportDefense:
                    if (Unit.TargetDomainId == null)
                        return false;
                    NeedIntoTarget = Unit.Type == enUnitCommandType.WarSupportDefense;
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
                enUnitCommandType.ForDelete => enMovementReason.Retreat,
                enUnitCommandType.War => enMovementReason.Atack,
                enUnitCommandType.CollectTax => enMovementReason.Defense,
                enUnitCommandType.WarSupportDefense => enMovementReason.Defense,
                enUnitCommandType.WarSupportAttack => enMovementReason.SupportAttack,
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
