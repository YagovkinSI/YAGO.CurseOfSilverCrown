namespace YAGO.World.Domain.Stories
{
    public class Story
    {
        public long Id { get; }
        public long UserId { get; }
        public StoryChapter[] StoryChapters { get; }

        public StoryChapter LastStoryChapter => StoryChapters[^1];

        public Story(
            long id,
            long userId,
            StoryChapter[] storyChapters)
        {
            Id = id;
            UserId = userId;
            StoryChapters = storyChapters;
        }
    }
}
