using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Persons;

namespace YAGO.World.Application.Council.Queries.GetCouncilPositions
{
    public class GetCouncilPositionsQueryHandler
        (IColonyRepository colonyRepository, IPersonRepository personRepository)
        : IRequestHandler<GetCouncilPositionsQuery, GetCouncilPositionsResult>
    {
        public async Task<GetCouncilPositionsResult> Handle(
            GetCouncilPositionsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var council = colony.State.Council;
            var positions = await Task.WhenAll(
                GetAdministrator(council, cancellationToken),
                GetEngineer(council, cancellationToken),
                GetFinancier(council, cancellationToken),
                GetSocial(council, cancellationToken));
            return new GetCouncilPositionsResult(positions);
        }

        private async Task<CouncilPositionDto> GetAdministrator(
            YAGO.World.Domain.Colonies.Council council,
            CancellationToken cancellationToken)
        {
            var person = await GetPerson(council.Administrator, cancellationToken);
            return new CouncilPositionDto(
                CouncilPosition.Administrator,
                "Администратор",
                "Координирует работу станции, связь с Консорциумом и замещает правителя. Решает задачи, не входящие в компетенцию других советников.",
                council.CanHireAdministrator(),
                person,
                council.Administrator?.Loyalty ?? 0);
        }

        private async Task<CouncilPositionDto> GetEngineer(
            YAGO.World.Domain.Colonies.Council council,
            CancellationToken cancellationToken)
        {
            var person = await GetPerson(council.Engineer, cancellationToken);
            return new CouncilPositionDto(
                CouncilPosition.Engineer,
                "Инженер станции",
                "Отвечает за реактор, системы жизнеобеспечения и техническое состояние станции. Без него станция умрёт. Нужен для расширения и модернизации модулей.",
                council.CanHireEngineer(),
                person,
                council.Engineer?.Loyalty ?? 0);
        }

        private async Task<CouncilPositionDto> GetFinancier(
            YAGO.World.Domain.Colonies.Council council,
            CancellationToken cancellationToken)
        {
            var person = await GetPerson(council.Financier, cancellationToken);
            return new CouncilPositionDto(
                CouncilPosition.Financier,
                "Финансист",
                "Управляет бюджетом, налогами, контрактами и отчётностью перед Консорциумом. Без него невозможны реформы и крупные финансовые операции.",
                council.CanHireFinancier(),
                person,
                council.Financier?.Loyalty ?? 0);
        }

        private async Task<CouncilPositionDto> GetSocial(
            YAGO.World.Domain.Colonies.Council council,
            CancellationToken cancellationToken)
        {
            var person = await GetPerson(council.Social, cancellationToken);
            return new CouncilPositionDto(
                CouncilPosition.Social,
                "Социальный советник",
                "Отвечает за найм, удержание колонистов и внутренний климат. Решает конфликты, без него станция рискует остаться без людей.",
                council.CanHireSocial(),
                person,
                council.Social?.Loyalty ?? 0);
        }

        private async Task<Person?> GetPerson(
            CouncilAdvisor? advisor,
            CancellationToken cancellationToken)
        {
            return advisor == null
                ? null
                : await personRepository.Get(advisor.Code, cancellationToken);
        }
    }

    public record GetCouncilPositionsQuery(long UserId) : IRequest<GetCouncilPositionsResult>;
    public record GetCouncilPositionsResult(IReadOnlyList<CouncilPositionDto> Positions);
    public record CouncilPositionDto(
        CouncilPosition Code,
        string Title,
        string Description,
        bool CanHire,
        Person? Person,
        int Loyalty);
}