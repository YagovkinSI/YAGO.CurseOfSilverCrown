using YAGO.World.Application.Dtos;

namespace YAGO.World.Host.Models.Playthroughs.Mappings
{
    public static class PlaythroughDataMapping
    {
        public static PlaythroughData ToPlaythroughData(this Playthrough source)
        {
            var chapterData = new ChapterData(
                source.ChapterNumber,
                source.Title
            );

            return new PlaythroughData(
                source.GameSessionId,
                source.CurrentFragmentId,
                chapterData,
                source.Slides,
                source.CurrentSlideIndex,
                source.Choices
            );
        }
    }
}
