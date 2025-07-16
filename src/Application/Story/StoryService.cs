using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUser;
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
            var user = await _currentUserService.FindCurrentUser(userClaimsPrincipal);
            if (user == null)
                throw new YagoNotAuthorizedException();

            return await _storyRepository.GetCurrentStoryNode(user.Id, cancellationToken);
        }
    }
}
