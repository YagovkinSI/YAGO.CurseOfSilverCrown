using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies.Slots;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameActions;

namespace YAGO.World.Domain.Colonies.Reforms
{
    /// <summary>
    /// Реформа
    /// </summary>
    public class Reform
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Иллюстрация
        /// </summary>
        public string Image { get; }

        /// <summary>
        /// Текст
        /// </summary>
        public string[] Text { get; }

        /// <summary>
        /// Параметры
        /// </summary>
        public IReadOnlyList<GameParameterChanging> Changes { get; }

        /// <summary>
        /// Описание
        /// </summary>
        public string[] Description { get; }

        public IReadOnlyList<GameParameterRequirement> Requirements { get; }
        public Action<ColonyState>? AdditionalCheck { get; }

        public Reform(
            long id,
            string name,
            string image,
            string[] text,
            IReadOnlyList<GameParameterChanging> changes,
            string[] description,
            IReadOnlyList<GameParameterRequirement> requirements,
            Action<ColonyState>? additionalCheck)
        {
            Id = id;
            Name = name;
            Image = image;
            Text = text;
            Changes = changes;
            Description = description;
            Requirements = requirements;
            AdditionalCheck = additionalCheck;
        }

        internal void SetReform(Colony colony, string? stringValue = null)
        {
            Check(colony.State);
            foreach (var parameter in Changes)
            {
                parameter.Apply(colony, stringValue);
            }
        }

        private void Check(ColonyState colonyState)
        {
            var actionPoints = Changes.FirstOrDefault(x => x.ParameterType == GameParameterType.ActionPointsCurrent)?.Delta ?? 0;
            if (colonyState.Resources.ActionPoints.Value < -actionPoints)
                throw new YagoException("Недостаточно очков действий.");

            var solarResservesParameter = Changes.FirstOrDefault(x => x.ParameterType == GameParameterType.SolarsCurrent)?.Delta ?? 0;
            if (colonyState.Resources.Solars.Value < -solarResservesParameter)
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = colonyState.Slots[ColonySlotType.Modules].GetFree(colonyState);
            if (zonesAvailable < -(Changes.FirstOrDefault(x => x.ParameterType == GameParameterType.ModulesUsed)?.Delta ?? 0))
                throw new YagoException("Недостаточно секторов.");

            AdditionalCheck?.Invoke(colonyState);
        }
    }
}
