using Newtonsoft.Json;
using YAGO.World.Domain.Stories;
using YAGO.World.Domain.Story;
using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Infrastructure.Database.Models.StoryDatas.Extensions
{
    public static class StoryDataExtensions
    {
        public static Story ToDomain(this StoryData source)
        {
            var storyDataImmutable = JsonConvert.DeserializeObject<StoryDataImmutable>(source.StoryDataJson);

            var storyChapter = new StoryChapter(source.Id, source.Id, 1, storyDataImmutable.FragmentIds.ToArray());

            return new Story
            (
                source.Id,
                source.UserId,
                new StoryChapter[] { storyChapter }
            );
        }

        public static StoryItem ToStoryItem(this StoryData source)
        {
            return new StoryItem
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
