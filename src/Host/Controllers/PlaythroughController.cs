using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Story.Interfaces;
using YAGO.World.Domain.Story;
using YAGO.World.Host.Controllers.Models.Story;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/playthrough")]
    public class PlaythroughController : Controller
    {
        private readonly IStoryService _storyService;

        public PlaythroughController(
            IStoryService gameService)
        {
            _storyService = gameService;
        }

        [HttpGet]
        [Route("GetPlaythrough")]
        public async Task<Playthrough> GetPlaythrough(CancellationToken cancellationToken)
        {
            return await _storyService.GetCurrentChapter(User, cancellationToken);
        }

        [HttpPost("SetChoice")]
        public async Task<Playthrough> SetChoice(SetChoiceRequest request, CancellationToken cancellationToken)
        {
            return await _storyService.SetNextFragment(User, request.StoryNodeId, request.ChoiceNumber, cancellationToken);
        }

        [HttpPost("DropPlaythrough")]
        public async Task<Playthrough> DropPlaythrough(CancellationToken cancellationToken)
        {
            return await _storyService.DropStory(User, cancellationToken);
        }
    }
}
