using YAGO.World.Domain.CurrentUsers;
using YAGO.World.Domain.Story.Extensions;
using YAGO.World.Infrastructure.Database.Resources;

namespace YAGO.World.Infrastructure.Database.Models.Users
{
    internal static class CurrentUserWithStoryNodeExtensions
    {
        public static CurrentUserWithStoryNode ToCurrentUserWithStoryNode(this User source)
        {
            var user = source.ToDomainCurrentUser();
            var storyNode = StoryDatabase.Fragments[source.StoryDatas[0].CurrentStoryNodeId].ToStoryNode();

            return new CurrentUserWithStoryNode(
                user,
                storyNode
            );
        }
    }
}
