using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Parameters;

namespace YSI.CurseOfSilverCrown.Core.ViewModels
{
    public class UnitEditor
    {
        public Unit Unit { get; }
        public Domain Domain { get; }
        public Domain Position { get; }
        public string Description { get; }
        public IEnumerable<Unit> UnitsForUnion { get; }
        public bool SeparationAvailable { get; }

        public Dictionary<enArmyCommandType, bool> AvailableCommands = new Dictionary<enArmyCommandType, bool>
        {
            { enArmyCommandType.CollectTax, false },
            { enArmyCommandType.War, true },
            { enArmyCommandType.WarSupportAttack, true },
            { enArmyCommandType.WarSupportDefense, true }
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
                AvailableCommands[enArmyCommandType.War] = false;

            var collectTax = context.Units
                    .SingleOrDefault(c => c.DomainId == unit.DomainId
                        && c.Type == enArmyCommandType.CollectTax);
            if (collectTax != null)
                AvailableCommands[enArmyCommandType.CollectTax] = false;
        }
    }
}
