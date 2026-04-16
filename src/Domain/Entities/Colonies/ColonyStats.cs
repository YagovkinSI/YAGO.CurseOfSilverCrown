using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.Decrees;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.ValueTypes;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyStats
    {
        public ColonySettings Settings { get; }
        public ColonyResources Resources { get; }
        public ColonyIndustryList Industries { get; }

        /// <summary>
        /// Настроение
        /// </summary>
        public LimitedDouble MoodTotal { get; private set; }

        /// <summary>
        /// Текущая неделя
        /// </summary>
        public int CurrentWeek { get; private set; }

        /// <summary>
        /// Количество пройденых эпизодов
        /// </summary>
        public int EpisodeCount { get; private set; }

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
            double moodTotal,
            int currentWeek,
            int episodeCount,
            bool firstWedding)
        {
            Settings = settings;
            Resources = resources;
            Industries = industries;
            MoodTotal = new LimitedDouble(moodTotal, 0, 100);
            CurrentWeek = currentWeek;
            EpisodeCount = episodeCount;
            FirstWedding = firstWedding;
        }

        public static ColonyStats CreateNew()
        {
            var colonySettings = ColonySettings.CreateNew();
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
                moodTotal: 52,
                currentWeek: 0,
                episodeCount: 0,
                firstWedding: false);
        }

        public double GetGameParameter(string parameterName)
        {
            return parameterName.StartsWith(ColonyStatGroupNames.Industry)
                ? Industries.GetIndustryParameter(parameterName)
                : parameterName switch
                {
                    ColonyStatNames.Economic_Reserves => Resources.Solars,
                    ColonyStatNames.Mood_Total => MoodTotal.Value,
                    ColonyStatNames.Mood_Total_Balance => MoodTotalBalanceCacl(),
                    ColonyStatNames.Population_Total => PopulationTotal,
                    ColonyStatNames.AreaCapacity_Occupied => ZonesOccupied,
                    ColonyStatNames.Economic_Budget_Balance => BudgetBalance,
                    ColonyStatNames.AreaCapacity_Total => Resources.ZonesTotal,
                    ColonyStatNames.AreaCapacity_Available => ZonesAvailable,
                    ColonyStatNames.Laws_TaxLevel => Settings.TaxLevel,
                    ColonyStatNames.Laws_SocialGuaranteesLevel => Settings.SocialGuaranteesLevel,
                    ColonyStatNames.Attractiveness_Total => AttractivenessTotalCalc(),
                    ColonyStatNames.FirstWedding => FirstWedding ? 1 : 0,
                    ColonyStatNames.CurrentWeek => CurrentWeek,
                    ColonyStatNames.EpisodeCount => EpisodeCount,
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
            MoodTotal += decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Mood_Total)?.Value ?? 0;
        }

        public void SetEpisodeParameters(IReadOnlyList<KeyValueParameter> colonyParameters, bool isCycleOver)
        {
            var solars = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Reserves);
            if (solars != null)
                Resources.AddSolars((int)solars.Value);

            Industries.SetIndustryParameters(colonyParameters);

            var moodTotal = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Mood_Total);
            if (moodTotal != null)
                MoodTotal += moodTotal.Value;

            var firstWedding = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.FirstWedding);
            if (firstWedding != null)
                FirstWedding = true;

            EpisodeCount++;

            if (isCycleOver)
                CurrentWeek++;
        }

        public double AttractivenessTotalCalc()
        {
            var defaultValue = 100;
            var taxEffect = -15 * Settings.TaxLevel;
            var standartsEffect = -15 * Settings.SocialGuaranteesLevel;
            var stabilityEffect = Math.Min(50, CurrentWeek / 10.0);
            return Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }

        private double MoodTotalBalanceCacl()
        {
            var codeOfLawsCoef = 1 + ((Settings.SocialGuaranteesLevel - 3) / 10.0);
            return -PopulationTotal * 0.01 * codeOfLawsCoef;
        }
    }
}