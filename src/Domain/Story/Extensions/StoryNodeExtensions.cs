namespace YAGO.World.Domain.Story.Extensions
{
    public static class StoryNodeExtensions
    {
        public static StoryNode RemoveResults(this StoryNodeWithResults storyNodeWithResults)
        {
            return new StoryNode(
                storyNodeWithResults.Id,
                storyNodeWithResults.Title,
                storyNodeWithResults.Cards,
                storyNodeWithResults.Choices
            );
        }
    }
}
