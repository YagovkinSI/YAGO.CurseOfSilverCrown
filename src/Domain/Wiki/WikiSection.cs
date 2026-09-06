using System.Collections.Generic;

namespace YAGO.World.Domain.Wiki
{
    public static class WikiSection
    {
        public const string Station = "station";
        public const string Life = "life";
        public const string Faction = "faction";
        public const string Characters = "characters";
        public const string Gameplay = "gameplay";
        public const string History = "history";

        public record SectionInfo(string Code, string Name, int Order);

        public static IReadOnlyList<SectionInfo> All =>
        [
            new(Station, "Станции", 1),
            new(Life, "Жизнь в Поясе", 2),
            new(Faction, "Фракции", 3),
            new(Characters, "Персонажи", 4),
            new(Gameplay, "Параметры", 5),
            new(History, "История", 6)
        ];
    }
}
