using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Council
{
    [ApiController]
    [Route("api/council")]
    public class CouncilsController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [Route("getCouncilPositions")]
        public IReadOnlyList<CouncilPositionResponse> GetCouncilPositions()
        {
            return PlaceholderPositions;
        }

        private static readonly IReadOnlyList<CouncilPositionResponse> PlaceholderPositions =
        [
            new(CouncilPositionCodeConstants.Administrator, "Администратор",
                "Координатор станции. Отвечает за связь с Консорциумом и общее управление. Открывает доступ к найму других советников.",
                CanHire: true, Member: null),
            new(CouncilPositionCodeConstants.Engineer, "Инженер станции",
                "Следит за реактором, водой, воздухом и энергией. Позволяет расширять станцию и модернизировать модули.",
                CanHire: false, Member: null),
            new(CouncilPositionCodeConstants.Financier, "Финансист",
                "Управляет бюджетом, налогами и отчётностью. Позволяет проводить реформы и заключать контракты.",
                CanHire: false, Member: null),
            new(CouncilPositionCodeConstants.Social, "Социальный советник",
                "Отвечает за найм, удержание людей и внутренний климат. Обеспечивает рост населения и предотвращает конфликты.",
                CanHire: false, Member: null),
        ];
    }
}