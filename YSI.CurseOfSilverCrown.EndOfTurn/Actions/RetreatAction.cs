using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;
using YSI.CurseOfSilverCrown.EndOfTurn.Helpers;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
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

        protected override bool Execute()
        {
            var unitDomain = Context.Domains.Find(Unit.DomainId);
            var currentPositionDomain = Context.Domains.Find(Unit.PositionDomainId);
            if (KingdomHelper.IsSameKingdoms(Context.Domains, unitDomain, currentPositionDomain) ||
                DomainRelationsHelper.HasPermissionOfPassage(Context, unitDomain.Id, currentPositionDomain.Id))
            {
                Unit.Status = enCommandStatus.Complited;
                Unit.Type = enArmyCommandType.WarSupportDefense;
                Unit.TargetDomainId = Unit.DomainId;
                Unit.Target2DomainId = null;
                Unit.Status = enCommandStatus.Complited;
                Context.Update(Unit);
                return true;
            }

            var newPosition = RouteHelper.GetNextPosition(Context,
                Unit.DomainId, Unit.PositionDomainId.Value, MovingTarget, true, out _);

            if (Unit.PositionDomainId.Value == newPosition)
            {
                CreateEventDestroyed(Unit);
                Unit.Status = enCommandStatus.Destroyed;
                Unit.Warriors = 0;
                Context.Update(Unit);
                return true;
            }

            CreateEvent(newPosition);
            Unit.PositionDomainId = newPosition;
            Unit.Type = enArmyCommandType.WarSupportDefense;
            Unit.TargetDomainId = Unit.DomainId;
            Unit.Target2DomainId = null;
            Unit.Status = enCommandStatus.Complited;
            Context.Update(Unit);
            return true;
        }

        private void CreateEventDestroyed(Unit unit)
        {
            var eventStoryResult = new EventStoryResult(enEventResultType.DestroyedUnit);
            var allDomainUnits = Context.Units
                .Where(u => u.DomainId == unit.DomainId &&
                    u.InitiatorPersonId == unit.Domain.PersonId)
                .Sum(u => u.Warriors);
            var temp = new List<EventParametrChange>
            {
                EventParametrChangeHelper.Create(enActionParameter.WarriorInWar, unit.Warriors, 0),
                EventParametrChangeHelper.Create(enActionParameter.Warrior, allDomainUnits, allDomainUnits - unit.Warriors)
            };
            eventStoryResult.AddEventOrganization(Unit.Domain.Id, enEventOrganizationType.Main, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(Unit.PositionDomainId.Value, enEventOrganizationType.Target, new List<EventParametrChange>());
            CreateEventStory(eventStoryResult, new Dictionary<int, int> { { Unit.DomainId, 5000 } });
        }

        private void CreateEvent(int newPostionId)
        {
            var unitMoving = Unit.PositionDomainId != newPostionId;
            var type = unitMoving
                ? enEventResultType.UnitMove
                : enEventResultType.UnitCantMove;
            var eventStoryResult = new EventStoryResult(type);
            eventStoryResult.AddEventOrganization(Unit.Domain.Id, enEventOrganizationType.Main, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(Unit.PositionDomainId.Value, enEventOrganizationType.Vasal, new List<EventParametrChange>());
            eventStoryResult.AddEventOrganization(unitMoving ? newPostionId : Unit.TargetDomainId.Value, enEventOrganizationType.Target, new List<EventParametrChange>());
            CreateEventStory(eventStoryResult, new Dictionary<int, int> { { Unit.DomainId, 100 } });
        }
    }
}
