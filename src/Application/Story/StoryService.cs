using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Application.Story.Interfaces;
using YAGO.World.Domain.Story;

namespace YAGO.World.Application.Story
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;

        public StoryService(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public Task<StoryNode> GetCurrentStoryNode(ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            return _storyRepository.GetCurrentStoryNode(0, cancellationToken);
        }
    }
}
