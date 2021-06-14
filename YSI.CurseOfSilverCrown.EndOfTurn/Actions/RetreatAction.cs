using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class RetreatAction : UnitActionBase
    {
        private int MovingTarget { get; set; }

        public RetreatAction(ApplicationDbContext context, Turn currentTurn, int unitId)
            : base (context, currentTurn, unitId)
        {
            MovingTarget = Unit.DomainId;
        }

        protected override bool CheckValidAction()
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
                Unit.DomainId,
                Unit.PositionDomainId.Value,
                MovingTarget,
                true);
            
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
                    u.InitiatorDomainId == unit.DomainId)
                .Sum(u => u.Warriors);
            var temp = new List<EventParametrChange>
                        {
                            new EventParametrChange
                            {
                                Type = enActionParameter.WarriorInWar,
                                Before = unit.Warriors,
                                After = 0
                            },
                            new EventParametrChange
                            {
                                Type = enActionParameter.Warrior,
                                Before = allDomainUnits,
                                After = allDomainUnits - unit.Warriors
                            }
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
