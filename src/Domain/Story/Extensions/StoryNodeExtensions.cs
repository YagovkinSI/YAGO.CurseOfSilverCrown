namespace YAGO.World.Domain.Story.Extensions
{
    public static class StoryNodeExtensions
    {
        public static StoryNode ToStoryNode(this Fragment storyNodeWithResults)
        {
            return new StoryNode(
                storyNodeWithResults.Id,
                storyNodeWithResults.Title,
                storyNodeWithResults.Slides,
                storyNodeWithResults.Choices
            );
        }
    }
}
