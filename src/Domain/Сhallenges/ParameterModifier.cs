using YAGO.World.Domain.Colonies;

namespace YAGO.World.Domain.Сhallenges
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
