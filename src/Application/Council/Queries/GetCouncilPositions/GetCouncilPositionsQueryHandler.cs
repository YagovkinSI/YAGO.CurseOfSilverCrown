using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Council.Queries.GetCouncilPositions
{
    public class GetCouncilPositionsQueryHandler
        (IColonyRepository colonyRepository)
        : IRequestHandler<GetCouncilPositionsQuery, GetCouncilPositionsResult>
    {
        public async Task<GetCouncilPositionsResult> Handle(
            GetCouncilPositionsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var council = colony.State.Council;
            if (council.Administrator != null
                || council.Engineer != null
                || council.Financier != null
                || council.Social != null)
                throw new System.NotImplementedException(
                    "Нанятые советники пока не поддерживаются.");

            var positions = BuildPositions(council);
            return new GetCouncilPositionsResult(positions);
        }

        private static IReadOnlyList<CouncilPositionDto> BuildPositions(
            YAGO.World.Domain.Colonies.Council council)
        {
            var administrator = council.Administrator;
            return
            [
                CreatePosition(
                    "administrator",
                    "Администратор",
                    "Координация всей работы станции, связь с Консорциумом, замещает правителя. Решает задачи, которые не входят в компетенцию других советников. Необходим для найма остальных членов совета станции.",
                    canHire: administrator == null),
                CreatePosition(
                    "engineer",
                    "Инженер станции",
                    "Жизнеобеспечение (вода, воздух, энергия), реактор, системы станции. Без него станция умрёт. Необходим для постройки технических модулей и расширения станции.",
                    canHire: council.Engineer == null && administrator != null),
                CreatePosition(
                    "financier",
                    "Финансист",
                    "Бюджет, налоги, контракты, юридическая защита, отношения с Консорциумом по KPI и отчётности. Необходим для выполнения реформ и открытия меню построек.",
                    canHire: council.Financier == null && administrator != null),
                CreatePosition(
                    "social",
                    "Социальный советник",
                    "Найм колонистов, контракты, увольнения, внутренний климат, решение конфликтов между работниками. Необходим для постройки модулей с населением.",
                    canHire: council.Social == null && administrator != null),
            ];
        }

        private static CouncilPositionDto CreatePosition(
            string code,
            string title,
            string description,
            bool canHire) =>
            new(code, title, description, canHire, Member: null);
    }

    public record GetCouncilPositionsQuery(long UserId) : IRequest<GetCouncilPositionsResult>;
    public record GetCouncilPositionsResult(IReadOnlyList<CouncilPositionDto> Positions);
    public record CouncilPositionDto(
        string Code,
        string Title,
        string Description,
        bool CanHire,
        CouncilMemberDto? Member);
    public record CouncilMemberDto(
        string Name,
        string Avatar,
        int Loyalty,
        string WikiArticleCode);
}