using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Playthroughs.Interfaces;
using YAGO.World.Host.Controllers.Models.Story;
using YAGO.World.Host.Models.Playthroughs;
using YAGO.World.Host.Models.Playthroughs.Mappings;

namespace YAGO.World.Host.Controllers
{
    [ApiController]
    [Route("api/playthrough")]
    public class PlaythroughController : Controller
    {
        private readonly IPlaythroughService _playthroughService;

        public PlaythroughController(
            IPlaythroughService playthroughService)
        {
            _playthroughService = playthroughService;
        }

        [HttpGet]
        [Route("GetPlaythrough")]
        public async Task<PlaythroughData> GetPlaythrough(CancellationToken cancellationToken)
        {
            var reslut = await _playthroughService.GetPlaythrough(User, cancellationToken);
            return reslut.ToPlaythroughData();
        }

        [HttpPost("SetChoice")]
        public async Task<PlaythroughData> SetChoice(SetChoiceRequest request, CancellationToken cancellationToken)
        {
            var reslut = await _playthroughService.SetNextFragment(User, request.StoryNodeId, request.ChoiceNumber, cancellationToken);
            return reslut.ToPlaythroughData();
        }

        [HttpPost("DropPlaythrough")]
        public async Task<PlaythroughData> DropPlaythrough(CancellationToken cancellationToken)
        {
            var reslut = await _playthroughService.DropPlaythrough(User, cancellationToken);
            return reslut.ToPlaythroughData();
        }
    }
}
