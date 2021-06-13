using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.BL.Models.Min
{
    public class DomainMin
    {
        public int Id { get; }

        [Display(Name = "Владелец")]
        public int PersonId { get; }

        [Display(Name = "Название")]
        public string Name { get; }

        [Display(Name = "Казна")]
        public int Coffers { get; }

        [Display(Name = "Имущество владения")]
        public int Investments { get; }

        [Display(Name = "Укрепления")]
        public int Fortifications { get; }

        public double DefenseCoeficient { get; }

        public int? SuzerainId { get; set; }
        public int TurnOfDefeat { get; set; }

        public DomainMin(Domain domain)
        {
            Id = domain.Id;
            PersonId = domain.PersonId;
            Name = domain.Name;
            Coffers = domain.Coffers;
            Investments = domain.Investments;
            Fortifications = domain.Fortifications;
            SuzerainId = domain.SuzerainId;
            TurnOfDefeat = domain.TurnOfDefeat;
            DefenseCoeficient = FortificationsHelper.GetWariorDefenseCoeficient(WarConstants.WariorDefenseSupport,
                domain.Fortifications);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
