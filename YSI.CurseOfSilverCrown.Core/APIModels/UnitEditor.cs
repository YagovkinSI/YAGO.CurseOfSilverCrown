using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.APIModels.BudgetModels;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Commands.UnitCommands;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.Units;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.APIModels
{
    public class UnitEditor
    {
        public Unit Unit { get; }
        public Domain Domain { get; }
        public Domain Position { get; }
        public string Description { get; }
        public IEnumerable<Unit> UnitsForUnion { get; }
        public bool SeparationAvailable { get; }

        public Dictionary<enUnitCommandType, bool> AvailableCommands = new Dictionary<enUnitCommandType, bool>
        {
            { enUnitCommandType.CollectTax, false },
            { enUnitCommandType.War, true },
            { enUnitCommandType.WarSupportAttack, true },
            { enUnitCommandType.WarSupportDefense, true }
        };

        public UnitEditor(Unit unit, ApplicationDbContext context)
        {
            Unit = unit;
            var allDomainUnits = context.Units
                .Where(d => d.DomainId == unit.DomainId && d.InitiatorPersonId == unit.InitiatorPersonId);

            SeparationAvailable = allDomainUnits.Count() < Constants.MaxUnitCount;

            Domain = context.Domains.Find(unit.DomainId);

            Position = context.Domains
                .Single(u => u.Id == unit.PositionDomainId);

            UnitsForUnion = allDomainUnits
                .Where(u => u.PositionDomainId == unit.PositionDomainId && u.Id != unit.Id);

            CheckAvailableCommands(context, unit);

            var budget = new Budget(context, Domain, unit.InitiatorPersonId);
            Description = budget.Lines.Single(l => l.CommandSourceTable == enCommandSourceTable.Units && l.CommandId == unit.Id).Descripton;
        }

        private void CheckAvailableCommands(ApplicationDbContext context, Unit unit)
        {
            if (Unit.Warriors < WarConstants.MinWarrioirsForAtack)
                AvailableCommands[enUnitCommandType.War] = false;

            var collectTax = context.Units
                    .SingleOrDefault(c => c.DomainId == unit.DomainId
                        && c.Type == enUnitCommandType.CollectTax);
            if (collectTax != null)
                AvailableCommands[enUnitCommandType.CollectTax] = false;
        }
    }
}
