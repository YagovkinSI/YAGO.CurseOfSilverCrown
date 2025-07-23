using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Story.Interfaces;
using YAGO.World.Domain.Story;
using YAGO.World.Host.Controllers.Models.Story;

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

        [HttpGet]
        [Route("getCurrentStoryNode")]
        public async Task<StoryNode> getCurrentStoryNode(CancellationToken cancellationToken) =>
            await _storyService.GetCurrentStoryNode(User, cancellationToken);

        [HttpPost("SetChoice")]
        public async Task<StoryNode> SetChoice(SetChoiceRequest request, CancellationToken cancellationToken)
        {
            return await _storyService.SetChoice(User, request.StoryNodeId, request.ChoiceNumber, cancellationToken);
        }

        [HttpPost("DropStory")]
        public async Task<StoryNode> DropStory(CancellationToken cancellationToken)
        {
            return await _storyService.DropStory(User, cancellationToken);
        }
    }
}
