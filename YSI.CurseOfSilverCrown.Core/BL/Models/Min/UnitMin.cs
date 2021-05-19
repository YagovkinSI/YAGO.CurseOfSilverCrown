using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.BL.Models.Main;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Core.BL.Models.Min
{
    public class UnitMin
    {
        public int Id { get; }
        public int DomainId { get; }

        [Display(Name = "Казна")]
        public int Coffers { get; }

        [Display(Name = "Воины")]
        public int Warriors { get; }


        [Display(Name = "Действие")]
        public enArmyCommandType Type { get; }

        [Display(Name = "Цель")]
        public int? TargetDomainId { get; }

        [Display(Name = "Дополнительная цель")]
        public int? Target2DomainId { get; }

        [Display(Name = "Инициатор приказа")]
        public int InitiatorDomainId { get; }

        [Display(Name = "Местоположение")]
        public int? PositionDomainId { get; }

        [Display(Name = "Статус")]
        public enCommandStatus Status { get; }

        public UnitMin(Unit unit)
        {
            Id = unit.Id;
            DomainId = unit.DomainId;
            Coffers = unit.Coffers;
            Warriors = unit.Warriors;
            Type = unit.Type;
            TargetDomainId = unit.TargetDomainId;
            Target2DomainId = unit.Target2DomainId;
            InitiatorDomainId = unit.InitiatorDomainId;
            PositionDomainId = unit.PositionDomainId;
            Status = unit.Status;
        }

        public async Task<UnitMain> GetUnitMain(ApplicationDbContext context)
        {
            return await context.GetUnitMain(Id);
        }
    }
}
