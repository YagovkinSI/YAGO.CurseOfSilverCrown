using System;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyDisplayInfo
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

        public ColonyDisplayInfo(string name, bool named)
        {
            DatabaseName = name;
            Named = named;
        }

        public static ColonyDisplayInfo CreateNew()
        {
            var random = new Random();
            var name = $"Колония {random.Next(100000, 999999)}";

            var colonyName = new ColonyDisplayInfo(
                name: name,
                named: false);
            return colonyName;
        }

        public void SetName(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return;
            Named = true;
            DatabaseName = name;
        }
    }
}
