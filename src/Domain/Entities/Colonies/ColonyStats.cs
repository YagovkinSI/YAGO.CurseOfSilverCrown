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
        /// <summary>
        /// Идентифиикатор корабля
        /// </summary>
        public long ShipId { get; private set; }

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
            long shipId,
            CodeOfLaws codeOfLaws,
            double solars,
            double festivalEffect,
            int currentWeek,
            bool firstWedding,
            int maintenance,
            int zones,
            ColonyIndustryList colonyIndustryList)
        {
            ShipId = shipId;
            CodeOfLaws = codeOfLaws;
            Solars = solars;
            FestivalEffect = festivalEffect;
            CurrentWeek = currentWeek;
            FirstWedding = firstWedding;
            Maintenance = maintenance;
            ZonesTotal = zones;
            Industries = colonyIndustryList;
        }

        public static ColonyStats CreateNew(
            CodeOfLaws gavernorType)
        {
            var colonyIndustryList = new ColonyIndustryList(
                minningIndustry: MinningIndustry.CreateNew(),
                productionIndustry: ProductionIndustry.CreateNew(),
                serviceIndustry: ServiceIndustry.CreateNew());
            return new ColonyStats(
                shipId: 1,
                codeOfLaws: gavernorType,
                solars: 1000,
                festivalEffect: 0,
                currentWeek: 0,
                firstWedding: false,
                maintenance: 100,
                zones: 140,
                colonyIndustryList);
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
                ColonyStatNames.Industry_Minning_Available => 12 - Industries.Minning.UnitCount,
                ColonyStatNames.Industry_Minning_Companies => Industries.Minning.UnitCount,
                ColonyStatNames.Industry_Production_Companies => Industries.Production.UnitCount,
                ColonyStatNames.Industry_Service_Companies => Industries.Service.UnitCount,
                ColonyStatNames.Industry_Service_Need => (PopulationTotal / 50.0) - Industries.Service.UnitCount - 1.5,
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

        public void IssueDecree(Decree decree)
        {
            if (Solars < -(decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Reserves)?.Value ?? 0))
                throw new YagoException("Недостаточно средств.");

            if (ZonesAvailable < -(decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.AreaCapacity_Occupied)?.Value ?? 0))
                throw new YagoException("Недостаточно секторов.");

            Solars += decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Reserves)?.Value ?? 0;
            FestivalEffect += decree.Parameters.FirstOrDefault(x => x.Name == ColonyStatNames.Mood_Total)?.Value ?? 0;
        }

        public void SetEpisodeParameters(IReadOnlyList<KeyValueParameter> colonyParameters, bool isCycleOver)
        {
            var solars = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Reserves);
            if (solars != null)
                Solars += (int)solars.Value;

            SetIndustryParameters(colonyParameters);

            var moodTotal = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Mood_Total);
            if (moodTotal != null)
                FestivalEffect += moodTotal.Value;

            var firstWedding = colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.FirstWedding);
            if (firstWedding != null)
                FirstWedding = true;

            if (isCycleOver)
                CurrentWeek++;
        }

        private void SetIndustryParameters(IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            var (industryChanges, count) = FindIndustryChanges(colonyParameters);

            if (industryChanges != null)
            {
                var zonesOccupied = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.AreaCapacity_Occupied)?.Value ?? 0);
                var solarIncome = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Budget_Balance)?.Value ?? 0);
                var population = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Population_Total)?.Value ?? 0);

                var industries = Industries;
                switch (industryChanges)
                {
                    case IndustryNameConstants.Minning:
                        industries.Minning.AddCompany(count, zonesOccupied, solarIncome, population);
                        break;
                    case IndustryNameConstants.Production:
                        industries.Production.AddCompany(count, zonesOccupied, solarIncome, population);
                        break;
                    case IndustryNameConstants.Service:
                        industries.Service.AddCompany(count, zonesOccupied, solarIncome, population);
                        break;
                    default:
                        throw new YagoUnknownTypeException(industryChanges);
                }
            }
        }

        private static (string? industryName, int count) FindIndustryChanges(IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            if (colonyParameters.Any(x => x.Name == ColonyStatNames.Industry_Minning_Companies))
                return (IndustryNameConstants.Minning, (int)colonyParameters.Single(x => x.Name == ColonyStatNames.Industry_Minning_Companies).Value);
            else if (colonyParameters.Any(x => x.Name == ColonyStatNames.Industry_Production_Companies))
                return (IndustryNameConstants.Production, (int)colonyParameters.Single(x => x.Name == ColonyStatNames.Industry_Production_Companies).Value);
            else if (colonyParameters.Any(x => x.Name == ColonyStatNames.Industry_Service_Companies))
                return (IndustryNameConstants.Service, (int)colonyParameters.Single(x => x.Name == ColonyStatNames.Industry_Service_Companies).Value);
            else
                return (null, 0);
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
            return moodTotal;
        }
    }
}
