namespace YAGO.World.Domain.GameParameters
{
    public class GameParameterNumberValue : GameParameter<double>
    {
        public GameParameterType ParameterType { get; }

        public GameParameterNumberValue(
            GameParameterType name,
            double value) 
            : base(new Common.DisplayInfo(name.ToString()), value)
        {
            ParameterType = name;
        }
    }
}
