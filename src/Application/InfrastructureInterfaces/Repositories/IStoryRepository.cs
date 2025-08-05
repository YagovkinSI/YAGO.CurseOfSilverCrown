using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Fragments;
using YAGO.World.Domain.Stories;
using YAGO.World.Domain.Story;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IStoryRepository
    {
        Task<Domain.Stories.Story> GetCurrentStoryData(long userId, CancellationToken cancellationToken);
        Task<Playthrough> GetCurrentChapter(long userId, CancellationToken cancellationToken);
        Task<Fragment> GetCurrentFragment(long userId, CancellationToken cancellationToken);
        Task<Playthrough> UpdateStory(long userId, Domain.Stories.Story storyData, CancellationToken cancellationToken);
        Task DropStory(long userId, CancellationToken cancellationToken);
        Task<PaginatedResponse<StoryItem>> GetStoryList(long? userId, int page, CancellationToken cancellationToken);
        Task<StoryFragment> GetStory(long gameSessionId, CancellationToken cancellationToken);
    }
}
