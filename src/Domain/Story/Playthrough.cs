using YAGO.World.Domain.Slides;

namespace YAGO.World.Domain.Story
{
    public class Playthrough
    {
        public long GameSessionId { get; }
        public long CurrentFragmentId { get; }
        public int ChapterNumber { get; }
        public string Title { get; }
        public Slide[] Slides { get; }
        public int CurrentSlideIndex { get; }
        public StoryChoice[] Choices { get; }

        public Playthrough(
            long gameSessionId,
            long currentFragmentId,
            int chapterNumber,
            string title,
            Slide[] slides,
            int currentSlideIndex,
            StoryChoice[] choices)
        {
            GameSessionId = gameSessionId;
            CurrentFragmentId = currentFragmentId;
            ChapterNumber = chapterNumber;
            Title = title;
            Slides = slides;
            CurrentSlideIndex = currentSlideIndex;
            Choices = choices;
        }
    }
}
