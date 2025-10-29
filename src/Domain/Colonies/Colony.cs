using System.Linq;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Colonies
{
    /// <summary>
    /// Колония
    /// </summary>
    public class Colony : IEntity
    {
        /// <summary>
        /// Идентифиикатор колонии
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Идентифиикатор пользователя владельца
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Солары
        /// </summary>
        public decimal Solars { get; private set; }

        /// <summary>
        /// Идентифиикатор корабля
        /// </summary>
        public long ShipId => 1;

        /// <summary>
        /// Идентифиикаторы построек
        /// </summary>
        public long[] BuildingIds { get; private set; }

        public Colony(
            long id,
            long userId,
            string name,
            decimal solars,
            long[] buildingIds)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Solars = solars;
            BuildingIds = buildingIds;
        }

        public static Colony CreateNew(
            long userId,
            string name,
            ColonyPresetType presetType)
        {
            var buildingIds = GetBuildingIds(presetType);

            return new Colony(
                id: default,
                userId: userId,
                name: name,
                solars: 1000,
                buildingIds: buildingIds
            );
        }

        public void AddSolars(decimal value)
        {
            Solars += value;
        }

        public void AddBuildingId(long buildingId)
        {
            var list = BuildingIds.ToList();
            list.Add(buildingId);
            BuildingIds = list.ToArray();
        }

        private static long[] GetBuildingIds(ColonyPresetType colonyPresetType)
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
