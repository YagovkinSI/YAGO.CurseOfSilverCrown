using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.ColonyStats.Parameters;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Services
{
    public static class RunCycleService
    {
        public static Episode RunCycle(Cycle cycle, Colony colony)
        {
            if (cycle.State == CycleState.Ready)
                cycle.SetInProgress();

            Slide? notification;
            var challenges = GameEventsDataset.Get();
            for (var i = cycle.StepNumber; i < challenges.Length; i++)
            {
                var challenge = challenges[i];
                if (challenge.Check(colony))
                {
                    notification = challenge.ToNotification();
                    SetParameters(colony, challenge.ParameterChanges);
                    cycle.SetStepNumber(i + 1);
                    return new Episode(id: null, [notification], сhoiceLabel: null, сhoice: null);
                }
            }

            cycle.SetStepNumber(challenges.Length);
            cycle.SetCompleted();
            var budget = new Budget(colony);
            colony.AddSolars(budget.Balance);
            var population = new Population(colony);
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

            var (industryChanges, count) = FindIndustryChanges(colonyParameters);
            if (industryChanges != null)
            {
                var zonesOccupied = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.AreaCapacity_Occupied)?.Value ?? 0);
                var solarIncome = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Economic_Budget_Balance)?.Value ?? 0);
                var population = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Population_Total)?.Value ?? 0);
                colony.AddCompany(industryChanges, count, zonesOccupied, solarIncome, population);
            }

            var moodTotal = colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.Mood_Total);
            if (moodTotal != null)
                colony.AddFestivalEffect(moodTotal.Value);

            var firstWedding = colonyParameters.FirstOrDefault(x => x.Name == ColonyParameterNames.FirstWedding);
            if (firstWedding != null)
                colony.SetFirstWedding();
        }

        private static (string? industryName, int count) FindIndustryChanges(IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            if (colonyParameters.Any(x => x.Name == ColonyParameterNames.Industry_Minning_Companies))
                return (IndustryNameConstants.Minning, (int)colonyParameters.Single(x => x.Name == ColonyParameterNames.Industry_Minning_Companies).Value);
            else if (colonyParameters.Any(x => x.Name == ColonyParameterNames.Industry_Production_Companies))
                return (IndustryNameConstants.Production, (int)colonyParameters.Single(x => x.Name == ColonyParameterNames.Industry_Production_Companies).Value);
            else if (colonyParameters.Any(x => x.Name == ColonyParameterNames.Industry_Service_Companies))
                return (IndustryNameConstants.Service, (int)colonyParameters.Single(x => x.Name == ColonyParameterNames.Industry_Service_Companies).Value);
            else
                return (null, 0);
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
