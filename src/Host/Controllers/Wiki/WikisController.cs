using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Wiki.Queries.GetWikiArticle;
using YAGO.World.Application.Wiki.Queries.GetWikiSummaries;
using YAGO.World.Host.Controllers.Common.Extensions;

namespace YAGO.World.Host.Controllers.Wiki
{
    [ApiController]
    [Route("api/wiki")]
    public class WikisController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WikisController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [Route("getWikiSummaries")]
        public async Task<IReadOnlyList<WikiSummaryResponse>> GetWikiSummaries(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var query = new GetWikiSummariesQuery(userId);
            var result = await _mediator.Send(query, cancellationToken);
            return result.Summaries.ToSummaryResponse();
        }

        [HttpGet]
        [Authorize]
        [Route("getWikiArticle")]
        public async Task<WikiArticleResponse> GetWikiArticle(string code, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var query = new GetWikiArticleQuery(userId, code);
            var result = await _mediator.Send(query, cancellationToken);
            return result.Article.ToResponse();
        }
    }
}
