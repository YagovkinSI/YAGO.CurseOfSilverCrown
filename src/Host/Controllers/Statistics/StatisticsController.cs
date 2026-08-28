using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Statistics.Queries;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Host.Controllers.Common.Extensions;

namespace YAGO.World.Host.Controllers.Statistics
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [Route("getStatistics")]
        public async Task<StatisticsResponse> GetStatistics(string code, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var query = code switch
            {
                StatisticCodeConstants.Main => (IRequest<StatisticsResult>)new GetMainStatisticsQuery(userId),
                StatisticCodeConstants.SolarDelta => new GetSolarDeltaStatisticsQuery(userId),
                _ => throw new YagoUnknownTypeException(code)
            };
            var result = await _mediator.Send(query, cancellationToken);
            return result.Statistics!.ToResponse();
        }
    }
}
