using YAGO.World.Application.Colonies;
using YAGO.World.Application.Common.Processors;

namespace YAGO.World.Application.GetColonyWithDetails
{
    public interface IColonyWithDetailsProvider : IProvider<GetColonyWithDetailsCommand, ColonyWithDetails?>
    {
    }
}
