using YAGO.World.Domain.Slides;
using YAGO.World.Domain.Story;

namespace YAGO.World.Host.Models.Playthroughs
{
    public class PlaythroughData
    {
        public long Id { get; }
        public long CurrentFragmentId { get; }
        public ChapterData Chapter { get; }
        public Slide[] Slides { get; }
        public int CurrentSlideIndex { get; }
        public StoryChoice[] Choices { get; }

        public PlaythroughData(
            long gameSessionId,
            long currentFragmentId,
            ChapterData chapter,
            Slide[] slides,
            int currentSlideIndex,
            StoryChoice[] choices)
        {
            Id = gameSessionId;
            CurrentFragmentId = currentFragmentId;
            Chapter = chapter;
            Slides = slides;
            CurrentSlideIndex = currentSlideIndex;
            Choices = choices;
        }
    }
}
