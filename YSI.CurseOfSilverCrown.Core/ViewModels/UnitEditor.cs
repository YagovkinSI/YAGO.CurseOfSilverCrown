using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.BL.Models;
using YSI.CurseOfSilverCrown.Core.BL.Models.Main;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.ViewModels
{
    public class UnitEditor
    {
        public UnitMain Unit { get; }
        public DomainMain Domain { get; }
        public Domain Position { get; }
        public string Description { get; }
        public IEnumerable<UnitMain> UnitsForUnion { get; }
        public bool SeparationAvailable { get; }

        public Dictionary<enArmyCommandType, bool> AvailableCommands = new Dictionary<enArmyCommandType, bool>
        {
            { enArmyCommandType.CollectTax, true },
            { enArmyCommandType.War, true },
            { enArmyCommandType.WarSupportAttack, true },
            { enArmyCommandType.WarSupportDefense, true }
        };

        public UnitEditor(UnitMain unit, ApplicationDbContext context)
        {
            Unit = unit;
            
            var allDomainUnits = context.GetUnitsMainAsync(unit.DomainId, unit.InitiatorDomainId).Result;

            SeparationAvailable = allDomainUnits.Count() < Constants.MaxUnitCount;

            Domain = context.GetDomainMain(unit.DomainId).Result;

            Position = context.Domains
                .Single(u => u.Id == unit.PositionDomainId);

            UnitsForUnion = allDomainUnits
                .Where(u => u.PositionDomainId == unit.PositionDomainId && u.Id != unit.Id);
            if (Unit.Warriors < WarConstants.MinWarrioirsForAtack)
                AvailableCommands[enArmyCommandType.War] = false;          

            var budget = new Budget(context, Domain, unit.InitiatorDomainId);
            Description = budget.Lines.Single(l => l.CommandId == unit.Id).Descripton;
        }
    }
}
