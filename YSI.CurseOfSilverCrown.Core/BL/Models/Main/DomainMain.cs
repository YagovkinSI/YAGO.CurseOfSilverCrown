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
    public class DomainMain : DomainMin
    {
        [Display(Name = "Сюзерен")]
        public DomainMin Suzerain { get; }

        [Display(Name = "Вассалы")]
        public IEnumerable<DomainMin> Vassals { get; }


        [Display(Name = "Отряды")]
        public IEnumerable<UnitMin> Units { get; }

        private int? warriors = null;

        [Display(Name = "Войско")]
        public int Warrioirs
        {
            get
            {
                if (warriors == null)
                    warriors = Units
                        .Where(u => u.InitiatorDomainId == Id)
                        .Sum(u => u.Warriors);
                return warriors.Value;
            }
        } 

        public DomainMain(Domain domain)
            :base(domain)
        {
            Suzerain = domain.SuzerainId == null
                ? null
                : new DomainMin(domain.Suzerain);
            Units = domain.Units.Select(u => new UnitMin(u));
            Vassals = domain.Vassals.Select(v => new DomainMin(v));
        }
    }
}
