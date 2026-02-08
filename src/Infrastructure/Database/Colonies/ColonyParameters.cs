using System;
using System.Collections.Generic;
using YAGO.World.Domain.Colonies;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyParameters
    {
        public long ShipId { get; private set; }
        public CodeOfLaws StartGavernorType { get; }
        public IReadOnlyList<long> Companies { get; private set; }

        [Obsolete]
        public Dictionary<long, int> Contracts { get; set; }

        public ColonyParameters(
            long shipId,
            CodeOfLaws startGavernorType,
            IReadOnlyList<long> companies)
        {
            ShipId = shipId;
            StartGavernorType = startGavernorType;
            Companies = companies;
        }

        public void ContractsToCompanies()
        {
            var companies = new List<long>();
            foreach (var contract in Contracts) 
            { 
                for (var i = 0; i < contract.Value; i++)
                    companies.Add(contract.Key);
            }
            Companies = companies;
            Contracts.Clear();
        }
    }
}
