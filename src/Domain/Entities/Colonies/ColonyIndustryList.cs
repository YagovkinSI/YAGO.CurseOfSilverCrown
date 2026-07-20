using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Entities.Colonies.Industries;

namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Отрасли колонии
    /// </summary>
    public class ColonyIndustryList : IReadOnlyList<BaseIndustry>
    {
        private readonly List<BaseIndustry> _items;

        public AdministrativeIndustry Administrative { get; }
        public MinningIndustry Minning { get; }
        public ProductionIndustry Production { get; }
        public ServiceIndustry Service { get; }

        public int BuildingCount => _items.Sum(x => x.BuildingCount);

        public int Count => _items.Count;

        public BaseIndustry this[int index] => _items[index];

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
        
        public IEnumerator<BaseIndustry> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
