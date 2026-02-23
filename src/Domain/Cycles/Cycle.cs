using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Companies;
using YAGO.World.Domain.Decrees;
using YAGO.World.Domain.Episodes;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Domain.Cycles
{
    public class Cycle : IEntity
    {
        /// <summary>
        /// Идентификатор цикла
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Идентификатор колонии владельца
        /// </summary>
        public long ColonyId { get; }

        /// <summary>
        /// Шаг цикла
        /// </summary>
        public int StepNumber { get; private set; }

        /// <summary>
        /// Дата и время запуска цикла
        /// </summary>
        public DateTime? RunAtUtc { get; private set; }

        /// <summary>
        /// Статус цикла
        /// </summary>
        public CycleState State { get; private set; }

        public Cycle(
            long id,
            long colonyId,
            int stepNumber,
            DateTime? runAtUtc,
            CycleState cycleState)
        {
            Id = id;
            ColonyId = colonyId;
            StepNumber = stepNumber;
            RunAtUtc = runAtUtc;
            State = cycleState;
        }

        public Episode RunCycle(Colony colony, ColonyCompanies companies, Ship ship)
        {
            if (State == CycleState.Ready)
                State = CycleState.InProgress;

            if (RunAtUtc == null)
                RunAtUtc = DateTime.UtcNow;

            Slide? notification;
            var challenges = GameEventsDataset.Get();
            for (var i = StepNumber; i < challenges.Length; i++)
            {
                var challenge = challenges[i];
                if (challenge.Check(colony, companies, ship))
                {
                    notification = challenge.ToNotification();
                    SetParameters(colony, challenge.ParameterChanges);
                    var newCompanies = CompanyDataset.GetCompanies(colony.CompanyIds);
                    companies.Update(newCompanies);
                    StepNumber = i + 1;
                    return new Episode(id: null, [notification], сhoiceLabel: null, сhoice: null);
                }
            }

            StepNumber = challenges.Length;

            var currentEpisode = GetEpisode(colony);
            if (currentEpisode != null)
                return currentEpisode;

            State = CycleState.Completed;
            var budget = new Budget(
                colony,
                companies,
                ship);
            colony.AddSolars(budget.Balance);
            var population = new Population(colony, companies);
            var moodReduction = Mood.CalculateReduction(population, colony.CodeOfLaws);
            colony.AddFestivalEffect(moodReduction);
            colony.AddWeek();
            return CycleCompletedNotification(budget);
        }

        public void SetParameters(Colony colony, IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            var solars = colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Economic_Reserves);
            if (solars != null)
                colony.AddSolars((int)solars.Value);

            var engineeringTeam = colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Companies_Minning_EngineeringTeam);
            if (engineeringTeam != null)
                colony.AddCompany(1);

            var miningBrigade = colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Companies_Minning_MiningBrigade);
            if (miningBrigade != null)
                colony.AddCompany(2);

            var rehabilitationContingent = colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Companies_Minning_RehabilitationContingent);
            if (rehabilitationContingent != null)
                colony.AddCompany(3);
        }

        private Episode? GetEpisode(Colony colony)
        {
            var episode = colony.CurrentWeek switch
            {
                2 => EpisodeDataset.Get(1),
                _ => null
            };

            return episode == null || colony.Episodes.ContainsKey(episode.Id!.Value)
                ? null
                : episode;
        }

        private static Episode CycleCompletedNotification(Budget budget)
        {
            var colonyParameters = new List<KeyValueParameter>()
            {
                new(ColonyParameterNames.Economic_Reserves, budget.Balance)
            };

            var slide = new Slide(
                "Успешное завершение цикла",
                ImageSet.RegularCycle,
                new string[]
                {
                    "В трюмах ритмично гудят дробилки, на мостике горят зелёные лампочки систем. " +
                    "Рудокопы в своих сменах монотонно, но эффективно откалывают породу.",
                    "Цикл успешно завершен, прибыль получена.",
                },
                colonyParameters);

            return new Episode(id: null, [slide], сhoiceLabel: null, сhoice: null);
        }
    }
}
