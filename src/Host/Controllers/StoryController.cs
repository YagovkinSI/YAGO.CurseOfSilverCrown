using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Story.Interfaces;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Exceptions;
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
        [Route("GetCurrentFragment")]
        public async Task<StoryNode> GetCurrentFragment(CancellationToken cancellationToken)
        {
            return await _storyService.GetCurrentFragment(User, cancellationToken);
        }

        [HttpGet]
        [Route("getStoryList")]
        public Task<PaginatedResponse<StoryItem>> GetStoryList(int? page, CancellationToken cancellationToken)
        {
            return _storyService.GetStoryList(User, page ?? 1, cancellationToken);
        }

        [HttpGet]
        [Route("getStory")]
        public Task<StoryFragment> GetStory(long? id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new YagoException("не указан идентификатор истории");

            return _storyService.GetStory(id.Value, cancellationToken);
        }

        [HttpPost("SetChoice")]
        public async Task<StoryNode> SetChoice(SetChoiceRequest request, CancellationToken cancellationToken)
        {
            return await _storyService.SetNextFragment(User, request.StoryNodeId, request.ChoiceNumber, cancellationToken);
        }

        [HttpPost("DropStory")]
        public async Task<StoryNode> DropStory(CancellationToken cancellationToken)
        {
            return await _storyService.DropStory(User, cancellationToken);
        }
    }
}
