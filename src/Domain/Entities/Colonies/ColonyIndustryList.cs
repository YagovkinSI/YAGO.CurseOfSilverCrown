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
        public MinningIndustry Minning => _industries[IndustryNameConstants.Minning] as MinningIndustry;

        /// <summary>
        /// Отрасль производства продукции
        /// </summary>
        public ProductionIndustry Production => _industries[IndustryNameConstants.Production] as ProductionIndustry;

        /// <summary>
        /// Отрасль оказания услуг
        /// </summary>
        public ServiceIndustry Service => _industries[IndustryNameConstants.Service] as ServiceIndustry;

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
    }
}
