using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Actions.Organizations.Parameters
{
    internal abstract class ActionParameter
    {
        public enActionParameter Type { get; protected set; }
        public int Before { get; protected set; }
        public int After { get; protected set; }

        public ActionParameter(int count)
        {
            Before = After = count;
        }
    }
}
