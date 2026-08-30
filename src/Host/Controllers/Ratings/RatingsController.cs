using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Ratings.Models;
using YAGO.World.Application.Ratings.Queries;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Host.Controllers.Common.Extensions;
using YAGO.World.Host.Controllers.Statistics;

namespace YAGO.World.Host.Controllers.Ratings
{
    [ApiController]
    [Route("api/ratings")]
    public class RatingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RatingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("getRatings")]
        public async Task<List<StatisticFieldResponse>> GetRatings(
            string code,
            string? userId,
            CancellationToken cancellationToken)
        {
            var ratingCode = code switch
            {
                RatingCodeConstants.Population => RatingCode.Population,
                RatingCodeConstants.Laws => RatingCode.Laws,
                RatingCodeConstants.Mood => RatingCode.Mood,
                RatingCodeConstants.Budget => RatingCode.Budget,
                RatingCodeConstants.Attractiveness => RatingCode.Attractiveness,
                RatingCodeConstants.Area => RatingCode.Area,
                RatingCodeConstants.Week => RatingCode.Week,
                _ => throw new YagoUnknownTypeException(code)
            };

            long? parsedUserId = null;
            if (!string.IsNullOrEmpty(userId) && long.TryParse(userId, out var parsed))
                parsedUserId = parsed;

            var result = await _mediator.Send(new GetRatingsQuery(ratingCode, parsedUserId), cancellationToken);
            return result.Select(x => x.ToResponse()).ToList();
        }
    }
}
