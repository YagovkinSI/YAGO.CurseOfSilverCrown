using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.BL.Models;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class UnitMoveAction : UnitActionBase
    {
        private int MovingTarget { get; set; }

        public UnitMoveAction(ApplicationDbContext context, Turn currentTurn, int unitId)
            : base (context, currentTurn, unitId)
        {            
        }

        protected override bool CheckValidAction()
        {
            var targetExist = SetMoveTarget();

            return targetExist && 
                Unit.PositionDomainId != MovingTarget;
        }

        private bool SetMoveTarget()
        {
            switch (Unit.Type)
            {
                case enArmyCommandType.ForDelete:
                    return false;
                case enArmyCommandType.CollectTax:
                    MovingTarget = Unit.DomainId;
                    return true;
                case enArmyCommandType.Rebellion:
                    var domain = Context.GetDomainMin(Unit.DomainId).Result;
                    if (domain.SuzerainId == null)
                        return false;
                    MovingTarget = domain.SuzerainId.Value;
                    return true;
                case enArmyCommandType.War:
                case enArmyCommandType.WarSupportAttack:
                case enArmyCommandType.WarSupportDefense:
                    if (Unit.TargetDomainId == null)
                        return false;
                    MovingTarget = Unit.TargetDomainId.Value;
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override bool Execute()
        {            
            var newPosition = RouteHelper.GetNextPosition(Context,
                Unit.DomainId,
                Unit.PositionDomainId.Value,
                MovingTarget);
            CreateEvent(newPosition);
            Unit.PositionDomainId = newPosition;
            Context.Update(Unit);
            return true;
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
