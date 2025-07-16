using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Story;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IStoryRepository
    {
        Task<StoryNode> GetCurrentStoryNode(long storyId, CancellationToken cancellationToken);
    }
}
