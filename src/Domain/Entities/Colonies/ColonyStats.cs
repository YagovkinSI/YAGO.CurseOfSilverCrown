using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyStats
    {
        public ColonySettings Settings { get; }
        public ColonyResources Resources { get; }
        public ColonyIndustryList Industries { get; }

        /// <summary>
        /// Эффект от праздника
        /// </summary>
        public double FestivalEffect { get; private set; }

        /// <summary>
        /// Текущая неделя
        /// </summary>
        public int CurrentWeek { get; private set; }

        /// <summary>
        /// была ли первая свадьба
        /// </summary>
        public bool FirstWedding { get; private set; }

        public int PopulationTotal => Industries.PopulationTotal;
        public int ZonesOccupied => Industries.ZonesOccupiedTotal;
        public double BudgetBalance => Industries.SolarsIncomeTotal;
        public int ZonesAvailable => Resources.ZonesTotal - ZonesOccupied;

        public ColonyStats(
            ColonySettings settings,
            ColonyResources resources,
            ColonyIndustryList industries,
            double festivalEffect,
            int currentWeek,
            bool firstWedding)
        {
            Settings = settings;
            Resources = resources;
            Industries = industries;
            FestivalEffect = festivalEffect;
            CurrentWeek = currentWeek;
            FirstWedding = firstWedding;
        }

        public static ColonyStats CreateNew(
            CodeOfLaws gavernorType)
        {
            var colonySettings = ColonySettings.CreateNew(gavernorType);
            var colonyResources = ColonyResources.CreateNew();
            var colonyIndustryList = new ColonyIndustryList(
                administrativeIndustry: AdministrativeIndustry.CreateNew(),
                minningIndustry: MinningIndustry.CreateNew(),
                productionIndustry: ProductionIndustry.CreateNew(),
                serviceIndustry: ServiceIndustry.CreateNew());
            return new ColonyStats(
                colonySettings,
                colonyResources,
                colonyIndustryList,
                festivalEffect: 0,
                currentWeek: 0,
                firstWedding: false);
        }

        public double GetGameParameter(string parameterName)
        {
            if (parameterName.StartsWith(ColonyStatGroupNames.Industry))
                return GetIndustryParameter(parameterName);

            return parameterName switch
            {
                ColonyStatNames.Economic_Reserves => Resources.Solars,
                ColonyStatNames.Mood_Total => MoodTotalCacl(),
                ColonyStatNames.Mood_Total_Balance => MoodTotalBalanceCacl(),
                ColonyStatNames.Population_Total => PopulationTotal,
                ColonyStatNames.AreaCapacity_Occupied => ZonesOccupied,
                ColonyStatNames.Economic_Budget_Balance => BudgetBalance,
                ColonyStatNames.AreaCapacity_Total => Resources.ZonesTotal,
                ColonyStatNames.AreaCapacity_Available => ZonesAvailable,
                ColonyStatNames.Laws_CodeOfLaws => (double)Settings.CodeOfLaws,
                ColonyStatNames.Laws_CodeOfLaws_HighTax => Settings.CodeOfLaws == CodeOfLaws.Capitalist ? 1 : 0,
                ColonyStatNames.Laws_CodeOfLaws_HighStandart => Settings.CodeOfLaws == CodeOfLaws.Humanist ? 1 : 0,
                ColonyStatNames.Attractiveness_Total => AttractivenessTotalCalc(),
                ColonyStatNames.FirstWedding => FirstWedding ? 1 : 0,
                ColonyStatNames.CurrentWeek => CurrentWeek,
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
            AddFestivalEffect(decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Mood_Total)?.Value ?? 0);
        }

        public void SetEpisodeParameters(IReadOnlyList<KeyValueParameter> colonyParameters, bool isCycleOver)
        {
            var solars = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Reserves);
            if (solars != null)
                Resources.AddSolars((int)solars.Value);

            Industries.SetIndustryParameters(colonyParameters);

            var moodTotal = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Mood_Total);
            if (moodTotal != null)
                AddFestivalEffect(moodTotal.Value);

            var firstWedding = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.FirstWedding);
            if (firstWedding != null)
                SetFirstWedding();

            if (isCycleOver)
                AddCurrentWeek();
        }

        public double AttractivenessTotalCalc()
        {
            var defaultValue = 100;
            var taxEffect = -30 * (int)Settings.CodeOfLaws;
            var standartsEffect = -30 * (3 - (int)Settings.CodeOfLaws);
            var stabilityEffect = Math.Min(50, CurrentWeek / 10.0);
            return Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }

        public double MoodTotalCacl()
        {
            var moodTotal = 52.0;
            moodTotal += FestivalEffect;
            return moodTotal;
        }

        private void AddFestivalEffect(double festivalEffect)
        {
            FestivalEffect += festivalEffect;
        }

        private void SetFirstWedding()
        {
            FirstWedding = true;
        }

        private void AddCurrentWeek()
        {
            CurrentWeek++;
        }

        private double GetIndustryParameter(string parameterName)
        {
            var minningIndustry = Industries.Minning;
            var productionIndustry = Industries.Production;
            var serviceIndustry = Industries.Service;

            return parameterName switch
            {
                ColonyStatNames.Industry_Minning_Available => 12 - minningIndustry.UnitCount,
                ColonyStatNames.Industry_Minning_Companies => minningIndustry.UnitCount,
                ColonyStatNames.Industry_Production_Companies => productionIndustry.UnitCount,
                ColonyStatNames.Industry_Service_Companies => serviceIndustry.UnitCount,
                ColonyStatNames.Industry_Service_Need => (PopulationTotal / 50.0) - serviceIndustry.UnitCount - 1.5,
                _ => throw new YagoUnknownTypeException(parameterName)
            };
        }

        private double MoodTotalBalanceCacl()
        {
            var codeOfLawsCoef = 1 + (((int)Settings.CodeOfLaws - 2) / 5.0);
            return -PopulationTotal * 0.01 * codeOfLawsCoef;
        }
    }
}