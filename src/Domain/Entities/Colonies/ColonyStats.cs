using System;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Colonies
{
    public class ColonyStats
    {
        /// <summary>
        /// Установленные законы
        /// </summary>
        public CodeOfLaws CodeOfLaws { get; }

        /// <summary>
        /// Солары
        /// </summary>
        public double Solars { get; private set; }

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

        /// <summary>
        /// Содержание станции
        /// </summary>
        public int Maintenance { get; }

        /// <summary>
        /// Максимальная прощадь под застройку
        /// </summary>
        public int ZonesTotal { get; }

        /// <summary>
        /// Отрасли колонии
        /// </summary>
        public ColonyIndustryList Industries { get; }
        public int PopulationTotal => Industries.PopulationTotal + 20;
        public int ZonesOccupied => Industries.ZonesOccupiedTotal + 20;
        public int ZonesAvailable => ZonesTotal - ZonesOccupied;
        public double BudgetBalance => Industries.SolarsIncomeTotal - Maintenance;

        public ColonyStats(
            CodeOfLaws codeOfLaws,
            double solars,
            double festivalEffect,
            int currentWeek,
            bool firstWedding,
            int maintenance,
            int zones,
            ColonyIndustryList colonyIndustryList)
        {
            CodeOfLaws = codeOfLaws;
            Solars = solars;
            FestivalEffect = festivalEffect;
            CurrentWeek = currentWeek;
            FirstWedding = firstWedding;
            Maintenance = maintenance;
            ZonesTotal = zones;
            Industries = colonyIndustryList;
        }

        public double GetGameParameter(string parameterName)
        {
            return parameterName switch
            {
                ColonyStatNames.Economic_Reserves => Solars,
                ColonyStatNames.Mood_Total => MoodTotalCacl(),
                ColonyStatNames.Mood_Total_Balance => MoodTotalBalanceCacl(),
                ColonyStatNames.Population_Total => PopulationTotal,
                ColonyStatNames.AreaCapacity_Occupied => ZonesOccupied,
                ColonyStatNames.Economic_Budget_Balance => BudgetBalance,
                ColonyStatNames.Industry_Minning_Available => 12 - Industries.Minning.CompanyCount,
                ColonyStatNames.Industry_Minning_Companies => Industries.Minning.CompanyCount,
                ColonyStatNames.Industry_Production_Companies => Industries.Production.CompanyCount,
                ColonyStatNames.Industry_Service_Companies => Industries.Service.CompanyCount,
                ColonyStatNames.Industry_Service_Need => (PopulationTotal / 50.0) - Industries.Service.CompanyCount - 1.5,
                ColonyStatNames.AreaCapacity_Total => ZonesTotal,
                ColonyStatNames.AreaCapacity_Available => ZonesAvailable,
                ColonyStatNames.Laws_CodeOfLaws => (double)CodeOfLaws,
                ColonyStatNames.Laws_CodeOfLaws_HighTax => CodeOfLaws == CodeOfLaws.Capitalist ? 1 : 0,
                ColonyStatNames.Laws_CodeOfLaws_HighStandart => CodeOfLaws == CodeOfLaws.Humanist ? 1 : 0,
                ColonyStatNames.Attractiveness_Total => AttractivenessTotalCalc(),
                ColonyStatNames.FirstWedding => FirstWedding ? 1 : 0,
                ColonyStatNames.CurrentWeek => CurrentWeek,
                _ => throw new YagoUnknownTypeException(parameterName)
            };
        }

        private double MoodTotalBalanceCacl()
        {
            var codeOfLawsCoef = 1 + (((int)CodeOfLaws - 2) / 5.0);
            return -PopulationTotal * 0.01 * codeOfLawsCoef;
        }

        public double AttractivenessTotalCalc()
        {
            var defaultValue = 100;
            var taxEffect = -30 * (int)CodeOfLaws;
            var standartsEffect = -30 * (3 - (int)CodeOfLaws);
            var stabilityEffect = Math.Min(50, CurrentWeek / 10.0);
            return Math.Clamp(defaultValue + taxEffect + standartsEffect + stabilityEffect, -100, 100);
        }

        public double MoodTotalCacl()
        {
            var moodTotal = 52.0;
            moodTotal += FestivalEffect;
            return Math.Clamp(moodTotal, 2, 98);
        }

        internal void AddSolars(double value)
        {
            Solars += value;
        }

        internal void AddFestivalEffect(double effect)
        {
            FestivalEffect += effect;
        }

        internal void AddWeek()
        {
            CurrentWeek++;
        }

        internal void SetFirstWedding()
        {
            FirstWedding = true;
        }

        internal void AddCompany(string industryName, int count, int zonesOccupied, int solarIncome, int population)
        {
            Industries.AddCompany(industryName, count, zonesOccupied, solarIncome, population);
        }
    }
}
