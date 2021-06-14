using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public User User { get; set; }
        public List<Domain> Domains { get; set; }
        public List<Unit> UnitsWithMyCommands { get; set; }
    }
}
