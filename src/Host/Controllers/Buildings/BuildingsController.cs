using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Buildings.Commands;
using YAGO.World.Host.Controllers.Common;
using static YAGO.World.Application.Buildings.Queries.GetBuildings.GetBuildingsQueryHandler;

namespace YAGO.World.Host.Controllers.Buildings
{
    [ApiController]
    [Route("api/buildings")]
    public class BuildingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BuildingsController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [Route("getBuildings")]
        public async Task<MyBuilding[]> Get(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var command = new GetBuildingsQuery(userId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.Buildings.Select(x => x.ToMyBuilding(result.ColonyState)).ToArray();
        }

        [HttpPost]
        [Authorize]
        [Route("build")]
        public async Task Build(BuildRequest request, CancellationToken cancellationToken)
        {
            var domainType = ColonyBuildingMapping.ToDomainType(request.BuildType);
            var userId = User.GetUserId();
            var command = new BuildCommand(userId, domainType, request.IsPrivate);
            await _mediator.Send(command, cancellationToken);
        }
    }
}
