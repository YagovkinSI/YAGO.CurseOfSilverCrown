using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Statistics.Queries.Budget
{
    public class GetAdministrationSalaryStatisticsQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetAdministrationSalaryStatisticsQuery, GetStatisticsResult>
    {
        public async Task<GetStatisticsResult> Handle(
            GetAdministrationSalaryStatisticsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var fields = colony.GetAdministrationSalaries()
                .Select(GetSalaryField)
                .Append(GetTotal(colony))
                .ToList();

            var statistics = new StatisticsResult(
                StatisticCode.AdministrationSalary,
                $"Администрация",
                fields);
            return new GetStatisticsResult(statistics);
        }

        private static StatisticFieldDto GetSalaryField(AdministrationSalary salary)
        {
            var roleName = GetRoleName(salary.Role);
            return new(
                ParameterCategory.SolarDelta,
                roleName,
                $"{(-salary.SalaryPerYear).ToBeautifulString(setPlus: true)}",
                salary.SalaryPerYear.ToStatusByZero(invert: true),
                Info: new DisplayInfo(
                    roleName,
                    description: [
                        $"Зарплата должности «{roleName}» оплачивается из бюджета колонии."]),
                ChildrenCode: null);
        }

        private static string GetRoleName(AdministrationSalaryRole role)
        {
            return role switch
            {
                AdministrationSalaryRole.Ruler => "Правитель",
                AdministrationSalaryRole.Administrator => "Администратор",
                AdministrationSalaryRole.Engineer => "Инженер станции",
                AdministrationSalaryRole.Financier => "Финансист",
                AdministrationSalaryRole.Social => "Социальный советник",
            };
        }

        private static StatisticFieldDto GetTotal(Colony colony)
        {
            var value = colony.GetAdministrationSalary();
            return new(
                ParameterCategory.SolarDelta,
                "ИТОГО",
                $"{(-value).ToBeautifulString(setPlus: true)}",
                value.ToStatusByZero(invert: true),
                Info: new DisplayInfo(
                    "Администрация",
                    description: [
                        "Расходы на содержание администрации колонии за год."]),
                ChildrenCode: null);
        }
    }

    public record GetAdministrationSalaryStatisticsQuery(long UserId) : IRequest<GetStatisticsResult>;
}
