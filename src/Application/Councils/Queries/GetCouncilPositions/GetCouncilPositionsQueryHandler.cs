using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies.Councils;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Persons;

namespace YAGO.World.Application.Councils.Queries.GetCouncilPositions
{
    public class GetCouncilPositionsQueryHandler
        (IColonyRepository colonyRepository,
        IPersonRepository personRepository)
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
                GetPosition(CouncilPosition.Administrator, council.Administrator, cancellationToken),
                GetPosition(CouncilPosition.Engineer, council.Engineer, cancellationToken),
                GetPosition(CouncilPosition.Financier, council.Financier, cancellationToken),
                GetPosition(CouncilPosition.Social, council.Social, cancellationToken));
            return new GetCouncilPositionsResult(positions);
        }

        private async Task<CouncilPositionDto> GetPosition(
            CouncilPosition code,
            CouncilAdvisor? advisor,
            CancellationToken cancellationToken)
        {
            var info = GetPositionInfo(code);
            var person = await personRepository.Get(GetPersonCode(code), cancellationToken);
            return new CouncilPositionDto(
                code,
                info.Title,
                info.Description,
                person,
                advisor?.Loyalty ?? 0);
        }

        private static string GetPersonCode(CouncilPosition code)
        {
            return code switch
            {
                CouncilPosition.Administrator => PersonCode.Camilla,
                CouncilPosition.Engineer => PersonCode.Lien,
                CouncilPosition.Financier => PersonCode.Cassius,
                CouncilPosition.Social => PersonCode.Darius,
            };
        }

        private static CouncilPositionInfo GetPositionInfo(CouncilPosition code)
        {
            return code switch
            {
                CouncilPosition.Administrator => new(
                    "Администратор",
                    "Координирует работу станции, связь с Консорциумом и замещает правителя. Решает задачи, не входящие в компетенцию других советников."),
                CouncilPosition.Engineer => new(
                    "Инженер станции",
                    "Отвечает за реактор, системы жизнеобеспечения и техническое состояние станции."),
                CouncilPosition.Financier => new(
                    "Финансист",
                    "Управляет бюджетом, налогами, контрактами и отчётностью перед Консорциумом."),
                CouncilPosition.Social => new(
                    "Социальный советник",
                    "Отвечает за найм, удержание колонистов и внутренний климат. Решает конфликты."),
            };
        }
    }

    public record GetCouncilPositionsQuery(long UserId) : IRequest<GetCouncilPositionsResult>;
    public record GetCouncilPositionsResult(IReadOnlyList<CouncilPositionDto> Positions);
    public record CouncilPositionDto(
        CouncilPosition Code,
        string Title,
        string Description,
        Person Person,
        int Loyalty);

    internal sealed record CouncilPositionInfo(
        string Title,
        string Description);
}