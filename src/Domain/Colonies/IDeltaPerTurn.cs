using System.Numerics;

namespace YAGO.World.Domain.Colonies
{
    public interface IDeltaPerTurn<T>
        where T : INumber<T>
    {
        T GetDeltaPerTurn(ColonyState colonyState);
    }
}
