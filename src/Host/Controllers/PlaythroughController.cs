using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Story.Interfaces;
using YAGO.World.Host.Controllers.Models.Story;
using YAGO.World.Host.Models.Playthroughs;
using YAGO.World.Host.Models.Playthroughs.Mappings;

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
        public async Task<PlaythroughData> GetPlaythrough(CancellationToken cancellationToken)
        {
            var reslut = await _storyService.GetCurrentChapter(User, cancellationToken);
            return reslut.ToPlaythroughData();
        }

        [HttpPost("SetChoice")]
        public async Task<PlaythroughData> SetChoice(SetChoiceRequest request, CancellationToken cancellationToken)
        {
            var reslut = await _storyService.SetNextFragment(User, request.StoryNodeId, request.ChoiceNumber, cancellationToken);
            return reslut.ToPlaythroughData();
        }

        [HttpPost("DropPlaythrough")]
        public async Task<PlaythroughData> DropPlaythrough(CancellationToken cancellationToken)
        {
            var reslut = await _storyService.DropStory(User, cancellationToken);
            return reslut.ToPlaythroughData();
        }
    }
}
