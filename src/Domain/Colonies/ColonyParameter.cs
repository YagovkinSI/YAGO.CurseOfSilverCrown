namespace YAGO.World.Domain.Colonies
{
    public class ColonyParameter
    {
        public ColonyParameterType Type { get; }
        public decimal Value { get; }

        public ColonyParameter(
            ColonyParameterType type, 
            decimal value)
        {
            Type = type;
            Value = value;
        }
    }
}
