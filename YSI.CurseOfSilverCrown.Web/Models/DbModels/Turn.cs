using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Web.Models.DbModels
{
    public class Turn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Started { get; set; }
        public bool IsActive { get; set; }

        public List<Command> Commands { get; set; }
    }
}
