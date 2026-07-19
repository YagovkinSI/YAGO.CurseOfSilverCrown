using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies.Industries;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Отрасли колонии
    /// </summary>
    public class ColonyIndustryList : IReadOnlyList<IIndustry>
    {
        private readonly List<BaseIndustry> _items;

        public AdministrativeIndustry Administrative { get; }
        public MinningIndustry Minning { get; }
        public ProductionIndustry Production { get; }
        public ServiceIndustry Service { get; }

        public int PopulationTotal => _items.Sum(x => x.Population);
        public int ZonesOccupiedTotal => _items.Sum(x => x.ZonesOccupiedTotal);
        public double SolarsIncomeTotal => _items.Sum(x => x.SolarsIncome);
        public int UnitCount => _items.Sum(x => x.BuildingCount);

        public int Count => _items.Count;

        public IIndustry this[int index] => _items[index];

        public ColonyIndustryList(
            AdministrativeIndustry administrativeIndustry,
            MinningIndustry minningIndustry,
            ProductionIndustry productionIndustry,
            ServiceIndustry serviceIndustry)
        {
            Administrative = administrativeIndustry;
            Minning = minningIndustry;
            Production = productionIndustry;
            Service = serviceIndustry;

            _items = [Administrative, Minning, Production, Service];
        }

        public double GetIndustryParameter(string parameterName)
        {
            return parameterName switch
            {
                ColonyStatNames.Industry_Administrative_Companies_StateOwned => Administrative.StateOwnedBuildingCount,
                ColonyStatNames.Industry_Administrative_Companies_Private => Administrative.PrivateBuildingCount,

                ColonyStatNames.Industry_Minning_Available => Minning.UnitAvailable,
                ColonyStatNames.Industry_Minning_Companies_StateOwned => Minning.StateOwnedBuildingCount,
                ColonyStatNames.Industry_Minning_Companies_Private => Minning.PrivateBuildingCount,

                ColonyStatNames.Industry_Production_Companies_StateOwned => Production.StateOwnedBuildingCount,
                ColonyStatNames.Industry_Production_Companies_Private => Production.PrivateBuildingCount,

                ColonyStatNames.Industry_Service_Companies_StateOwned => Service.StateOwnedBuildingCount,
                ColonyStatNames.Industry_Service_Companies_Private => Service.PrivateBuildingCount,

                ColonyStatNames.Industry_Service_Need => Service.NeedCalculation(PopulationTotal),
                _ => throw new YagoUnknownTypeException(parameterName)
            };
        }
        
        public IEnumerator<IIndustry> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
