using System.Collections.Generic;

namespace YAGO.World.Domain.Story
{
    public class StoryDataImmutable
    {
        public Dictionary<string, bool> Events { get; }

        public StoryDataImmutable(Dictionary<string, bool> events)
        {
            Events = events;
        }

        public void SetEvent(string storyEvent, bool value)
        {
            if (Events.ContainsKey(storyEvent))
                Events[storyEvent] = value;
            else
                Events.Add(storyEvent, value);

        }
    }
}
