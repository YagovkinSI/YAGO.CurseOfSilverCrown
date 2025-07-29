using YAGO.World.Domain.YagoEntities;

namespace YAGO.World.Domain.Story
{
    public class StoryFragment
    {
        public YagoEntity User { get; }
        public YagoEntity GameSession { get; }
        public string Title { get; }
        public int Chapter { get; }
        public StoryCard[] Cards { get; }

        public StoryFragment(
            YagoEntity user,
            YagoEntity gameSession,
            string title,
            int chapter,
            StoryCard[] cards)
        {
            User = user;
            GameSession = gameSession;
            Title = title;
            Chapter = chapter;
            Cards = cards;
        }
    }
}
