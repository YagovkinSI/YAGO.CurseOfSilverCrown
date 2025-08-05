using YAGO.World.Domain.Story;

namespace YAGO.World.Domain.Stories
{
    public class Story
    {
        public long Id { get; }
        public long UserId { get; }
        public StoryDataImmutable Data { get; }

        public Story(
            long id,
            long userId, 
            StoryDataImmutable data)
        {
            Id = id;
            UserId = userId;
            Data = data;
        }
    }
}
