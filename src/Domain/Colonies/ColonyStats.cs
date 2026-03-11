using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Colonies.Parameters;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Colonies
{
    /// <summary>
    /// Динамические параметры колонии (рассчитываемые игрой)
    /// </summary>
    public class ColonyStats
    {
        /// <summary>
        /// Солары
        /// </summary>
        public double Solars { get; private set; }

        /// <summary>
        /// Эффект от праздника
        /// </summary>
        public double FestivalEffect { get; private set; }

        /// <summary>
        /// Контракты колонии
        /// </summary>
        public IReadOnlyList<long> CompanyIds { get; private set; }

        /// <summary>
        /// Текущая неделя
        /// </summary>
        public int CurrentWeek { get; private set; }

        public ColonyStats(
            double solars,
            double festivalEffect,
            IReadOnlyList<long> companyIds,
            int currentWeek)
        {
            Solars = solars;
            FestivalEffect = festivalEffect;
            CompanyIds = companyIds;
            CurrentWeek = currentWeek;
        }

        public static ColonyStats CreateNew()
        {
            return new ColonyStats(
                solars: 1000,
                festivalEffect: 0,
                companyIds: [2, 2, 2, 2],
                currentWeek: 0);
        }

        public void AddSolars(double value)
        {
            Solars += value;
        }

        public void AddCompany(long companyId)
        {
            var companyIds = CompanyIds.ToList();
            companyIds.Add(companyId);
            CompanyIds = companyIds;
        }

        public void ValidateContracts(ColonyCompanies companies)
        {
            if (companies.Companies.Count != CompanyIds.Count)
                throw new YagoException("Несовпадение количества Colony.Сontracts и Сontracts");

            if (!CompanyIds
                    .OrderBy(x => x)
                    .SequenceEqual(companies.Companies.Select(x => x.Id).OrderBy(x => x)))
            {
                throw new YagoException("Несовпадение Colony.Сontracts и Сontracts");
            }
        }

        public void AddFestivalEffect(double effect)
        {
            FestivalEffect += effect;
        }

        internal void AddWeek()
        {
            CurrentWeek++;
        }
    }
}
