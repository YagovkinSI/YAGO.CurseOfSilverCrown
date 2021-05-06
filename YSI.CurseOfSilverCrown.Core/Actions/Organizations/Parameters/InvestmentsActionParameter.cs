using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Actions.Organizations.Parameters
{
    internal class InvestmentsActionParameter : ActionParameter
    {
        public InvestmentsActionParameter(int count)
            : base(count)
        {
            Type = Database.Enums.enActionParameter.Investments;
        }
    }
}
