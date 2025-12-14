using System.Collections.Generic;
using YAGO.World.Application.Common.Processors;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Cycles;

namespace YAGO.World.Application.Colonies.AttackColony
{
    public class AttackColonyResult : IProcessorResult
    {
        public Cycle MyCycle { get; }
        public ColonyWithShipAndBuildings MyColony { get; }
        public IReadOnlyList<ColonyWithShipAndBuildings> OtherColonies { get; }

        public AttackColonyResult(
            Cycle myCycle,
            ColonyWithShipAndBuildings myColony,
            IReadOnlyList<ColonyWithShipAndBuildings> otherColonies)
        {
            MyCycle = myCycle;
            MyColony = myColony;
            OtherColonies = otherColonies;
        }
    }
}
