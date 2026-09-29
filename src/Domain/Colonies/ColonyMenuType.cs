using System;

namespace YAGO.World.Domain.Colonies
{
    /// <summary>
    /// Разделы игры как набор битов. Хранится одним числом, поэтому
    /// переименование элементов не затрагивает сохранённые данные.
    /// </summary>
    [Flags]
    public enum ColonyMenuType
    {
        None = 0,
        Reform = 1 << 0,
        Build = 1 << 1,
        Statistics = 1 << 2,
        Council = 1 << 3,
        Wiki = 1 << 4
    }
}
