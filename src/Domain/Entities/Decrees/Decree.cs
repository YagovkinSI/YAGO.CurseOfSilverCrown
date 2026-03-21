using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Decrees
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
    }
}
