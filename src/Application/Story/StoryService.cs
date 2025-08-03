using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Application.Story.Interfaces;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Exceptions;
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

        public async Task<CurrentChapter> GetCurrentChapter(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var authorizationData = await _currentUserService.GetAuthorizationData(userClaimsPrincipal, cancellationToken);
            return !authorizationData.IsAuthorized
                ? throw new YagoNotAuthorizedException()
                : await _storyRepository.GetCurrentChapter(authorizationData.User!.Id, cancellationToken);
        }

        public async Task<CurrentChapter> SetNextFragment(ClaimsPrincipal userClaimsPrincipal, long currentFragmentId, long nextFragmentId, CancellationToken cancellationToken)
        {
            var authorizationData = await _currentUserService.GetAuthorizationData(userClaimsPrincipal, cancellationToken);
            if (!authorizationData.IsAuthorized)
                throw new YagoNotAuthorizedException();

            var user = authorizationData.User!;
            var currentFragment = await _storyRepository.GetCurrentFragment(user.Id, cancellationToken);
            if (currentFragment.Id != currentFragmentId)
                throw new YagoException("Ошибка определения событий текущей игровой сессии.");

            var hasNextFragment = currentFragment.NextFragmentIds.Contains(nextFragmentId);
            if (!hasNextFragment)
                throw new YagoException("Ошибка определения выбора по текущему событию.");

            var currentStoryData = await _storyRepository.GetCurrentStoryData(user.Id, cancellationToken);
            currentStoryData.Data.AddFragment(nextFragmentId);

            return await _storyRepository.UpdateStory(user.Id, currentStoryData, cancellationToken);
        }

        public async Task<CurrentChapter> DropStory(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var authorizationData = await _currentUserService.GetAuthorizationData(userClaimsPrincipal, cancellationToken);
            if (!authorizationData.IsAuthorized)
                throw new YagoNotAuthorizedException();

            var user = authorizationData.User!;
            await _storyRepository.DropStory(user.Id, cancellationToken);

            return await _storyRepository.GetCurrentChapter(user!.Id, cancellationToken);
        }

        public async Task<PaginatedResponse<StoryItem>> GetStoryList(ClaimsPrincipal userClaimsPrincipal, int page, CancellationToken cancellationToken)
        {
            var authorizationData = await _currentUserService.GetAuthorizationData(userClaimsPrincipal, cancellationToken);
            var userId = authorizationData.User?.Id;

            return await _storyRepository.GetStoryList(userId, page, cancellationToken);
        }

        public Task<StoryFragment> GetStory(long gameSessionId, CancellationToken cancellationToken)
        {
            return _storyRepository.GetStory(gameSessionId, cancellationToken);
        }
    }
}
