using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.ColonyStats.Parameters;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Companies;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Services
{
    public static class RunCycleService
    {
        public static Episode RunCycle(Cycle cycle, Colony colony, ColonyCompanies companies)
        {
            if (cycle.State == CycleState.Ready)
                cycle.SetInProgress();

            Slide? notification;
            var challenges = GameEventsDataset.Get();
            for (var i = cycle.StepNumber; i < challenges.Length; i++)
            {
                var challenge = challenges[i];
                if (challenge.Check(colony, companies))
                {
                    notification = challenge.ToNotification();
                    SetParameters(colony, challenge.ParameterChanges);
                    var newCompanies = CompanyDataset.GetCompanies(colony.CompanyIds);
                    companies.Update(newCompanies);
                    cycle.SetStepNumber(i + 1);
                    return new Episode(id: null, [notification], сhoiceLabel: null, сhoice: null);
                }
            }

            cycle.SetStepNumber(challenges.Length);
            cycle.SetCompleted();
            var budget = new Budget(
                colony,
                companies);
            colony.AddSolars(budget.Balance);
            var population = new Population(colony, companies);
            var moodReduction = Mood.CalculateReduction(population, colony.CodeOfLaws);
            colony.AddFestivalEffect(moodReduction);
            colony.AddWeek();
            return CycleCompletedNotification(budget);
        }

        private static void SetParameters(Colony colony, IReadOnlyList<KeyValueParameter> colonyParameters)
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
