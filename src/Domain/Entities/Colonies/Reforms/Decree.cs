using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies.Resources;
using YAGO.World.Domain.Entities.Colonies.Slots;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Services;

namespace YAGO.World.Domain.Entities.Colonies.Reforms
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
        public IReadOnlyList<KeyValueParameter> Parameters { get; }

        /// <summary>
        /// Описание
        /// </summary>
        public string[] Description { get; }

        public IReadOnlyList<RequirementsParameter> Requirements { get; }
        public Action<ColonyState>? AdditionalCheck { get; }

        public Reform(
            long id,
            string name,
            string image,
            string[] text,
            IReadOnlyList<KeyValueParameter> parameters,
            string[] description,
            IReadOnlyList<RequirementsParameter> requirements,
            Action<ColonyState>? additionalCheck)
        {
            Id = id;
            Name = name;
            Image = image;
            Text = text;
            Parameters = parameters;
            Description = description;
            Requirements = requirements;
            AdditionalCheck = additionalCheck;
        }

        internal void SetReform(ColonyState colonyState)
        {
            Check(colonyState);
            foreach (var parameter in Parameters)
            {
                colonyState.AddParameter(parameter.Name, parameter.Value);
            }
        }

        private void Check(ColonyState colonyState)
        {
            var actionPoints = Parameters.FirstOrDefault(x => x.Name == StateKey.ActionPointsCurrent)?.Value ?? 0;
            if (colonyState.Resources[ColonyResourceType.ActionPoints].Value < -actionPoints)
                throw new YagoException("Недостаточно очков действий.");

            var solarResservesParameter = Parameters.FirstOrDefault(x => x.Name == StateKey.SolarsCurrent)?.Value ?? 0;
            if (colonyState.Resources[ColonyResourceType.Solars].Value < -solarResservesParameter)
                throw new YagoException("Недостаточно средств.");

            var zonesAvailable = colonyState.Slots[ColonySlotType.Modules].GetFree(colonyState);
            if (zonesAvailable < -(Parameters.FirstOrDefault(x => x.Name == StateKey.ModulesUsed)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");

            AdditionalCheck?.Invoke(colonyState);
        }
    }
}
