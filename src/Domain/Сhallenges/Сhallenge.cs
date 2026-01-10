using System;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Notifications;

namespace YAGO.World.Domain.Сhallenges
{
    public class Сhallenge
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Иллюстрация
        /// </summary>
        public string Image { get; }

        /// <summary>
        /// Текстовое описание
        /// </summary>
        public string[] Text { get; }

        /// <summary>
        /// Вероятность возникновения (от 0 до 1)
        /// </summary>
        public double ChanceDefault { get; }

        /// <summary>
        /// Изменение количества соларов
        /// </summary>
        public int SolarChange { get; }

        public Сhallenge(
            long id,
            string title,
            string image,
            string[] text,
            double chanceDefault,
            int solarChange)
        {
            Id = id;
            Title = title;
            Image = image;
            Text = text;
            ChanceDefault = chanceDefault;
            SolarChange = solarChange;
        }

        public bool Check()
        {
            var random = new Random();
            if (random.NextDouble() < ChanceDefault)
                return true;

            return false;
        }

        public Notification ToNotification()
        {
            var colonyParameter = new ColonyParameter(ColonyParameterType.Solars, SolarChange);

            return new Notification(Title, Image, Text, [colonyParameter]);
        }
    }
}
