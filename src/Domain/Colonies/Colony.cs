using System;
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
        public int Solars { get; private set; }

        /// <summary>
        /// Идентифиикатор корабля
        /// </summary>
        public long ShipId => 1;

        /// <summary>
        /// Идентифиикаторы юнитов
        /// </summary>
        public long[] UnitIds { get; private set; }

        /// <summary>
        /// Состояния колонии
        /// </summary>
        public ColonyState[] States { get; private set; }

        public int WarPower => UnitIds.Length;

        public Colony(
            long id,
            long userId,
            string name,
            int solars,
            long[] buildingIds,
            ColonyState[] colonyStates)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Solars = solars;
            UnitIds = buildingIds;
            States = colonyStates;
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
                buildingIds: buildingIds,
                colonyStates: new ColonyState[0]
            );
        }

        public void AddSolars(int value)
        {
            Solars += value;
        }

        public void AddBuildingId(long buildingId)
        {
            var list = UnitIds.ToList();
            list.Add(buildingId);
            UnitIds = list.ToArray();
        }

        public void AddState(ColonyStateType colonyStateType, int cycleRemaining)
        {
            if (colonyStateType == ColonyStateType.Unknown)
                throw new YagoUnknownTypeException(nameof(ColonyStateType));

            if (Array.Exists(States, x => x.Type == colonyStateType))
                throw new YagoException("Колония уже имеет данный статус.");

            var list = States.ToList();
            list.Add(ColonyState.CreateNew(colonyStateType, cycleRemaining));
            States = list.ToArray();
        }

        private static long[] GetBuildingIds(ColonyPresetType colonyPresetType)
        {
            return colonyPresetType switch
            {
                ColonyPresetType.Unknown => throw new YagoUnknownTypeException(nameof(ColonyPresetType)),
                ColonyPresetType.Humanist => new long[] { 1, 1, 1, 1 },
                ColonyPresetType.Pragmatist => new long[] { 2, 2, 2, 2 },
                ColonyPresetType.Tyrant => new long[] { 3, 3, 3, 3 },
                _ => throw new NotImplementedException(),
            };
        }
    }
}
