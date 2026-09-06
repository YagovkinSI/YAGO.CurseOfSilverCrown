using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.Persons;

namespace YAGO.World.Application.Councils.Queries.GetCouncilPositions
{
    public class GetCouncilPositionsQueryHandler
        (IColonyRepository colonyRepository,
        IPersonRepository personRepository,
        IColonyEventRepository colonyEventRepository,
        IGameEventRepository gameEventRepository)
        : IRequestHandler<GetCouncilPositionsQuery, GetCouncilPositionsResult>
    {
        public async Task<GetCouncilPositionsResult> Handle(
            GetCouncilPositionsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var council = colony.State.Council;
            var quests = await GetHireQuests(colony.Id, council, cancellationToken);
            var eventsByCode = await GetEventsByCode(cancellationToken);
            var positions = await Task.WhenAll(
                GetPosition(CouncilPosition.Administrator, council.Administrator, quests, eventsByCode, cancellationToken),
                GetPosition(CouncilPosition.Engineer, council.Engineer, quests, eventsByCode, cancellationToken),
                GetPosition(CouncilPosition.Financier, council.Financier, quests, eventsByCode, cancellationToken),
                GetPosition(CouncilPosition.Social, council.Social, quests, eventsByCode, cancellationToken));
            return new GetCouncilPositionsResult(positions);
        }

        private async Task<CouncilPositionDto> GetPosition(
            CouncilPosition code,
            CouncilAdvisor? advisor,
            IReadOnlyList<ColonyEvent> quests,
            IReadOnlyDictionary<string, IReadOnlyList<string>> eventsByCode,
            CancellationToken cancellationToken)
        {
            var info = GetPositionInfo(code);
            var person = await GetPerson(advisor, cancellationToken);
            var hireEventId = advisor == null && info.HireTag != null
                ? FindHireEventId(quests, eventsByCode, info.HireTag)
                : null;
            return new CouncilPositionDto(
                code,
                info.Title,
                info.Description,
                hireEventId,
                person,
                advisor?.Loyalty ?? 0);
        }

        private async Task<Person?> GetPerson(
            CouncilAdvisor? advisor,
            CancellationToken cancellationToken)
        {
            return advisor == null
                ? null
                : await personRepository.Get(advisor.Code, cancellationToken);
        }

        private async Task<IReadOnlyDictionary<string, IReadOnlyList<string>>> GetEventsByCode(
            CancellationToken cancellationToken)
        {
            var gameEvents = await gameEventRepository.GetAll(cancellationToken);
            return gameEvents.ToDictionary(e => e.Code, e => e.Tags);
        }

        private static long? FindHireEventId(
            IReadOnlyList<ColonyEvent> quests,
            IReadOnlyDictionary<string, IReadOnlyList<string>> eventsByCode,
            string hireTag)
        {
            return quests
                .Where(quest => eventsByCode.TryGetValue(quest.EventCode, out var tags) && tags.Contains(hireTag))
                .Select(quest => (long?)quest.Id)
                .FirstOrDefault();
        }

        private async Task<IReadOnlyList<ColonyEvent>> GetHireQuests(
            long colonyId,
            Council council,
            CancellationToken cancellationToken)
        {
            var hireTags = GetHireTags(council);
            if (hireTags.Count == 0)
                return [];

            return await colonyEventRepository.FindByColonyId(
                colonyId, onlyNotComplited: true, hireTags, cancellationToken);
        }

        private static IReadOnlyList<string> GetHireTags(Council council)
        {
            return new[]
            {
                (Advisor: council.Administrator, Position: CouncilPosition.Administrator),
                (Advisor: council.Engineer, Position: CouncilPosition.Engineer),
                (Advisor: council.Financier, Position: CouncilPosition.Financier),
                (Advisor: council.Social, Position: CouncilPosition.Social),
            }
            .Where(pair => pair.Advisor == null)
            .Select(pair => GetPositionInfo(pair.Position).HireTag)
            .OfType<string>()
            .Distinct()
            .ToArray();
        }

        private static CouncilPositionInfo GetPositionInfo(CouncilPosition code)
        {
            return code switch
            {
                CouncilPosition.Administrator => new(
                    "Администратор",
                    "Координирует работу станции, связь с Консорциумом и замещает правителя. Решает задачи, не входящие в компетенцию других советников.",
                    GameEventTags.CouncilAdministrator),
                CouncilPosition.Engineer => new(
                    "Инженер станции",
                    "Отвечает за реактор, системы жизнеобеспечения и техническое состояние станции. Без него станция умрёт. Нужен для расширения и модернизации модулей.",
                    null),
                CouncilPosition.Financier => new(
                    "Финансист",
                    "Управляет бюджетом, налогами, контрактами и отчётностью перед Консорциумом. Без него невозможны реформы и крупные финансовые операции.",
                    null),
                CouncilPosition.Social => new(
                    "Социальный советник",
                    "Отвечает за найм, удержание колонистов и внутренний климат. Решает конфликты, без него станция рискует остаться без людей.",
                    null),
            };
        }
    }

    public record GetCouncilPositionsQuery(long UserId) : IRequest<GetCouncilPositionsResult>;
    public record GetCouncilPositionsResult(IReadOnlyList<CouncilPositionDto> Positions);
    public record CouncilPositionDto(
        CouncilPosition Code,
        string Title,
        string Description,
        long? HireEventId,
        Person? Person,
        int Loyalty);

    internal sealed record CouncilPositionInfo(
        string Title,
        string Description,
        string? HireTag);
}
