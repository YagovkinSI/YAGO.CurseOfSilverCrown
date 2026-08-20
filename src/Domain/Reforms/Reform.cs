using YAGO.World.Domain.Common;
using YAGO.World.Domain.GameActions;

namespace YAGO.World.Domain.Reforms
{
    public class Reform
    {
        public string Code { get; }
        public DisplayInfo DisplayInfo { get; }
        public GameAction Action { get; }

        public Reform(
            string code,
            DisplayInfo displayInfo,
            GameAction action)
        {
            Code = code;
            DisplayInfo = displayInfo;
            Action = action;
        }
    }
}
