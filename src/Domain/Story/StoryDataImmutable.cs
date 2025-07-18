using System.Collections.Generic;

namespace YAGO.World.Domain.Story
{
    public class StoryDataImmutable
    {
        public Dictionary<string, int> PersonalOpinions { get; private set; }
        public Dictionary<string, bool> Events { get; private set; }

        public StoryDataImmutable(
            Dictionary<string, int> personalOpinions,
            Dictionary<string, bool> events)
        {
            PersonalOpinions = personalOpinions ?? new Dictionary<string, int>();
            Events = events ?? new Dictionary<string, bool>();
        }

        public static StoryDataImmutable Empty =>
            new StoryDataImmutable(
                personalOpinions: new Dictionary<string, int>(),
                events: new Dictionary<string, bool>()
            );

        public void SetEvent(string storyEvent, bool value)
        {
            if (Events == null)
                Events = new Dictionary<string, bool>();

            if (Events.ContainsKey(storyEvent))
                Events[storyEvent] = value;
            else
                Events.Add(storyEvent, value);

        }

        public void ChangePersonalOpinions(string opinion, int value)
        {
            if (PersonalOpinions == null)
                PersonalOpinions = new Dictionary<string, int>();

            if (PersonalOpinions.ContainsKey(opinion))
                PersonalOpinions[opinion] += value;
            else
                PersonalOpinions.Add(opinion, value);

            if (PersonalOpinions[opinion] > 100)
                PersonalOpinions[opinion] = 100;

            if (PersonalOpinions[opinion] < -100)
                PersonalOpinions[opinion] = -100;

        }
    }
}
