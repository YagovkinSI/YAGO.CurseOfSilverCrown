using YAGO.World.Domain.Story;

namespace YAGO.World.Domain.CurrentUsers
{
    public class CurrentUserWithStoryNode
    {
        public CurrentUser User { get; }
        public StoryNode StoryNode { get; }

        public CurrentUserWithStoryNode(
            CurrentUser user,
            StoryNode storyNode)
        {
            User = user;
            StoryNode = storyNode;
        }
    }
}
