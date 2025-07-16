namespace YAGO.World.Domain.Story
{
    public class StoryData
    {
        public long StoreNodeId { get; private set; }
        public StoryDataImmutable Data { get; }

        public StoryData(long storeNodeId, StoryDataImmutable data)
        {
            StoreNodeId = storeNodeId;
            Data = data;
        }

        public void SetStoreNodeId(long storeNodeId)
        {
            StoreNodeId = storeNodeId;
        }
    }
}
