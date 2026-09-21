namespace YAGO.World.Domain.Colonies.Reforms
{
    /// <summary>
    /// Тип свода законов колонии
    /// </summary>
    public enum LawsType
    {
        Standard,
        Corporate,
        Humanist
    }

    public static class LawsTypeExtensions
    {
        public static string GetDisplayName(this LawsType type)
        {
            return type switch
            {
                LawsType.Humanist => "Гуманные",
                LawsType.Corporate => "Корпоративные",
                _ => "Стандартные",
            };
        }
    }
}