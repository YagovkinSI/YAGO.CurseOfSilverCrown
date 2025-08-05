using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.Dtos;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Application.Playthroughs.Interfaces;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Playthroughs
{
    public class PlaythroughService : IPlaythroughService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly ICurrentUserService _currentUserService;

        public PlaythroughService(
            IStoryRepository storyRepository,
            ICurrentUserService currentUserService)
        {
            _storyRepository = storyRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Playthrough> GetPlaythrough(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetCurrentUser(userClaimsPrincipal, cancellationToken);
            return currentUser == null
                ? throw new YagoNotAuthorizedException()
                : await _storyRepository.GetCurrentChapter(currentUser!.Id, cancellationToken);
        }

        public async Task<Playthrough> SetNextFragment(ClaimsPrincipal userClaimsPrincipal, long currentFragmentId, long nextFragmentId, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetCurrentUser(userClaimsPrincipal, cancellationToken);
            if (currentUser == null)
                throw new YagoNotAuthorizedException();

            var currentFragment = await _storyRepository.GetCurrentFragment(currentUser.Id, cancellationToken);
            if (currentFragment.Id != currentFragmentId)
                throw new YagoException("Ошибка определения событий текущей игровой сессии.");

            var hasNextFragment = currentFragment.NextFragmentIds.Contains(nextFragmentId);
            if (!hasNextFragment)
                throw new YagoException("Ошибка определения выбора по текущему событию.");

            var currentStoryData = await _storyRepository.GetCurrentStoryData(currentUser.Id, cancellationToken);
            currentStoryData.LastStoryChapter.AddFragment(nextFragmentId);

            return await _storyRepository.UpdateStory(currentUser.Id, currentStoryData, cancellationToken);
        }

        public async Task<Playthrough> DropPlaythrough(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetCurrentUser(userClaimsPrincipal, cancellationToken);
            if (currentUser == null)
                throw new YagoNotAuthorizedException();

            await _storyRepository.DropStory(currentUser.Id, cancellationToken);

            return await _storyRepository.GetCurrentChapter(currentUser!.Id, cancellationToken);
        }
    }
}
