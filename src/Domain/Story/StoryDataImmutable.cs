using System.Collections.Generic;

namespace YAGO.World.Domain.Story
{
    public class StoryDataImmutable
    {
        public long StoreNodeId { get; private set; }
        public Dictionary<string, bool> Events { get; }

        public StoryDataImmutable(long storeNodeId, Dictionary<string, bool> events)
        {
            StoreNodeId = storeNodeId;
            Events = events;
        }

        public void SetEvent(string storyEvent, bool value)
        {
            if (Events.ContainsKey(storyEvent))
                Events[storyEvent] = value;
            else
                Events.Add(storyEvent, value);

        }
        public void SetStoreNodeId(long storeNodeId)
        {
            StoreNodeId = storeNodeId;
        }
    }
}
