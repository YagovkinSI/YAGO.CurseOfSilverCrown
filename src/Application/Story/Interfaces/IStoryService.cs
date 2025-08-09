using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Dtos;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Story;

namespace YAGO.World.Application.Story.Interfaces
{
    public interface IStoryService
    {
        Task<PaginatedResponse<StoryItem>> GetStoryList(ClaimsPrincipal userClaimsPrincipal, int page, CancellationToken cancellationToken);
        Task<StoryFragment> GetStory(long gameSessionId, CancellationToken cancellationToken);
    }
}
