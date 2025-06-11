using YAGO.World.Domain.YagoEntities.Enums;

namespace YAGO.World.Domain.YagoEntities
{
    public class YagoEntity
    {
        public int Id { get; }
        public YagoEntityType Type { get; }
        public string Name { get; }

        public YagoEntity(
            int id,
            YagoEntityType type,
            string name)
        {
            Id = id;
            Type = type;
            Name = name;
        }
    }
}
