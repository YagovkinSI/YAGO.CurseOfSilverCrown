using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Story;

namespace YAGO.World.Application.InfrastructureInterfaces.Repositories
{
    public interface IStoryRepository
    {
        
        Task<StoryDataImmutable> GetCurrentStoryData(long userId, CancellationToken cancellationToken);
        Task<StoryNode> GetCurrentStoryNode(long userId, CancellationToken cancellationToken);
        Task<StoryNodeWithResults> GetCurrentStoryNodeWithResults(long userId, CancellationToken cancellationToken);
        Task<StoryNode> UpdateStoryNode(long userId, StoryDataImmutable storyData, CancellationToken cancellationToken);
    }
}
