using System;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyName
    {
        /// <summary>
        /// Название в БД
        /// </summary>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// Получила ли колония название
        /// </summary>
        public bool Named { get; private set; }

        /// <summary>
        /// Отображаемое название
        /// </summary>
        public string DisplayName => Named ? DatabaseName : "Акционер";

        public ColonyName(string name, bool named)
        {
            DatabaseName = name;
            Named = named;
        }

        public static ColonyName CreateNew()
        {
            var random = new Random();
            var name = $"Колония {random.Next(100000, 999999)}";

            var colonyName = new ColonyName(
                name: name,
                named: false);
            return colonyName;
        }

        public void SetName(string name)
        {
            Named = true;
            DatabaseName = name;
        }
    }
}
