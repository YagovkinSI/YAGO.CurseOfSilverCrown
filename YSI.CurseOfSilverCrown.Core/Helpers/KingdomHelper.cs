using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Commands;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Parameters;
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
            var allDomains = GetAllDomins(context);
            var array = new Dictionary<string, MapElement>();
            foreach (var domain in allDomains)
            {
                var name = $"domain_{domain.Id}";
                var color = KingdomHelper.GetColor(context, allDomains, domain);
                var domainFullName = GetDomainFullName(allDomains, domain);
                var domainInfoText = GetDomainInfoText(context, allDomains, domain);
                array.Add(name, new MapElement(domainFullName, color, alpha, domainInfoText));
            }

            for (var i = Constants.MaxPlayerCount + 1; i <= 108; i++)
            {
                array.Add($"domain_{i}", new MapElement("Недоступные земли", Color.Black, alpha, new List<string>()));
            }

            array.Add("unknown_earth", new MapElement("Недоступные земли", Color.Black, alpha, new List<string>()));
            return array;
        }

        private static List<Domain> GetAllDomins(ApplicationDbContext context)
        {
            return context.Domains
                .Where(d => d.Id <= Constants.MaxPlayerCount)
                .Include(p => p.Person)
                .Include("Person.User")
                .Include(p => p.Suzerain)
                .Include(p => p.Vassals)
                .Include(p => p.UnitsHere)
                .ToList();
        }

        private static List<string> GetDomainInfoText(ApplicationDbContext context, List<Domain> allDomains, Domain domain)
        {
            var domainInfoText = new List<string>();
            var unitTextInDomain = GetUnitTextInDomain(context, allDomains, domain);
            domainInfoText.AddRange(unitTextInDomain);
            var fortification = FortificationsHelper.GetDefencePercent(domain.Fortifications);
            domainInfoText.Add($"Укрепления - {fortification}%");
            domainInfoText.Add($"Площадь владения - {domain.MoveOrder}");
            return domainInfoText;
        }

        private static string GetDomainFullName(List<Domain> allDomains, Domain domain)
        {
            var king = KingdomHelper.GetKingdomCapital(allDomains, domain);
            var domainFullName = domain.SuzerainId == null
                    ? $"{domain.Name}"
                    : domain.SuzerainId == king.Id
                        ? $"{domain.Name} ({king.Name})"
                        : $"{domain.Name} ({domain.Suzerain.Name}, {king.Name})";
            return domainFullName;
        }

        private static List<string> GetUnitTextInDomain(ApplicationDbContext context, List<Domain> allDomains, Domain domain)
        {
            var unitIds = domain.UnitsHere
                    .Select(u => u.Id);
            var unitHere = context.Units
                .Include(u => u.Domain)
                .Where(u => unitIds.Contains(u.Id))
                .Where(u => u.InitiatorPersonId == u.Domain.PersonId)
                .ToList();
            var groups = unitHere
                .GroupBy(u => u.DomainId);

            var unitText = new List<string> { $"Воинов во владении - {unitHere.Sum(u => u.Warriors)}:" };
            foreach (var group in groups)
            {
                var groupDomain = context.Domains
                    .Include(d => d.Suzerain)
                    .Single(g => g.Id == group.Key);
                var domainName = GetDomainFullName(allDomains, groupDomain);
                var text = $"- из владения {domainName} - {group.Sum(g => g.Warriors)}";
                unitText.Add(text);
            }
            return unitText;
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
            if (domain.Person.User == null && domain.SuzerainId == null)
                return Color.Gray;

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
