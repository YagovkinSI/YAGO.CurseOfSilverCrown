using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Domain.Decrees
{
    /// <summary>
    /// ОТряд или юнит
    /// </summary>
    public class Decree
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

        public Decree(
            long id,
            string name,
            string image,
            string[] text,
            IReadOnlyList<KeyValueParameter> parameters,
            string[] description)
        {
            Id = id;
            Name = name;
            Image = image;
            Text = text;
            Parameters = parameters;
            Description = description;
        }

        public void IssueDecree(Colony colony,
            Ship ship,
            ColonyCompanies companies)
        {
            colony.ValidateShip(ship);
            colony.ValidateContracts(companies);

            if (colony.Solars < (Parameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Economic_Reserves)?.Value ?? 0))
                throw new YagoException("Недостаточно средств.");

            var areaCapacity = new AreaCapacity(colony, companies, ship);
            if (ship.Zones - areaCapacity.Occupied < (Parameters.FirstOrDefault(x => x.Name == ColonyParameterNames.AreaCapacity_Occupied)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");

            colony.AddSolars(Parameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Economic_Reserves)?.Value ?? 0);
            colony.AddFestivalEffect(Parameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Mood_Total)?.Value ?? 0);
        }
    }
}
