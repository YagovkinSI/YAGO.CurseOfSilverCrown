using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using YAGO.World.Domain.YagoEntities;
using YAGO.World.Domain.YagoEntities.Enums;
using YAGO.World.Infrastructure.APIModels;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Domains;
using YAGO.World.Infrastructure.Helpers.Map.Routes;
using YAGO.World.Infrastructure.Helpers.War;

namespace YAGO.World.Infrastructure.Helpers
{
    public static class KingdomHelper
    {
        public static Organization GetKingdomCapital(List<Organization> allOrganizations, Organization organization)
        {
            if (organization.SuzerainId == null)
                return organization;

            var suzerain = allOrganizations
                .Single(o => o.Id == organization.SuzerainId);
            return GetKingdomCapital(allOrganizations, suzerain);
        }

        public static Organization GetKingdomCapital(this DbSet<Organization> organizationsDbSet, Organization organization)
        {
            if (organization.SuzerainId == null)
                return organization;

            var suzerain = organizationsDbSet
                .Single(o => o.Id == organization.SuzerainId);
            return GetKingdomCapital(organizationsDbSet, suzerain);
        }

        public static bool IsSameKingdoms(this DbSet<Organization> organizationsDbSet, Organization organization1, Organization organization2)
        {
            var kingdomCapital1 = GetKingdomCapital(organizationsDbSet, organization1);
            var kingdomCapital2 = GetKingdomCapital(organizationsDbSet, organization2);
            return kingdomCapital1.Id == kingdomCapital2.Id;
        }

        public static Dictionary<string, MapElement> GetDomainColors(ApplicationDbContext context)
        {
            var alpha = "0.7";
            var allDomains = context.Domains.ToList();
            var array = new Dictionary<string, MapElement>();
            foreach (var domain in allDomains)
            {
                var id = domain.Id.ToString();
                var color = GetColor(context, allDomains, domain);
                var domainInfoText = GetDomainInfoText(context, allDomains, domain);
                var domainName = domain.Name;
                var yagoEntity = new YagoEntity(domain.Id, YagoEntityType.Province, domainName);
                array.Add(id, new MapElement(yagoEntity, color, alpha, domainInfoText));
            }

            var unknownEntity = new YagoEntity(0, YagoEntityType.Unknown, "Недоступные земли");
            array.Add("unknown_earth", new MapElement(unknownEntity, Color.Black, alpha, new List<string>()));
            return array;
        }

        private static List<string> GetDomainInfoText(ApplicationDbContext context, List<Organization> allDomains, Organization domain)
        {
            var domainInfoText = new List<string>();

            var unitTextInDomain = GetUnitTextInDomain(context, domain);
            domainInfoText.AddRange(unitTextInDomain);
            domainInfoText.Add("<hr>");

            var defenseTextInDomain = GetDefenseTextInDomain(context, domain);
            domainInfoText.AddRange(defenseTextInDomain);
            domainInfoText.Add("<hr>");

            var infoTextInDomain = GetInfoTextInDomain(allDomains, domain);
            domainInfoText.AddRange(infoTextInDomain);
            domainInfoText.Add("<hr>");

            var infoTextAboutDomainUnits = GetInfoTextAboutDomainUnits(context, domain);
            domainInfoText.AddRange(infoTextAboutDomainUnits);

            return domainInfoText;
        }

        private static List<string> GetUnitTextInDomain(ApplicationDbContext context, Organization domain)
        {
            var unitIds = domain.UnitsHere
                    .Select(u => u.Id);
            var unitHere = context.Units
                .Where(u => unitIds.Contains(u.Id))
                .ToList();
            var groups = unitHere
                .GroupBy(u => u.DomainId);

            var unitText = new List<string> { $"Отряды во владении:" };
            foreach (var group in groups)
            {
                var groupDomain = context.Domains.Find(group.Key);
                var text = $"- {groupDomain.Name} - {group.Sum(g => g.Warriors)} воинов";
                unitText.Add(text);
            }
            return unitText;
        }

