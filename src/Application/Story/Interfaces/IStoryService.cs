using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Story;

namespace YAGO.World.Application.Story.Interfaces
{
    public interface IStoryService
    {
        Task<StoryNode> GetCurrentStoryNode(ClaimsPrincipal user, CancellationToken cancellationToken);
    }
}
