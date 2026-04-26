using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Services;
using static YAGO.World.Application.Cycles.Commands.RunCycle.RunCycleCommandHandler;

namespace YAGO.World.Application.Cycles.Commands.RunCycle
{
    public class RunCycleCommandHandler(
        IColonyRepository colonyRepository,
        ICycleRepository cycleRepository,
        IGameEventGenerator gameEventGenerator,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<RunCycleCommand, RunCycleResult>
    {
        public async Task<RunCycleResult> Handle(RunCycleCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException($"Отсутствует колония у пользователя с UserId={command.UserId}");
            var cycle = await cycleRepository.FindLastColonyCycle(colony.Id, cancellationToken)
                ?? Cycle.CreateNew(colony.Id, prevCycle: null);
            return cycle.ActiveEventId != null
                ? GetActiveEvent(cycle.ActiveEventId, colony.Stats)
                : await GenerateNewEpisode(colony, cycle, cancellationToken);
        }

        private async Task<RunCycleResult> GenerateNewEpisode(Colony colony, Cycle cycle, CancellationToken cancellationToken)
        {
            cycle.RunCycle();
            var gameEvents = GameEventsDataset.GetAll();
            var gameEventGenerateResult = gameEventGenerator.Generate(gameEvents, cycle.StepNumber, colony);
            var colonyStats = colony.Stats;
            var episode = AddDaysPassed(colonyStats, gameEventGenerateResult);
            var activeEvent = episode.Dilemma != null ? episode.Id : null;
            cycle.SetStepNumber(gameEventGenerateResult.StepNumber, activeEvent, gameEventGenerateResult.IsCycleEnded);
            if (episode.ChangesWithoutChoice != null)
            {
                colonyStats.SetEpisodeParameters(episode.ChangesWithoutChoice, gameEventGenerateResult.IsCycleEnded);
            }

            var list = new List<IEntity> { colony, cycle };
            if (gameEventGenerateResult.IsCycleEnded)
            {
                var nextCycle = Cycle.CreateNew(colony.Id, cycle);
                list.Add(nextCycle);
            }
            await unitOfWorkRepository.SaveInTransactionAsync(list, cancellationToken);

            var episodeForColony = new ColonyEpisode(episode, colony.Stats);
            return new RunCycleResult(episodeForColony, gameEventGenerateResult.IsCycleEnded);
        }

        private static Episode AddDaysPassed(ColonyStats colonyStats, GameEventGenerateResult gameEventGenerateResult)
        {
            if (gameEventGenerateResult.DaysPassed == 0)
                return gameEventGenerateResult.Episode;

            var episode = gameEventGenerateResult.Episode;
            var daysPassedSlide = GetDaysPassedSlide(colonyStats, gameEventGenerateResult, episode);
            return new Episode(
                episode.Id,
                episode.Title,
                prologSlides: [daysPassedSlide, .. episode.PrologueSlides],
                dilemma: episode.Dilemma);
        }

        private static PrologueSlide GetDaysPassedSlide(
            ColonyStats colonyStats,
            GameEventGenerateResult gameEventGenerateResult, 
            Episode episode)
        {
            var text = gameEventGenerateResult.DaysPassed switch
            {
                1 => "Прошёл день.",
                2 => "Спустя пару дней.",
                3 or 4 => $"Прошло {gameEventGenerateResult.DaysPassed} дня.",
                _ => $"Спустя {gameEventGenerateResult.DaysPassed} спокойных дней."
            };
            var parameters = CalculateAndSetParametersChanges(colonyStats, gameEventGenerateResult.DaysPassed);
            var daysPassedSlide = new PrologueSlide(
                episode.Title,
                ImageSet.Station_1,
                [text],
                parameters,
                continueButtonName: "Далее");
            return daysPassedSlide;
        }

        private static List<KeyValueParameter> CalculateAndSetParametersChanges(
            ColonyStats colonyStats,
            int daysPassed)
        {
            var list = new List<KeyValueParameter>();
            const double daysInCycle = 7;
            var chacgeCoefficient = daysPassed / daysInCycle;
            if (colonyStats.BudgetBalance != 0)
            {
                var change = colonyStats.BudgetBalance * chacgeCoefficient;
                list.Add(new KeyValueParameter(ColonyStatNames.Economic_Budget_Balance, change));
            }

            var moodTotalBalance = colonyStats.MoodTotalBalanceCacl();
            if (moodTotalBalance != 0)
            {
                var change = moodTotalBalance * chacgeCoefficient;
                list.Add(new KeyValueParameter(ColonyStatNames.Mood_Total, change));
            }

            colonyStats.SetEpisodeParameters(list, false);
            return list;
        }

        private static RunCycleResult GetActiveEvent(string activeEvent, ColonyStats colonyStats)
        {
            var gameEvent = GameEventsDataset.Get(activeEvent);
            var episodeForColony = new ColonyEpisode(gameEvent.Episode, colonyStats);
            return new RunCycleResult(episodeForColony, IsCycleCompleted: false);
        }

        public record RunCycleCommand(long UserId) : IRequest<RunCycleResult>;
        public record RunCycleResult(ColonyEpisode Episode, bool IsCycleCompleted);
    }
}
