using System.Linq;

namespace YAGO.World.Domain.Story.Extensions
{
    public static class StoryNodeExtensions
    {
        public static StoryNode ToStoryNode(this Fragment storyNodeWithResults, Fragment[] choiceFragments)
        {
            var choices = choiceFragments
                .Select(f => new StoryChoice(f.Id, f.ChoiceText))
                .ToArray();

            return new StoryNode(
                storyNodeWithResults.Id,
                "Обычное поручение",
                storyNodeWithResults.Slides,
                choices
            );
        }
    }
}
