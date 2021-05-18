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
        public Unit Unit { get; }
        public DomainMain Domain { get; }
        public Domain Position { get; }
        public string Description { get; }
        public IEnumerable<Unit> UnitsForUnion { get; }
        public bool SeparationAvailable { get; }

        public Dictionary<enArmyCommandType, bool> AvailableCommands = new Dictionary<enArmyCommandType, bool>
        {
            { enArmyCommandType.CollectTax, true },
            { enArmyCommandType.Rebellion, true },
            { enArmyCommandType.War, true },
            { enArmyCommandType.WarSupportAttack, true },
            { enArmyCommandType.WarSupportDefense, true }
        };

        public UnitEditor(Unit unit, ApplicationDbContext context)
        {
            Unit = unit;
            
            var allDomainUnits = context.Units
                .Where(u => u.DomainId == unit.DomainId &&
                            u.InitiatorDomainId == unit.InitiatorDomainId);

            SeparationAvailable = allDomainUnits.Count() < Constants.MaxUnitCount;

            Domain = context.GetDomainMain(unit.DomainId).Result;

            Position = context.Domains
                .Single(u => u.Id == unit.PositionDomainId);

            UnitsForUnion = allDomainUnits
                .Where(u => u.PositionDomainId == unit.PositionDomainId && u.Id != unit.Id);
            if (unit.PositionDomainId != unit.DomainId || allDomainUnits.Any(u => u.Type == enArmyCommandType.CollectTax))
                AvailableCommands[enArmyCommandType.CollectTax] = false;
            if (Domain.SuzerainId == null)
                AvailableCommands[enArmyCommandType.Rebellion] = false;
            /*

        <p id="rebelionIsNotAvailibleText" hidden="true" style="color:red">
            Вы не можете нападать на сюзерена, которому вы проиграли воину менее 5 сезонов назад, так как сюзерен удерживает ваших родственников
            в плену.
            У вас будет возможность поднять восстание через несколько сезонов, а именно через @turnCountBeforeRebelion , когда вы сможете вызволить семью из плена.
        </p>
             */

            var budget = new Budget(context, Domain, unit.InitiatorDomainId);
            Description = budget.Lines.Single(l => l.CommandId == unit.Id).Descripton;
        }
    }
}
