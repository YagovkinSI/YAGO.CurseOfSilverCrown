using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Colonies.Companies;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Colonies.Ships;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Dilemmas;
using YAGO.World.Domain.Episodes;
using YAGO.World.Domain.GameEvents;

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
                    var colonyStats = colony.Stats;
                    var newCompanies = CompanyDataset.GetCompanies(colonyStats.CompanyIds);
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
            var policies = colony.Policies;
            var moodReduction = Mood.CalculateReduction(population, policies.CodeOfLaws);
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

            var industry_Production_Companies = colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Industry_Production_Companies);
            if (industry_Production_Companies != null)
                colony.AddCompany(4);

            var industry_Service_Companies = colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Industry_Service_Companies);
            if (industry_Service_Companies != null)
                colony.AddCompany(5);

            var moodTotal = colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Mood_Total);
            if (moodTotal != null)
                colony.AddFestivalEffect(moodTotal.Value);

            var firstWedding = colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.FirstWedding);
            if (firstWedding != null)
                colony.SetFirstWedding();
        }

        private Episode? GetEpisode(Colony colony)
        {
            var colonyStats = colony.Stats;
            var colonyFlags = colony.Flags;
            var episode = colonyStats.CurrentWeek switch
            {
                200 => DilemmaDataset.Get(1),
                _ => null
            };

            return episode == null || colonyFlags.Episodes.ContainsKey(episode.Id!.Value)
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
