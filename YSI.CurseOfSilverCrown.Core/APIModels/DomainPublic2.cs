using System.Collections.Generic;

namespace YSI.CurseOfSilverCrown.Core.APIModels
{
    public class DomainPublic2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Warriors { get; set; }
        public int Gold { get; set; }
        public int Investments { get; set; }
        public int Fortifications { get; set; }
        public DomainLink Suzerain { get; set; }
        public int VassalCount { get; set; }
        public UserLink User { get; set; }

    }
}
