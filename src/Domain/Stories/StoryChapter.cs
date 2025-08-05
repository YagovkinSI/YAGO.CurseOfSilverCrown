using System.Linq;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Stories
{
    public class StoryChapter
    {
        public long Id { get; }
        public long StoryId { get; }
        public long ChapterId { get; }
        public long[] FragmentIds { get; private set; }

        public StoryChapter(
            long id,
            long storyId,
            long chapterId,
            long[] fragmentIds)
        {
            Id = id;
            StoryId = storyId;
            ChapterId = chapterId;
            FragmentIds = fragmentIds;
        }

        public void AddFragment(long nextFragmentId)
        {
            if (FragmentIds[^1] == nextFragmentId)
                return;

            if (FragmentIds.Contains(nextFragmentId))
                throw new YagoException("История уже содержит следующий фрагмент. Обратитесь к разработчику!");

            var fragments = FragmentIds.ToList();
            fragments.Add(nextFragmentId);
            FragmentIds = fragments.ToArray();
        }
    }
}
