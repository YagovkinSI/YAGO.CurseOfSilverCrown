using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Units.Enums;
using YAGO.World.Infrastructure.APIModels.BudgetModels;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Database.Models.Units;
using YAGO.World.Infrastructure.Parameters;

namespace YAGO.World.Infrastructure.APIModels
{
    public class UnitEditor
    {
        public Unit Unit { get; }
        public Organization Domain { get; }
        public Organization Position { get; }
        public string Description { get; }
        public IEnumerable<Unit> UnitsForUnion { get; }
        public bool SeparationAvailable { get; }

        public Dictionary<UnitCommandType, bool> AvailableCommands = new()
        {
            { UnitCommandType.Disbandment, true },
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

            CheckAvailableCommands();

            var budget = new Budget(context, Domain);
            Description = budget.Lines.Single(l => l.CommandSourceTable == BudgetLineSource.Units && l.CommandId == unit.Id).Descripton;
        }

        private void CheckAvailableCommands()
        {
            if (Unit.Warriors < WarConstants.MinWarrioirsForAtack)
                AvailableCommands[UnitCommandType.War] = false;
        }
    }
}
