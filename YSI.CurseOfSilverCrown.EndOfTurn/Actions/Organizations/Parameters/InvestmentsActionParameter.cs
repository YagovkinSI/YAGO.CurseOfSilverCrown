using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions.Organizations.Parameters
{
    internal class InvestmentsActionParameter : ActionParameter
    {
        public InvestmentsActionParameter(int count)
            : base(count)
        {
            Type = enActionParameter.Investments;
        }
    }
}
