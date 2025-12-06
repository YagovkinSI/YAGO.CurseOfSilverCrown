using System;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyState
    {
        public ColonyStateType Type { get; }
        public int CycleRemaining { get; private set; }

        public ColonyState(
            ColonyStateType type,
            int cycleRemaining)
        {
            Type = type;
            CycleRemaining = cycleRemaining;
        }

        public static ColonyState CreateNew(ColonyStateType colonyStateType, int cycleRemaining)
        {
            if (colonyStateType == ColonyStateType.Unknown)
                throw new YagoUnknownTypeException(nameof(ColonyStateType));

            return new ColonyState(colonyStateType, cycleRemaining);
        }
    }
}
