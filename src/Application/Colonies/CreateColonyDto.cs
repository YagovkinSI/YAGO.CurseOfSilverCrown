using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies
{
    public record CreateColonyDto
    {
        public long UserId { get; }
        public string Name { get; }
        public long[] BuildingIds { get; }

        public CreateColonyDto(
            long userId,
            string name,
            ColonyPresetType presetType)
        {
            UserId = userId;
            Name = name;
            BuildingIds = GetBuildingIds(presetType);
        }

        private long[] GetBuildingIds(ColonyPresetType colonyPresetType)
        {
            return colonyPresetType switch
            {
                ColonyPresetType.Unknown => throw new YagoUnknownTypeException(nameof(ColonyPresetType)),
                ColonyPresetType.Humanist => new long[] { 1, 1 },
                ColonyPresetType.Pragmatist => new long[] { 2, 2 },
                ColonyPresetType.Dictator => new long[] { 3, 3 },
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}
