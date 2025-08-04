using YAGO.World.Domain.Story;

namespace YAGO.World.Host.Models.Playthroughs.Mappings
{
    public static class PlaythroughDataMapping
    {
        public static PlaythroughData ToPlaythroughData(this Playthrough source)
        {
            return new PlaythroughData(
                source.GameSessionId,
                source.CurrentFragmentId,
                source.ChapterNumber,
                source.Title,
                source.Slides,
                source.CurrentSlideIndex,
                source.Choices
            );
        }
    }
}
