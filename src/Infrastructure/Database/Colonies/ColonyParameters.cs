using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    public class ColonyParameters
    {
        public long ShipId { get; private set; }
        public CodeOfLaws StartGavernorType { get; }
        [Obsolete]
        public IReadOnlyList<long> Companies { get; private set; }
        public double FestivalEffect { get; private set; }
        public bool FirstWedding { get; private set; }
        public int CurrentWeek { get; private set; }
        public int Maintenance { get; private set; }
        public int Zones { get; private set; }
        public IndustryEntity MinningIndustry { get; private set; }
        public IndustryEntity ProductionIndustry { get; private set; }
        public IndustryEntity ServiceIndustry { get; private set; }

        public ColonyParameters(
            long shipId,
            CodeOfLaws startGavernorType,
            IReadOnlyList<long> companies,
            double festivalEffect,
            bool firstWedding,
            int currentWeek,
            int maintenance,
            int zones,
            IndustryEntity minningIndustry,
            IndustryEntity productionIndustry,
            IndustryEntity serviceIndustry)
        {
            ShipId = shipId;
            StartGavernorType = startGavernorType;
            Companies = companies;
            FestivalEffect = festivalEffect;
            FirstWedding = firstWedding;
            CurrentWeek = currentWeek;
            Maintenance = maintenance;
            Zones = zones;
            MinningIndustry = minningIndustry;
            ProductionIndustry = productionIndustry;
            ServiceIndustry = serviceIndustry;
        }

        internal void SetShipParameters()
        {
            Maintenance = 100;
            Zones = 140;
        }

        internal void SetIndustry(IReadOnlyList<long> companies)
        {
            MinningIndustry = new IndustryEntity() { Name = IndustryNameConstants.Minning };
            ProductionIndustry = new IndustryEntity() { Name = IndustryNameConstants.Production };
            ServiceIndustry = new IndustryEntity() { Name = IndustryNameConstants.Service };

            foreach (long companyId in companies)
            {
                switch (companyId)
                {
                    case 1:
                        MinningIndustry.CompanyCount += 1;
                        MinningIndustry.ZonesOccupied += 3;
                        MinningIndustry.SolarsIncome += 20;
                        MinningIndustry.Population += 10;
                        break;
                    case 2:
                        MinningIndustry.CompanyCount += 1;
                        MinningIndustry.ZonesOccupied += 3;
                        MinningIndustry.SolarsIncome += 30;
                        MinningIndustry.Population += 15;
                        break;
                    case 3:
                        MinningIndustry.CompanyCount += 1;
                        MinningIndustry.ZonesOccupied += 4;
                        MinningIndustry.SolarsIncome += 50;
                        MinningIndustry.Population += 30;
                        break;
                    case 4:
                        ProductionIndustry.CompanyCount += 1;
                        ProductionIndustry.ZonesOccupied += 5;
                        ProductionIndustry.SolarsIncome += 25;
                        ProductionIndustry.Population += 25;
                        break;
                    case 5:
                        ServiceIndustry.CompanyCount += 1;
                        ServiceIndustry.ZonesOccupied += 3;
                        ServiceIndustry.SolarsIncome += 10;
                        ServiceIndustry.Population += 10;
                        break;
                }

            }
        }

        internal void SetIndustryNames()
        {
            MinningIndustry.Name = IndustryNameConstants.Minning;
            ProductionIndustry.Name = IndustryNameConstants.Production;
            ServiceIndustry.Name = IndustryNameConstants.Service;
        }
    }
}
