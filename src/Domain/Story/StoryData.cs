namespace YAGO.World.Domain.Story
{
    public class StoryData
    {
        public long GameSessionId { get; }
        public StoryDataImmutable Data { get; }

        public StoryData(long gameSessionId, StoryDataImmutable data)
        {
            GameSessionId = gameSessionId;
            Data = data;
        }
    }
}
