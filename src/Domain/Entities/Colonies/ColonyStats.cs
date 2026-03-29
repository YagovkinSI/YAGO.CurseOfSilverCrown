using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyStats
    {
        public ColonySettings Settings { get; }
        public ColonyResources Resources { get; }
        public ColonyIndicators Indicators { get; }

        public int ZonesAvailable => Resources.ZonesTotal - Indicators.ZonesOccupied;

        public ColonyStats(
            ColonySettings settings,
            ColonyResources resources,
            ColonyIndicators indicators)
        {
            Settings = settings;
            Resources = resources;
            Indicators = indicators;
        }

        public static ColonyStats CreateNew(
            CodeOfLaws gavernorType)
        {
            var colonySettings = ColonySettings.CreateNew(gavernorType);
            var colonyResources = ColonyResources.CreateNew();
            var colonyIndicators = ColonyIndicators.CreateNew();
            return new ColonyStats(
                colonySettings,
                colonyResources,
                colonyIndicators);
        }

        public double GetGameParameter(string parameterName)
        {
            //TODO: Разделить
            var colonyIndustries = Indicators.Industries;
            var minningIndustry = colonyIndustries.Minning;
            var productionIndustry = colonyIndustries.Production;
            var serviceIndustry = colonyIndustries.Service;

            return parameterName switch
            {
                ColonyStatNames.Economic_Reserves => Resources.Solars,
                ColonyStatNames.Mood_Total => Indicators.MoodTotalCacl(),
                ColonyStatNames.Mood_Total_Balance => MoodTotalBalanceCacl(),
                ColonyStatNames.Population_Total => Indicators.PopulationTotal,
                ColonyStatNames.AreaCapacity_Occupied => Indicators.ZonesOccupied,
                ColonyStatNames.Economic_Budget_Balance => Indicators.BudgetBalance,
                ColonyStatNames.Industry_Minning_Available => 12 - minningIndustry.UnitCount,
                ColonyStatNames.Industry_Minning_Companies => minningIndustry.UnitCount,
                ColonyStatNames.Industry_Production_Companies => productionIndustry.UnitCount,
                ColonyStatNames.Industry_Service_Companies => serviceIndustry.UnitCount,
                ColonyStatNames.Industry_Service_Need => (Indicators.PopulationTotal / 50.0) - serviceIndustry.UnitCount - 1.5,
                ColonyStatNames.AreaCapacity_Total => Resources.ZonesTotal,
                ColonyStatNames.AreaCapacity_Available => ZonesAvailable,
                ColonyStatNames.Laws_CodeOfLaws => (double)Settings.CodeOfLaws,
                ColonyStatNames.Laws_CodeOfLaws_HighTax => Settings.CodeOfLaws == CodeOfLaws.Capitalist ? 1 : 0,
                ColonyStatNames.Laws_CodeOfLaws_HighStandart => Settings.CodeOfLaws == CodeOfLaws.Humanist ? 1 : 0,
                ColonyStatNames.Attractiveness_Total => AttractivenessTotalCalc(),
                ColonyStatNames.FirstWedding => Indicators.FirstWedding ? 1 : 0,
                ColonyStatNames.CurrentWeek => Indicators.CurrentWeek,
                _ => throw new YagoUnknownTypeException(parameterName)
            };
        }

        public void IssueDecree(Decree decree)
        {
            var solarResservesParameter = decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Reserves)?.Value ?? 0;
            if (Resources.Solars < -solarResservesParameter)
                throw new YagoException("Недостаточно средств.");

            if (ZonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.AreaCapacity_Occupied)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");

            Resources.AddSolars(solarResservesParameter);
            Indicators.AddFestivalEffect(decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Mood_Total)?.Value ?? 0);
        }

        public void SetEpisodeParameters(IReadOnlyList<KeyValueParameter> colonyParameters, bool isCycleOver)
        {
            var solars = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Reserves);
            if (solars != null)
                Resources.AddSolars((int)solars.Value);

            var colonyIndustries = Indicators.Industries;
            colonyIndustries.SetIndustryParameters(colonyParameters);

            var moodTotal = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Mood_Total);
            if (moodTotal != null)
                Indicators.AddFestivalEffect(moodTotal.Value);

            var firstWedding = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.FirstWedding);
            if (firstWedding != null)
                Indicators.SetFirstWedding();

            if (isCycleOver)
                Indicators.AddCurrentWeek();
        }        

        private double MoodTotalBalanceCacl()
        {
            var codeOfLawsCoef = 1 + (((int)Settings.CodeOfLaws - 2) / 5.0);
            return -Indicators.PopulationTotal * 0.01 * codeOfLawsCoef;
        }

        public double AttractivenessTotalCalc()
        {
            var defaultValue = 100;
            var taxEffect = -30 * (int)Settings.CodeOfLaws;
            var standartsEffect = -30 * (3 - (int)Settings.CodeOfLaws);
            var stabilityEffect = Math.Min(50, Indicators.CurrentWeek / 10.0);
            return Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }
    }
}