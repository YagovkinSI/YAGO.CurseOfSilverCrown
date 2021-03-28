using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Web.Enums
{
    public enum enEventOrganizationType
    {
        //Not matter
        Main = 1,

        //Vasal-Suzerain
        Vasal = 1001,
        Suzerain = 1002,

        //War
        Agressor = 2001,
        Defender = 2002
    }
}
