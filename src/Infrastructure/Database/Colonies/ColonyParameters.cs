using System;
using System.Collections.Generic;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Plots;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyParameters
    {
        public long ShipId { get; private set; }
        public CodeOfLaws StartGavernorType { get; }
        public IReadOnlyList<long> Companies { get; private set; }
        public double FestivalEffect { get; private set; }
        public Plot Plot { get; private set; }

        [Obsolete]
        public Dictionary<long, int> Contracts { get; set; }

        public ColonyParameters(
            long shipId,
            CodeOfLaws startGavernorType,
            IReadOnlyList<long> companies,
            double festivalEffect,
            Plot plot)
        {
            ShipId = shipId;
            StartGavernorType = startGavernorType;
            Companies = companies;
            FestivalEffect = festivalEffect;
            Plot = plot;
        }

        [Obsolete]
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

        internal void SetPlot(Plot plot)
        {
            Plot = plot;
        }
    }
}
