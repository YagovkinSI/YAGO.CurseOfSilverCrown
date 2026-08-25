using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.GameParameters
{
    public interface IGameParameter
    {
        DisplayInfo DisplayInfo { get; }
    }

    public class GameParameter<T> : IGameParameter
    {
        public DisplayInfo DisplayInfo { get; }
        public T Value { get; }


        public GameParameter(
            DisplayInfo displayInfo,
            T value)
        {
            DisplayInfo = displayInfo;
            Value = value;
        }
    }
}
