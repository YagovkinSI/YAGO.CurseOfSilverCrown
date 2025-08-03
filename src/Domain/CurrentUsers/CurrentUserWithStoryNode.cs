using YAGO.World.Domain.Story;

namespace YAGO.World.Domain.CurrentUsers
{
    public class CurrentUserWithStoryNode
    {
        public CurrentUser User { get; }
        public CurrentChapter StoryNode { get; }

        public CurrentUserWithStoryNode(
            CurrentUser user,
            CurrentChapter storyNode)
        {
            User = user;
            StoryNode = storyNode;
        }
    }
}
