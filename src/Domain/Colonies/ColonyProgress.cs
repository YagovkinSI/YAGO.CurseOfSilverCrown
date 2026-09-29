using YAGO.World.Domain.Colonies.Councils;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyProgress
    {
        public ColonyAchievements Achievements { get; }
        public UnlockedWikiArticles UnlockedWikiArticles { get; }
        public Council Council { get; }
        public ColonyMenus Menus { get; }

        public ColonyProgress(
            ColonyAchievements achievements,
            UnlockedWikiArticles unlockedWikiArticles,
            Council council,
            ColonyMenus menus)
        {
            Achievements = achievements;
            UnlockedWikiArticles = unlockedWikiArticles;
            Council = council;
            Menus = menus;
        }

        internal static ColonyProgress CreateNew()
        {
            return new ColonyProgress(
                ColonyAchievements.CreateNew(),
                UnlockedWikiArticles.CreateNew(),
                Council.CreateNew(),
                ColonyMenus.CreateNew());
        }
    }
}
