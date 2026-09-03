using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Statistics.Queries;
using YAGO.World.Application.Statistics.Queries.Budget;
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
            IRequest<GetStatisticsResult> query = code switch
            {
                StatisticCodeConstants.Main => new GetMainStatisticsQuery(userId),
                StatisticCodeConstants.MainMore => new GetMainMoreStatisticsQuery(userId),
                StatisticCodeConstants.Solars => new GetSolarsStatisticsQuery(userId),
                StatisticCodeConstants.SolarDelta => new GetSolarDeltaStatisticsQuery(userId),
                StatisticCodeConstants.PublicDebt => new GetPublicDebtStatisticsQuery(userId),
                StatisticCodeConstants.AdministrationSalary => new GetAdministrationSalaryStatisticsQuery(userId),
                _ => throw new YagoUnknownTypeException(code)
            };
            var result = await _mediator.Send(query, cancellationToken);
            return result.Statistics!.ToResponse();
        }

        [HttpGet]
        [Authorize]
        [Route("getHeaderParameters")]
        public async Task<List<StatisticFieldResponse>> GetHeaderParameters(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _mediator.Send(new GetHeaderParametersQuery(userId), cancellationToken);
            return result.Select(x => x.ToResponse()).ToList();
        }
    }
}
