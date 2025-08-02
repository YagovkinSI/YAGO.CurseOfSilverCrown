using YAGO.World.Domain.YagoEntities.Enums;

namespace YAGO.World.Domain.YagoEntities
{
    public class YagoEntity
    {
        public long Id { get; }
        public YagoEntityType Type { get; }
        public string Name { get; }

        public YagoEntity(
            long id,
            YagoEntityType type,
            string name)
        {
            Id = id;
            Type = type;
            Name = name;
        }

        public static YagoEntity CreateFakeEntity(string name) => new(0, YagoEntityType.Unknown, name);
    }
}
