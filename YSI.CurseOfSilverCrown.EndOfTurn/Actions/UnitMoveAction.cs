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

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions
{
    internal class UnitMoveAction : UnitActionBase
    {
        public UnitMoveAction(ApplicationDbContext context, Turn currentTurn, Unit unit)
            : base (context, currentTurn, unit)
        {
        }

        protected override bool Execute()
        {
            int targetPosition;
            switch (Unit.Type)
            {
                case enArmyCommandType.CollectTax:
                    targetPosition = Unit.DomainId;
                    break;
                case enArmyCommandType.Rebellion:
                    var domain = Context.GetDomainMin(Unit.DomainId).Result;
                    targetPosition = domain.SuzerainId.Value;
                    break;
                case enArmyCommandType.War:
                    targetPosition = Unit.TargetDomainId.Value;
                    break;
                case enArmyCommandType.WarSupportAttack:
                    targetPosition = Unit.TargetDomainId.Value;
                    break;
                case enArmyCommandType.WarSupportDefense:
                    targetPosition = Unit.TargetDomainId.Value;
                    break;
                default:
                    throw new NotImplementedException();
            }
            var newPosition = RouteHelper.GetNextPosition(Context,
                Unit.PositionDomainId.Value,
                targetPosition);
            Unit.PositionDomainId = newPosition;
            Context.Update(Unit);
            return true;
        }
    }
}
