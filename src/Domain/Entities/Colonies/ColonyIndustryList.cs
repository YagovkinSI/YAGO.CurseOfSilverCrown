using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Отрасли колонии
    /// </summary>
    public class ColonyIndustryList : IReadOnlyList<IIndustry>
    {
        private readonly List<BaseIndustry> _items = [];

        public AdministrativeIndustry Administrative => (AdministrativeIndustry)_items.Single(x => x is AdministrativeIndustry);
        public MinningIndustry Minning => (MinningIndustry)_items.Single(x => x is MinningIndustry);
        public ProductionIndustry Production => (ProductionIndustry)_items.Single(x => x is ProductionIndustry);
        public ServiceIndustry Service => (ServiceIndustry)_items.Single(x => x is ServiceIndustry);

        public int PopulationTotal => _items.Sum(x => x.Population);
        public int ZonesOccupiedTotal => _items.Sum(x => x.ZonesOccupied);
        public int SolarsIncomeTotal => _items.Sum(x => x.SolarsIncome);
        public int Count => _items.Count;
        public IIndustry this[int index] => _items[index];

        public ColonyIndustryList(
            AdministrativeIndustry administrativeIndustry,
            MinningIndustry minningIndustry,
            ProductionIndustry productionIndustry,
            ServiceIndustry serviceIndustry)
        {
            _items.Add(administrativeIndustry);
            _items.Add(minningIndustry);
            _items.Add(productionIndustry);
            _items.Add(serviceIndustry);
        }

        public void SetIndustryParameters(IReadOnlyList<KeyValueParameter> colonyParameters)
        {
            var (industryChanges, count) = FindIndustryChanges(colonyParameters);

            if (industryChanges != null)
            {
                var zonesOccupied = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.AreaCapacity_Occupied)?.Value ?? 0);
                var solarIncome = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Economic_Budget_Balance)?.Value ?? 0);
                var population = (int)(colonyParameters.FirstOrDefault(x => x.Name == ColonyStatNames.Population_Total)?.Value ?? 0);

                switch (industryChanges)
                {
                    case IndustryNameConstants.Minning:
                        Minning.AddCompany(count, zonesOccupied, solarIncome, population);
                        break;
                    case IndustryNameConstants.Production:
                        Production.AddCompany(count, zonesOccupied, solarIncome, population);
                        break;
                    case IndustryNameConstants.Service:
                        Service.AddCompany(count, zonesOccupied, solarIncome, population);
                        break;
                    default:
                        throw new YagoUnknownTypeException(industryChanges);
                }
            }
        }
        public IEnumerator<IIndustry> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
    }
}
