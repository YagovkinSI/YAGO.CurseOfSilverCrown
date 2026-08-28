using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Statistics.Queries.Models;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Application.Statistics.Queries
{
    public class GetMainMoreStatisticsQueryHandler(
        IColonyRepository colonyRepository)
        : IRequestHandler<GetMainMoreStatisticsQuery, GetStatisticsResult>
    {
        public async Task<GetStatisticsResult> Handle(
            GetMainMoreStatisticsQuery query,
            CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(query.UserId, cancellationToken)
                ?? throw new YagoException("Необходимо иметь колонию.");

            var fields = new List<StatisticFieldDto>
            {
                GetFieldStation(colony),

                GetFieldGdp(colony),
                GetFieldPopulation(colony),
                GetFieldAttractiveness(colony),

                GetFieldReforms(colony),

                GetFieldTurnNumber(colony),
            };

            var statistics = new StatisticsResult(
                StatisticCode.MainMore,
                $"Колония: {colony.DisplayInfo.DisplayName}",
                fields);
            return new GetStatisticsResult(statistics);
        }

        private static StatisticFieldDto GetFieldStation(Colony colony)
        {
            return new(
                ParameterCategory.Info,
                "Станция",
                "Рассвет-342",
                ParameterStatus.Neutral,
                Description: [
                    "«Рассвет-342» — типовой проект корпорации RAS, основная рабочая лошадка Пояса. Станция представляет собой вращающееся кольцо диаметром 150 метров, создающее искусственную гравитацию в 0.85 g — достаточно для комфортной жизни и работы без серьёзных последствий для здоровья.",
                    "Внутри — три жилых этажа с центральной улицей, опоясывающей всё кольцо. Пространство разделено на модульные секции, позволяющие гибко настраивать внутреннюю планировку под нужды колонии: от жилых капсул до производственных цехов и лабораторий. Полная застройка вмещает до тысячи человек.",
                    "Станция оснащена ядерным реактором на быстрых нейтронах, обеспечивающим энергией все системы. Замкнутый цикл рециркуляции воды и воздуха поддерживает жизнеобеспечение с эффективностью 99.9%. Гидропонные фермы и биореакторы покрывают до 85% потребностей в пище, снижая зависимость от земных поставок. Два стыковочных узла позволяют принимать грузовые и пассажирские буксиры, интегрируя станцию в транспортную сеть Пояса.",
                    "«Рассвет-342» — это не просто жилой модуль. Это полноценная платформа для добычи, производства и жизни в космосе, способная существовать автономно долгие месяцы."],
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldGdp(Colony colony)
        {
            return new(
                ParameterCategory.Solars,
                "ВВП",
                $"{colony.State.GetGdp().ToBeautifulString()} солар",
                ParameterStatus.Neutral,
                Description: [],
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldPopulation(Colony colony)
        {
            return new(
                ParameterCategory.Population,
                "Население",
                $"{colony.State.GetPopulation().ToBeautifulString()}",
                ParameterStatus.Neutral,
                Description: [],
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldAttractiveness(Colony colony)
        {
            return new(
                ParameterCategory.PrivateCapital,
                "Привлекательность",
                $"{colony.State.GetAttractiveness().ToBeautifulString()}",
                ParameterStatus.Neutral,
                Description: [
                    "Инвестиционная привлекательность колонии показывает, насколько она интересна для новых частных компаний и потенциальных колонистов, и измеряется от –100 до 100 баллов.",
                    "Положительное значение говорит о благоприятном инвестиционном климате: чем ближе показатель к 100, тем выше вероятность, что новые компании и жители появятся уже на текущей неделе. Значение около нуля свидетельствует о стабильности, хотя при этом возможна естественная ротация — одни колонисты уезжают, другие прибывают. Отрицательная привлекательность указывает на серьёзные проблемы: если не принять мер, колонию начнут покидать и бизнес, и жители.",
                    "Для роста колонии нужно стремиться к высоким значениям, для удержания стабильности достаточно околонулевых, а при отрицательных показателях правители обычно принимают срочные меры для стимулирования инвестиционной привлекательности."],
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldReforms(Colony colony)
        {
            var humanism = colony.State.Reforms[ColonyReformType.SocialGuaranteesLevel].Value -
                colony.State.Reforms[ColonyReformType.TaxLevel].Value;
            var value = humanism switch
            {
                > 1 => "Гуманные",
                < -1 => "Корпоративные",
                _ => "Стандартные"
            };
            return new(
                ParameterCategory.Reforms,
                "Законы",
                value,
                ParameterStatus.Neutral,
                Description: [],
                ChildrenCode: null);
        }

        private static StatisticFieldDto GetFieldTurnNumber(Colony colony)
        {
            return new(
                ParameterCategory.Info,
                "Ход",
                $"{colony.State.Resources.TurnNumber.Value.ToBeautifulString()}",
                ParameterStatus.Neutral,
                Description: [],
                ChildrenCode: null);
        }

    }

    public record GetMainMoreStatisticsQuery(long UserId) : IRequest<GetStatisticsResult>;
}
