using System;
using YAGO.World.Domain.Entities.Episodes;

namespace YAGO.World.Domain.Entities.Colonies.Buildings
{
    public class ColonyMining : ColonyBuilding
    {
        public override ColonyBuildingType Type => ColonyBuildingType.Mining;

        public ColonyMining(int privateCount, int stateCount) : base(privateCount, stateCount)
        {
        }

        public override BuildingSettings GetSettings()
        {
            return new BuildingSettings(
                Type,
                "Шахтёрская бригада",
                ImageSet.MiningBrigade,
                [
                    "Компания откроет небольшой офис и наймёт бригаду лицензированных шахтёров " +
                    "с надёжным оборудованием коих сотни на Поясе.",
                    "Она будет заниматься добычей ресурсов на астероиде. Новые рабочие места и налоги."],
                cost: 1000,
                zonesOccupied: 2,
                population: 10,
                solarsIncome: 30);
        }

        public override (bool isBuildAvailable, string? reason) IsBuildAvailable(bool isPrivate, ColonyState colonyState)
        {
            var settings = GetSettings();
            if (colonyState.Slots[Slots.ColonySlotType.Modules].GetFree(colonyState) < settings.ZonesOccupied)
                return (false, "Недостаточно модулей на станции.");

            var slots = colonyState.Slots[Slots.ColonySlotType.Mining].GetFree(colonyState);
            if (slots < 1)
                return (false, "Недостаточно мест добаычи на астероиде.");

            if (isPrivate)
            {
                if (colonyState.Reforms[ColonyReformType.TaxLevel].Value +
                    colonyState.Reforms[ColonyReformType.SocialGuaranteesLevel].Value > 6)
                    return (false, "Добыча не рентабельна.");
                var competition = colonyState.GetAttractiveness() + slots - 6;
                if (competition < 1)
                    return (false, $"Возможно через ходов: {(int)Math.Ceiling((1 - competition) * 3)}");
            }
            else
            {
                if (colonyState.Resources[Resources.ColonyResourceType.Solars].Value < settings.Cost)
                    return (false, "Недостаточно Солар.");
            }

            return (true, null);
        }
    }
}
