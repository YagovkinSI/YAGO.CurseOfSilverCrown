using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.CurrentUser;
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
            var user = await _currentUserService.FindCurrentUser(User);
            if (user == null)
                throw new YagoNotAuthorizedException();

            var storyData = await _storyRepository.GetCurrentStoryData(user.Id, cancellationToken);
            storyData.SetStoreNodeId(0);
            _ = await _storyRepository.UpdateStoryNode(user.Id, storyData, cancellationToken);
        }
    }
}
