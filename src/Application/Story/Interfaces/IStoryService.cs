using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Story;

namespace YAGO.World.Application.Story.Interfaces
{
    public interface IStoryService
    {
        Task<CurrentChapter> GetCurrentChapter(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);
        Task<CurrentChapter> SetNextFragment(ClaimsPrincipal userClaimsPrincipal, long currentFragmentId, long nextFragmentId, CancellationToken cancellationToken);
        Task<CurrentChapter> DropStory(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);
        Task<PaginatedResponse<StoryItem>> GetStoryList(ClaimsPrincipal userClaimsPrincipal, int page, CancellationToken cancellationToken);
        Task<StoryFragment> GetStory(long gameSessionId, CancellationToken cancellationToken);
    }
}
