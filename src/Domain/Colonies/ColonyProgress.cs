namespace YAGO.World.Domain.Colonies
{
    public class ColonyProgress
    {
        public ColonyAchievements Achievements { get; }
        public UnlockedWikiArticles UnlockedWikiArticles { get; }
        public Council Council { get; }

        public ColonyProgress(
            ColonyAchievements achievements,
            UnlockedWikiArticles unlockedWikiArticles,
            Council council)
        {
            Achievements = achievements;
            UnlockedWikiArticles = unlockedWikiArticles;
            Council = council;
        }

        internal static ColonyProgress CreateNew()
        {
            return new ColonyProgress(
                ColonyAchievements.CreateNew(),
                UnlockedWikiArticles.CreateNew(),
                Council.CreateNew());
        }
    }
}