        //TODO: Big
        private static List<string> GetDefenseTextInDomain(ApplicationDbContext context, Organization domain)
        {
            var defenseText = new List<string> { $"Данные по обороне владения {domain.Name}:" };

            var recomendedUnitCount = WarActionHelper.CalcRecomendedUnitCount(context, domain);
            var recomendedUnitCountText = $"- рекомендуется воинов для вторжения - {recomendedUnitCount}";
            defenseText.Add(recomendedUnitCountText);

            var defenders = WarActionHelper.GetAllDefenderDomains(context, domain);
            var defendersText = $"- владение под защитой владений: {string.Join(", ", defenders.Select(d => d.Name))}";
            defenseText.Add(defendersText);

            var fortCoef = FortificationsHelper.GetFortCoef(domain.Fortifications);
            defenseText.Add($"- укрепления - {fortCoef}");

            var unitsInDomain = domain.UnitsHere.Sum(u => u.Warriors);
            var unitsInDomainText = $"- защитников во владении: {unitsInDomain} воинов";
            defenseText.Add(unitsInDomainText);

            var defenderIds = defenders.Select(d => d.Id);
            var neibourIds = RouteHelper.GetNeighbors(context, domain.Id)
                .Select(d => d.Id);
            var unitsInNeibourDomains = context.Units
                .Where(u => defenderIds.Contains(u.DomainId))
                .Where(u => neibourIds.Contains(u.PositionDomainId.Value))
                .Sum(u => u.Warriors);
            var unitsInNeibourDomainText = $"- защитников в неделе пути: {unitsInNeibourDomains + unitsInDomain} воинов";
            defenseText.Add(unitsInNeibourDomainText);

            var allDefendersUnit = context.Units
                .Where(u => defenderIds.Contains(u.DomainId))
                .Sum(u => u.Warriors);
            var allDefendersUnitText = $"- защитников всего: {allDefendersUnit} воинов";
            defenseText.Add(allDefendersUnitText);

            return defenseText;
        }

        private static List<string> GetInfoTextInDomain(List<Organization> allDomains, Organization domain)
        {
            var infoText = new List<string>
            {
                $"Данные о владени {domain.Name}:",
                $"- сюзерен - {domain.Suzerain?.Name ?? "отсутсвует"}",
                $"- столица королевства - {GetKingdomCapital(allDomains, domain).Name}",
                $"- количество вассалов - {domain.Vassals.Count()}",
                $"- имущество - {domain.InvestmentsShowed}",
                $"- собираемые налоги - {InvestmentsHelper.GetInvestmentTax(domain.Investments)}",
                $"- площадь владения - {domain.Size}",
            };

            return infoText;
        }

        private static List<string> GetInfoTextAboutDomainUnits(ApplicationDbContext context, Organization domain)
        {
            var units = context.Units
                .Where(u => u.DomainId == domain.Id)
                .ToList()
                .GroupBy(u => u.PositionDomainId)
                .OrderByDescending(u => u.Sum(i => i.Warriors));
            var infoText = new List<string> { $"Все отряды владения (воинов - {domain.WarriorCount}):" };
            foreach (var unit in units)
                infoText.Add($"- во владении {unit.First().Position.Name} - {unit.Sum(i => i.Warriors)}");

            return infoText;
        }

        public static List<int> GetAllDomainsIdInKingdoms(this DbSet<Organization> organizationsDbSet, Organization organization)
        {
            var kingdomCapital = GetKingdomCapital(organizationsDbSet, organization);

            return GetAllLevelVassalIds(organizationsDbSet, kingdomCapital.Id);
        }

        public static List<int> GetAllLevelVassalIds(this DbSet<Organization> organizationsDbSet, int suzerainId,
            List<int> currentList = null)
        {
            currentList ??= new List<int>();

            currentList.Add(suzerainId);

            var vassals = organizationsDbSet
                .Where(o => o.SuzerainId == suzerainId)
                .ToList();

            foreach (var vassal in vassals)
            {
                var ids = GetAllLevelVassalIds(organizationsDbSet, vassal.Id, currentList);
                currentList.AddRange(ids);
            }

            return currentList
                .Distinct()
                .ToList();
        }

        public static Color GetColor(ApplicationDbContext context, List<Organization> allDomains, Organization domain)
        {
            var count = allDomains.Count;
            var colorParts = (int)Math.Ceiling(Math.Pow(count, 1 / 3.0));
            var colorStep = 255 / (colorParts - 1);
            var colorCount = (int)Math.Pow(colorParts, 3);
            var sqrt = (int)Math.Floor(Math.Sqrt(colorCount));

            var capital = GetKingdomCapital(context.Domains, domain);

            var colorNum = (capital.Id % sqrt * (colorCount / sqrt)) + (capital.Id / sqrt);

            var colorR = colorNum % colorParts * colorStep;
            var colorG = colorNum / colorParts % colorParts * colorStep;
            var colorB = colorNum / colorParts / colorParts % colorParts * colorStep;

            return Color.FromArgb(colorR, colorG, colorB);
        }
    }
}
