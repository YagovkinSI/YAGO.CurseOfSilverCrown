using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Story;

namespace YAGO.World.Application.Story.Interfaces
{
    public interface IStoryService
    {
        Task<StoryNode> GetCurrentStoryNode(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken);
        Task<StoryNode> SetChoice(ClaimsPrincipal userClaimsPrincipal, long storyNodeId, int choiceNumber, CancellationToken cancellationToken);
    }
}
