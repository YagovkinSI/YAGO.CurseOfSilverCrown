using YAGO.World.Domain.YagoEntities;

namespace YAGO.World.Domain.Story
{
    public class StoryItem
    {
        public long Id { get; }
        public YagoEntity? User { get; }
        public YagoEntity? GameSession { get; }
        public int Chapter { get; }
        public string Title { get; }

        public StoryItem(
            long id, 
            YagoEntity? user, 
            YagoEntity? gameSession, 
            int chapter, 
            string title)
        {
            Id = id;
            User = user;
            GameSession = gameSession;
            Chapter = chapter;
            Title = title;
        }
    }
}
