using Newtonsoft.Json;
using YAGO.World.Domain.Story;
using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Infrastructure.Database.Models.StoryDatas.Extensions
{
    public static class StoryDataExtensions
    {
        public static Domain.Stories.Story ToDomain(this StoryData source)
        {
            var storyDataImmutable = JsonConvert.DeserializeObject<StoryDataImmutable>(source.StoryDataJson);

            return new Domain.Stories.Story
            (
                source.Id,
                source.UserId,
                storyDataImmutable
            );
        }

        public static Domain.Story.StoryItem ToStoryItem(this StoryData source)
        {
            return new Domain.Story.StoryItem
            (
                source.Id,
                source.User.ToYagoEntity(),
                source.ToYagoEntity(),
                0, //TODO
                "Обычное поручение" //TODO
            );
        }

        public static Domain.YagoEntities.YagoEntity ToYagoEntity(this StoryData source)
        {
            return new Domain.YagoEntities.YagoEntity
            (
                source.Id,
                Domain.YagoEntities.Enums.YagoEntityType.GameSession,
                source.Name
            );
        }
    }
}
