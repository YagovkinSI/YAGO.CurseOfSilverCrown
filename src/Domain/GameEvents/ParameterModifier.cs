namespace YAGO.World.Domain.GameEvents
{
    public class ParameterModifier
    {
        public ColonyParameterType ParameterType { get; set; }
        public double Coefficient { get; set; }

        public ParameterModifier(ColonyParameterType type, double coefficient)
        {
            ParameterType = type;
            Coefficient = coefficient;
        }
    }
}
