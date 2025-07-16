using Newtonsoft.Json;
using YAGO.World.Domain.Story;

namespace YAGO.World.Infrastructure.Database.Models.StoryDatas.Extensions
{
    public static class StoryDataExtensions
    {
        public static Domain.Story.StoryData ToDomain(this StoryData storyData)
        {
            var storyDataImmutable = JsonConvert.DeserializeObject<StoryDataImmutable>(storyData.StoryDataJson);

            return new Domain.Story.StoryData
            (
                storyData.CurrentStoryNodeId,
                storyDataImmutable
            );
        }
    }
}
