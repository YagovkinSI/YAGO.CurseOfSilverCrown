using System.Collections.Generic;
using System.Linq;

namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Отрасли колонии
    /// </summary>
    public class ColonyIndustryList
    {
        /// <summary>
        /// Отрасли колонии в виде словаря, где ключ - имя отрасли из IndustryNameConstants
        /// </summary>
        private readonly Dictionary<string, Industry> _industries = [];

        /// <summary>
        /// Отрасль добычи ресурсов
        /// </summary>
        public Industry Minning => _industries[IndustryNameConstants.Minning];

        /// <summary>
        /// Отрасль производства продукции
        /// </summary>
        public Industry Production => _industries[IndustryNameConstants.Production];

        /// <summary>
        /// Отрасль оказания услуг
        /// </summary>
        public Industry Service => _industries[IndustryNameConstants.Service];

        public int PopulationTotal => _industries.Values.Sum(x => x.Population);
        public int ZonesOccupiedTotal => _industries.Values.Sum(x => x.ZonesOccupied);
        public int SolarsIncomeTotal => _industries.Values.Sum(x => x.SolarsIncome);

        public ColonyIndustryList(
            Industry minningIndustry,
            Industry productionIndustry,
            Industry serviceIndustry)
        {
            _industries.Add(minningIndustry.Name, minningIndustry);
            _industries.Add(productionIndustry.Name, productionIndustry);
            _industries.Add(serviceIndustry.Name, serviceIndustry);
        }

        internal void AddCompany(string industryName, int count, int zonesOccupied, int solarIncome, int population)
        {
            var industry = _industries[industryName];
            industry.AddCompany(count, zonesOccupied, solarIncome, population);
        }
    }
}
