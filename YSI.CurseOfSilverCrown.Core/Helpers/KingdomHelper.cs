using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.ViewModels;

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

            var unitTextInDomain = GetUnitTextInDomain(context, allDomains, domain);
            domainInfoText.AddRange(unitTextInDomain);
            domainInfoText.Add("<hr>");

            var defenseTextInDomain = GetDefenseTextInDomain(context, allDomains, domain);
            domainInfoText.AddRange(defenseTextInDomain);
            domainInfoText.Add("<hr>");

            var infoTextInDomain = GetInfoTextInDomain(context, allDomains, domain);
            domainInfoText.AddRange(infoTextInDomain);

            return domainInfoText;
        }

        private static List<string> GetUnitTextInDomain(ApplicationDbContext context, List<Domain> allDomains, Domain domain)
        {
            var unitIds = domain.UnitsHere
                    .Select(u => u.Id);
            var unitHere = context.Units
                .Where(u => unitIds.Contains(u.Id))
                .Where(u => u.InitiatorPersonId == u.Domain.PersonId)
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

        private static List<string> GetDefenseTextInDomain(ApplicationDbContext context, List<Domain> allDomains, Domain domain)
        {
            var defenseText = new List<string> { "Данные по обороне владения:" };

            var defender = domain.Suzerain ?? domain;
            var defenderText = domain.Suzerain == null
                ? "- независимо, защищается своими силами"
                : $"- под защитой сюзерена из владения {domain.Suzerain.Name}";
            defenseText.Add(defenderText);

            var allUnitDefender = defender.WarriorCount;
            defenseText.Add($"- всего воинов у владения {defender.Name} - {allUnitDefender}");

            var fortificationCoef = FortificationsHelper.GetDefencePercent(domain.Fortifications);
            defenseText.Add($"- укрепления владения {domain.Name} - {fortificationCoef} %");

            var unitInDomain = defender.Units
                .Where(u => u.PositionDomainId == domain.Id)
                .Sum(u => u.Warriors);
            var defenseInDomain = unitInDomain * fortificationCoef / 100.0;
            defenseText.Add($"- защита с учетом войск во владении - {defenseInDomain}");

            defenseText.Add($"- воинов защитника вне владения - {allUnitDefender - unitInDomain}");
            defenseText.Add($"- возможная общая защита - {allUnitDefender - unitInDomain + defenseInDomain}");

            return defenseText;
        }

        private static List<string> GetInfoTextInDomain(ApplicationDbContext context, List<Domain> allDomains, Domain domain)
        {
            var infoText = new List<string> { $"Данные о владени {domain.Name}:" };

            infoText.Add($"- имущество - {domain.InvestmentsShowed}");
            infoText.Add($"- собираемые налоги - {InvestmentsHelper.GetInvestmentTax(domain.Investments)}");
            infoText.Add($"- количество вассалов - {domain.Vassals.Count()}");
            infoText.Add($"- площадь владения - {domain.MoveOrder}");
            infoText.Add($"- столица королевства - {GetKingdomCapital(allDomains, domain).Name}");

            return infoText;
        }

        public static List<int> GetAllDomainsIdInKingdoms(this DbSet<Domain> organizationsDbSet, Domain organization)
        {
            var kingdomCapital = GetKingdomCapital(organizationsDbSet, organization);

            return GetAllLevelVassalIds(organizationsDbSet, kingdomCapital.Id);
        }

        private static List<int> GetAllLevelVassalIds(this DbSet<Domain> organizationsDbSet, int suzerainId, List<int> currentList = null)
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
            var king = context.Persons
                .Single(p => p.Id == capital.PersonId);

            var colorNum = (king.Id % sqrt * (colorCount / sqrt)) + (king.Id / sqrt);

            var colorR = colorNum % colorParts * colorStep;
            var colorG = (colorNum / colorParts) % colorParts * colorStep;
            var colorB = (colorNum / colorParts / colorParts) % colorParts * colorStep;

            return Color.FromArgb(colorR, colorG, colorB);
        }
    }
}
