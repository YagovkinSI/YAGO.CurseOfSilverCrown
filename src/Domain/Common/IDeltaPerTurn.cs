using System.Numerics;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.Common
{
    public interface IDeltaPerTurn<T>
        where T : INumber<T>
    {
        T GetDeltaPerTurn(ColonyState colonyState);
    }
}
