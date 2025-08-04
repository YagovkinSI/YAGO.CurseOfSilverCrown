using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Story.Interfaces;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Exceptions;
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
    }
}
