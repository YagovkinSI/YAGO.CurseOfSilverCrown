using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Application.Story.Interfaces;
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

        public async Task<StoryNode> GetCurrentStoryNode(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var authorizationData = await _currentUserService.GetAuthorizationData(userClaimsPrincipal, cancellationToken);
            return !authorizationData.IsAuthorized
                ? throw new YagoNotAuthorizedException()
                : await _storyRepository.GetCurrentStoryNode(authorizationData.User!.Id, cancellationToken);
        }

        public async Task<StoryNode> SetChoice(ClaimsPrincipal userClaimsPrincipal, long storyNodeId, int choiceNumber, CancellationToken cancellationToken)
        {
            var authorizationData = await _currentUserService.GetAuthorizationData(userClaimsPrincipal, cancellationToken);
            if (!authorizationData.IsAuthorized)
                throw new YagoNotAuthorizedException();

            var user = authorizationData.User!;
            var currentStoryNode = await _storyRepository.GetCurrentStoryNodeWithResults(user.Id, cancellationToken);
            if (currentStoryNode.Id != storyNodeId)
                throw new YagoException("Ошибка определения событий текущей игровой сессии.");

            var choice = currentStoryNode.Choices.FirstOrDefault(c => c.Number == choiceNumber);
            if (choice == null)
                throw new YagoException("Ошибка определения выбора по текущему событию.");

            var currentStoryData = await _storyRepository.GetCurrentStoryData(user.Id, cancellationToken);
            choice.Action(currentStoryData);
            currentStoryData.SetStoreNodeId(choice.NextStoreNodeId);

            return await _storyRepository.UpdateStoryNode(user.Id, currentStoryData, cancellationToken);
        }
    }
}
