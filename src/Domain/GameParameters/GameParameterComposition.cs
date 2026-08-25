using System.Collections.Generic;
using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.GameParameters
{
    public class GameParameterComposition
    {
        public DisplayInfo DisplayInfo { get; }
        public IReadOnlyList<IGameParameter> Parameters { get; }

        public GameParameterComposition(
            DisplayInfo displayInfo, 
            IReadOnlyList<IGameParameter> parameters)
        {
            DisplayInfo = displayInfo;
            Parameters = parameters;
        }
    }
}
