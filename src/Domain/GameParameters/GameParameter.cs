namespace YAGO.World.Domain.GameParameters
{
    public class GameParameter
    {
        public GameParameterType ParameterType { get; }
        public double Value { get; }

        public GameParameter(
            GameParameterType parameterType,
            double value)
        {
            ParameterType = parameterType;
            Value = value;
        }
    }
}
