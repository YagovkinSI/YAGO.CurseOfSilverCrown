using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Story.Interfaces;
using YAGO.World.Domain.Story;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/story")]
    public class StoryController : Controller
    {
        private readonly IStoryService _storyService;

        public StoryController(
            IStoryService gameService)
        {
            _storyService = gameService;
        }

        public async Task<StoryNode> Index(CancellationToken cancellationToken) =>
            await _storyService.GetCurrentStoryNode(User, cancellationToken);

        public async Task<StoryNode> SetChoice(long storyNodeId, int choiceNumber, CancellationToken cancellationToken) =>
            await _storyService.SetChoice(User, storyNodeId, choiceNumber, cancellationToken);
    }
}
