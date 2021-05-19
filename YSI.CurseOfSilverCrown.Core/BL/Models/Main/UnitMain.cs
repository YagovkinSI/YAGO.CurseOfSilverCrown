using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.BL.Models.Min;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Core.BL.Models.Main
{
    public class UnitMain : UnitMin
    {
        public DomainMin Domain { get; set; }
        public DomainMin Target { get; set; }
        public DomainMin Target2 { get; set; }
        public DomainMin Position { get; set; }
        public DomainMin Initiator { get; set; }

        public UnitMain(Unit unit)
            : base(unit)
        {
            Domain = new DomainMin(unit.Domain);
            Target = unit.TargetDomainId == null
                ? null
                : new DomainMin(unit.Target);
            Target2 = unit.Target2DomainId == null
                ? null
                : new DomainMin(unit.Target2);
            Position = new DomainMin(unit.Position);
            Initiator = new DomainMin(unit.Initiator);
        }

        public override string ToString()
        {
            return $"Отряд владения {Domain.Name} во владении {Position.Name}, воинов - {Warriors}";
        }
    }
}
