using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.Dtos;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Application.Story.Interfaces;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Story;

namespace YAGO.World.Application.Story
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly ICurrentUserService _currentUserService;

        public StoryService(
            IStoryRepository storyRepository,
            ICurrentUserService currentUserService)
        {
            _storyRepository = storyRepository;
            _currentUserService = currentUserService;
        }
        
        public async Task<PaginatedResponse<StoryItem>> GetStoryList(ClaimsPrincipal userClaimsPrincipal, int page, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetCurrentUser(userClaimsPrincipal, cancellationToken);
            var userId = currentUser?.Id;

            return await _storyRepository.GetStoryList(userId, page, cancellationToken);
        }

        public Task<StoryFragment> GetStory(long gameSessionId, CancellationToken cancellationToken)
        {
            return _storyRepository.GetStory(gameSessionId, cancellationToken);
        }
    }
}
