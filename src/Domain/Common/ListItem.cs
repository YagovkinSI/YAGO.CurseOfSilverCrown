using YAGO.World.Domain.YagoEntities;

namespace YAGO.World.Domain.Common
{
    public class ListItem
    {
        public int? Number { get; }
        public YagoEntity Entity { get; }
        public YagoEntity? Value { get; }

        public ListItem(
            int? number,
            YagoEntity entity,
            YagoEntity? value)
        {
            Number = number;
            Entity = entity;
            Value = value;
        }
    }
}
