using System;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Domain.Colonies
{
    /// <summary>
    /// Доступность разделов игры. Открывается эффектами квестов,
    /// только накапливается и не закрывается.
    /// </summary>
    public class ColonyMenus
    {
        public ColonyMenuType Opened { get; private set; }

        public ColonyMenus(ColonyMenuType opened)
        {
            Opened = opened;
        }

        internal static ColonyMenus CreateNew()
        {
            return new ColonyMenus(ColonyMenuType.None);
        }

        public bool IsAvailable(ColonyMenuType menu) => (Opened & menu) != 0;

        public void Open(string menuCode)
        {
            Opened |= ParseMenu(menuCode);
        }

        private static ColonyMenuType ParseMenu(string menuCode)
        {
            if (Enum.TryParse<ColonyMenuType>(menuCode, out var menu)
                && menu != ColonyMenuType.None
                && Enum.IsDefined(menu))
                return menu;
            throw new YagoException($"Неизвестное меню: {menuCode}.");
        }
    }
}
