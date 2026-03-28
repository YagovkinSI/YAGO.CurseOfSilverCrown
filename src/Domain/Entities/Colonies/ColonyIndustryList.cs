using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies.Industries;

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
        private readonly Dictionary<string, BaseIndustry> _industries = [];

        /// <summary>
        /// Отрасль добычи ресурсов
        /// </summary>
        public BaseIndustry Minning => _industries[IndustryNameConstants.Minning];

        /// <summary>
        /// Отрасль производства продукции
        /// </summary>
        public BaseIndustry Production => _industries[IndustryNameConstants.Production];

        /// <summary>
        /// Отрасль оказания услуг
        /// </summary>
        public BaseIndustry Service => _industries[IndustryNameConstants.Service];

        public int PopulationTotal => _industries.Values.Sum(x => x.Population);
        public int ZonesOccupiedTotal => _industries.Values.Sum(x => x.ZonesOccupied);
        public int SolarsIncomeTotal => _industries.Values.Sum(x => x.SolarsIncome);

        public ColonyIndustryList(
            MinningIndustry minningIndustry,
            ProductionIndustry productionIndustry,
            ServiceIndustry serviceIndustry)
        {
            _industries.Add(IndustryNameConstants.Minning, minningIndustry);
            _industries.Add(IndustryNameConstants.Production, productionIndustry);
            _industries.Add(IndustryNameConstants.Service, serviceIndustry);
        }

        internal void AddCompany(string industryName, int count, int zonesOccupied, int solarIncome, int population)
        {
            var industry = _industries[industryName];
            industry.AddCompany(count, zonesOccupied, solarIncome, population);
        }
    }
}
