using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.Buildings
{
    public static class BuildingDataset
    {
        public static IReadOnlyList<Building> Get()
        {
            return
            [
                GetAdministrative(),
                GetMining(),
                GetService(),
                GetProduction(),
            ];
        }

        public static Building GetAdministrative()
        {
            return new Building(
                BuildingType.Administrative,
                cost: 3000,
                zonesOccupied: 10,
                population: 30,
                solarsIncome: -10);
        }

        public static Building GetMining()
        {
            return new Building(
                BuildingType.Mining,
                cost: 1000,
                zonesOccupied: 2,
                population: 10,
                solarsIncome: 30);
        }

        public static Building GetService()
        {
            return new Building(
                BuildingType.Service,
                cost: 1000,
                zonesOccupied: 3,
                population: 10,
                solarsIncome: 12);
        }

        public static Building GetProduction()
        {
            return new Building(
                BuildingType.Production,
                cost: 2500,
                zonesOccupied: 5,
                population: 25,
                solarsIncome: 35);
        }
    }
}
