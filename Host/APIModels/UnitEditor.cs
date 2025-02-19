using System.Collections.Generic;
using System.Linq;
using YAGO.World.Host.APIModels.BudgetModels;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Units;
using YAGO.World.Host.Infrastructure.Database;
using YAGO.World.Host.Parameters;

namespace YAGO.World.Host.APIModels
{
    public class UnitEditor
    {
        public Unit Unit { get; }
        public Organization Domain { get; }
        public Organization Position { get; }
        public string Description { get; }
        public IEnumerable<Unit> UnitsForUnion { get; }
        public bool SeparationAvailable { get; }

        public Dictionary<UnitCommandType, bool> AvailableCommands = new Dictionary<UnitCommandType, bool>
        {
            { UnitCommandType.CollectTax, false },
            { UnitCommandType.War, true },
            { UnitCommandType.WarSupportAttack, true },
            { UnitCommandType.WarSupportDefense, true }
        };

        public UnitEditor(Unit unit, ApplicationDbContext context)
        {
            Unit = unit;
            var allDomainUnits = context.Units
                .Where(d => d.DomainId == unit.DomainId);

            SeparationAvailable = allDomainUnits.Count() < Constants.MaxUnitCount;

            Domain = context.Domains.Find(unit.DomainId);

            Position = context.Domains
                .Single(u => u.Id == unit.PositionDomainId);

            UnitsForUnion = allDomainUnits
                .Where(u => u.PositionDomainId == unit.PositionDomainId && u.Id != unit.Id);

            CheckAvailableCommands(context, unit);

            var budget = new Budget(context, Domain);
            Description = budget.Lines.Single(l => l.CommandSourceTable == BudgetLineSource.Units && l.CommandId == unit.Id).Descripton;
        }

        private void CheckAvailableCommands(ApplicationDbContext context, Unit unit)
        {
            if (Unit.Warriors < WarConstants.MinWarrioirsForAtack)
                AvailableCommands[UnitCommandType.War] = false;

            var collectTax = context.Units
                    .SingleOrDefault(c => c.DomainId == unit.DomainId
                        && c.Type == UnitCommandType.CollectTax);
            if (collectTax != null)
                AvailableCommands[UnitCommandType.CollectTax] = false;
        }
    }
}
