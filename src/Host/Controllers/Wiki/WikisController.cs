using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Wiki.Queries.GetWikiArticle;
using YAGO.World.Application.Wiki.Queries.GetWikiSummaries;

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
            var query = new GetWikiSummariesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.Articles.ToSummaryResponse();
        }

        [HttpGet]
        [Authorize]
        [Route("getWikiArticle")]
        public async Task<WikiArticleResponse> GetWikiArticle(string code, CancellationToken cancellationToken)
        {
            var query = new GetWikiArticleQuery(code);
            var result = await _mediator.Send(query, cancellationToken);
            return result.Article.ToResponse();
        }
    }
}
