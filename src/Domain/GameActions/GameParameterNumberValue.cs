namespace YAGO.World.Domain.GameActions
{
    public class GameParameterNumberValue
    {
        public GameParameterType ParameterType { get; }
        public double Value { get; }

        public GameParameterNumberValue(
            GameParameterType name,
            double value)
        {
            ParameterType = name;
            Value = value;
        }
    }
}
