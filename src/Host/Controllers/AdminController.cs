using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IStoryRepository _storyRepository;

        public AdminController(
            ICurrentUserService currentUserService,
            IStoryRepository storyRepository)
        {
            _currentUserService = currentUserService;
            _storyRepository = storyRepository;
        }

        public async Task WipeStory(CancellationToken cancellationToken)
        {
            var authorizationData = await _currentUserService.GetAuthorizationData(User, cancellationToken);
            if (!authorizationData.IsAuthorized)
                throw new YagoNotAuthorizedException();

            var user = authorizationData.User!;
            var storyData = await _storyRepository.GetCurrentStoryData(user.Id, cancellationToken);
            storyData.SetStoreNodeId(0);
            _ = await _storyRepository.UpdateStoryNode(user.Id, storyData, cancellationToken);
        }
    }
}
