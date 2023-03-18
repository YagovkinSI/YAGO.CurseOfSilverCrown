using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Helpers.Map.Routes;
using YSI.CurseOfSilverCrown.Core.Helpers.War;
using YSI.CurseOfSilverCrown.Core.APIModels;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class KingdomHelper
    {
        public static Domain GetKingdomCapital(List<Domain> allOrganizations, Domain organization)
        {
            if (organization.SuzerainId == null)
                return organization;

            var suzerain = allOrganizations
                .Single(o => o.Id == organization.SuzerainId);
            return GetKingdomCapital(allOrganizations, suzerain);
        }

        public static Domain GetKingdomCapital(this DbSet<Domain> organizationsDbSet, Domain organization)
        {
            if (organization.SuzerainId == null)
                return organization;

            var suzerain = organizationsDbSet
                .Single(o => o.Id == organization.SuzerainId);
            return GetKingdomCapital(organizationsDbSet, suzerain);
        }

        public static bool IsSameKingdoms(this DbSet<Domain> organizationsDbSet, Domain organization1, Domain organization2)
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
                var name = $"domain_{domain.Id}";
                var color = GetColor(context, allDomains, domain);
                var domainName = $"<a href=\"/Organizations/Details/{domain.Id}\">{domain.Name}</a>";
                var domainInfoText = GetDomainInfoText(context, allDomains, domain);
                array.Add(name, new MapElement(domainName, color, alpha, domainInfoText));
            }

            array.Add("unknown_earth", new MapElement("Недоступные земли", Color.Black, alpha, new List<string>()));
            return array;
        }

        private static List<string> GetDomainInfoText(ApplicationDbContext context, List<Domain> allDomains, Domain domain)
        {
            var domainInfoText = new List<string>();

            var unitTextInDomain = GetUnitTextInDomain(context, domain);
            domainInfoText.AddRange(unitTextInDomain);
            domainInfoText.Add("<hr>");

            var defenseTextInDomain = GetDefenseTextInDomain(context, domain);
            domainInfoText.AddRange(defenseTextInDomain);
            domainInfoText.Add("<hr>");

            var infoTextInDomain = GetInfoTextInDomain(context, allDomains, domain);
            domainInfoText.AddRange(infoTextInDomain);
            domainInfoText.Add("<hr>");

            var infoTextAboutDomainUnits = GetInfoTextAboutDomainUnits(context, domain);
            domainInfoText.AddRange(infoTextAboutDomainUnits);

            return domainInfoText;
        }

        private static List<string> GetUnitTextInDomain(ApplicationDbContext context, Domain domain)
        {
            var unitIds = domain.UnitsHere
                    .Select(u => u.Id);
            var unitHere = context.Units
                .Where(u => unitIds.Contains(u.Id))
                .Where(u => u.InitiatorCharacterId == u.Domain.OwnerId)
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
        private static List<string> GetDefenseTextInDomain(ApplicationDbContext context, Domain domain)
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

        private static List<string> GetInfoTextInDomain(ApplicationDbContext context, List<Domain> allDomains, Domain domain)
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

        private static List<string> GetInfoTextAboutDomainUnits(ApplicationDbContext context, Domain domain)
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

        public static List<int> GetAllDomainsIdInKingdoms(this DbSet<Domain> organizationsDbSet, Domain organization)
        {
            var kingdomCapital = GetKingdomCapital(organizationsDbSet, organization);

            return GetAllLevelVassalIds(organizationsDbSet, kingdomCapital.Id);
        }

        public static List<int> GetAllLevelVassalIds(this DbSet<Domain> organizationsDbSet, int suzerainId,
            List<int> currentList = null)
        {
            if (currentList == null)
                currentList = new List<int>();

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

        public static Color GetColor(ApplicationDbContext context, List<Domain> allDomains, Domain domain)
        {
            var count = allDomains.Count;
            var colorParts = (int)Math.Ceiling(Math.Pow(count, 1 / 3.0));
            var colorStep = 255 / (colorParts - 1);
            var colorCount = (int)Math.Pow(colorParts, 3);
            var sqrt = (int)Math.Floor(Math.Sqrt(colorCount));

            var capital = GetKingdomCapital(context.Domains, domain);
            var king = context.Characters
                .Single(p => p.Id == capital.OwnerId);

            var colorNum = (king.Id % sqrt * (colorCount / sqrt)) + (king.Id / sqrt);

            var colorR = colorNum % colorParts * colorStep;
            var colorG = (colorNum / colorParts) % colorParts * colorStep;
            var colorB = (colorNum / colorParts / colorParts) % colorParts * colorStep;

            return Color.FromArgb(colorR, colorG, colorB);
        }
    }
}
