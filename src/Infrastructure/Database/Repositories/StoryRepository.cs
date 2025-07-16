using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Story;
using YAGO.World.Infrastructure.Database.Resources;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        public Task<StoryNode> GetCurrentStoryNode(long storyId, CancellationToken cancellationToken)
        {
            return Task.FromResult(StoryDatabase.Nodes[0]);
        }
    }
}
