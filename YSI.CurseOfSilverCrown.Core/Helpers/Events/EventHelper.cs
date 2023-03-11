using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.APIModels;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Domains;
using YSI.CurseOfSilverCrown.Core.Database.EventDomains;
using YSI.CurseOfSilverCrown.Core.Database.Events;
using YSI.CurseOfSilverCrown.Core.Database.Users;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Events
{
    public static class EventHelper
    {
        public static async Task<List<List<string>>> GetTextStories(ApplicationDbContext context, List<Event> eventStories,
            HistoryFilter historyFilter = null)
        {
            var textStories = new List<List<string>>();
            var maxCount = 200;
            var currentCount = 0;
            foreach (var eventStory in eventStories)
            {
                var turn = GameSessionHelper.GetName(context, eventStory.Turn);
                var (textStory, type) = await GetTextStoryAsync(context, eventStory);
                if (!historyFilter?.ResultTypes.Contains(type) ?? false)
                    continue;
                var pair = new List<string> { turn };
                pair.AddRange(textStory);
                textStories.Add(pair);
                currentCount++;
                if (currentCount >= maxCount)
                    break;
            }
            return textStories;
        }

        public static async Task<List<List<string>>> GetHistory(ApplicationDbContext context, HistoryFilter historyFilter,
            User currentUser)
        {
            var currentTurn = context.Turns.Single(t => t.IsActive);
            var firstTurnId = currentTurn.Id - historyFilter.Turns;

            var domainIds = GetDomainIds(context, historyFilter.Region, currentUser);

            var organizationEventStories = await context.OrganizationEventStories
               .Where(o => historyFilter.DomainId == null || o.DomainId == historyFilter.DomainId.Value)
               .Where(o => historyFilter.Turns == int.MaxValue || o.TurnId >= firstTurnId)
               .Where(o => historyFilter.Important == 0 || o.Importance >= historyFilter.Important)
               .Where(o => domainIds == null || domainIds.Contains(o.DomainId))
               .ToListAsync();

            var eventStories = GetEventStories(organizationEventStories);

            return await GetTextStories(context, eventStories, historyFilter);
        }

        private static List<int> GetDomainIds(ApplicationDbContext context, int region, User currentUser)
        {
            var presonId = currentUser?.PersonId;
            var userDoamin = context.Domains
                .FirstOrDefault(d => d.PersonId == presonId);
            if (userDoamin == null)
                return null;

            switch (region)
            {
                case 0:
                    return null;
                case 1:
                    return context.Domains.GetAllDomainsIdInKingdoms(userDoamin);
                case 2:
                    return context.Domains.GetAllLevelVassalIds(userDoamin.Id);
                case 3:
                    var list = new List<int> { userDoamin.Id };
                    list.AddRange(userDoamin.Vassals.Select(v => v.Id));
                    return list;
                case 4:
                    return new List<int> { userDoamin.Id };
                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<List<List<string>>> GetTopHistory(ApplicationDbContext context, int? domainId = null)
        {
            var currentTurn = context.Turns
                .Single(t => t.IsActive);
            var top = new List<Event>();
            var countFromLastWeek = 0;
            var countFromLastDay = 0;
            var count = 0;
            while (count < 10)
            {
                var maxTurnId = 10 - count <= 3 - countFromLastDay
                    ? currentTurn.Id - 1
                    : 10 - count <= 6 - countFromLastWeek
                        ? currentTurn.Id - 7
                        : 0;
                var eventDomain = GetTopEventDomain(context, top.Select(e => e.Id), maxTurnId, domainId);
                if (eventDomain?.EventStory != null)
                    top.Add(eventDomain.EventStory);
                countFromLastDay += eventDomain?.TurnId >= currentTurn.Id - 1 ? 1 : 0;
                countFromLastWeek += eventDomain?.TurnId >= currentTurn.Id - 7 ? 1 : 0;
                count++;
            }
            top = top
                .OrderByDescending(e => e.Id)
                .OrderByDescending(e => e.TurnId)
                .ToList();
            return await GetTextStories(context, top);
        }

        private static EventDomain GetTopEventDomain(ApplicationDbContext context, IEnumerable<int> filterEventIds, int maxTurnId, int? domainId)
        {
            return context.OrganizationEventStories
                    .Where(o => o.TurnId >= maxTurnId)
                    .Where(o => domainId == null || o.DomainId == domainId)
                    .Where(o => !filterEventIds.Contains(o.EventStoryId))
                    .OrderByDescending(o => o.Importance)
                    .FirstOrDefault();
        }

        private static List<Event> GetEventStories(List<EventDomain> domainEventStories)
        {
            return domainEventStories
                    .Select(o => o.EventStory)
                    .Distinct()
                    .OrderByDescending(o => o.Id)
                    .OrderByDescending(o => o.TurnId)
                    .ToList();
        }

        private static async Task<(List<string>, EventType)> GetTextStoryAsync(
            ApplicationDbContext context, Event eventStory)
        {
            var text = new List<string>();
            var eventStoryResult = JsonConvert.DeserializeObject<EventJson>(eventStory.EventStoryJson);
            var type = eventStoryResult.EventResultType;

            var ids = eventStoryResult.Organizations.Select(e => e.Id);
            var allOrganizations = await context.Domains
                        .Where(c => ids.Contains(c.Id))
                        .ToListAsync();

            var eventStoryCard = GetEventStoryCard(eventStoryResult, allOrganizations);
            FillEventMainText(text, eventStoryResult, eventStoryCard);
            FillEventParameters(text, eventStoryResult, allOrganizations);
            return (text, type);
        }

        private static EventJsonDomainNameHelper GetEventStoryCard(EventJson eventStoryResult,
            List<Domain> allOrganizations)
        {
            var eventStoryCard = new EventJsonDomainNameHelper();
            foreach (var domain in eventStoryResult.Organizations)
            {
                var type = domain.EventOrganizationType;
                var name = allOrganizations.Single(d => d.Id == domain.Id).Name;
                eventStoryCard.TryAddName(type, name);
            }
            return eventStoryCard;
        }

        private static void FillEventMainText(List<string> text, EventJson eventStoryResult,
            EventJsonDomainNameHelper card)
        {
            var mainText = EventTextHelper.GetEventText(eventStoryResult.EventResultType, card);
            text.Add(mainText);
        }

        private static void FillEventParameters(List<string> text, EventJson eventStoryResult,
            List<Domain> allOrganizations)
        {
            foreach (var eventOrganization in eventStoryResult.Organizations)
            {
                var changes = eventOrganization.EventOrganizationChanges;
                if (changes.Count == 0)
                    continue;
                var organization = allOrganizations.First(o => o.Id == eventOrganization.Id);
                text.Add($"\r\n{organization.Name}: ");
                foreach (var change in changes)
                {
                    var chainging = change.Before > change.After
                        ? change.Type == EventParticipantParameterType.Coffers
                            ? "Потрачено"
                            : "Потеряно"
                        : "Получено";
                    text.Add($"{EnumHelper<EventParticipantParameterType>.GetDisplayValue(change.Type)}: " +
                        $"Было - {ViewHelper.GetSweetNumber(change.Before)}, " +
                        $"{chainging} - {Math.Abs(change.Before - change.After)}, " +
                        $"Стало - {ViewHelper.GetSweetNumber(change.After)}.");
                }
            }
        }

        internal static int GetThresholdImportance(int oldValue, int newValue)
        {
            var threshold = 1;
            var thresholdImportance = 0;
            var max = Math.Max(oldValue, newValue);
            var min = Math.Min(oldValue, newValue);
            while (true)
            {
                threshold = threshold.ToString().StartsWith("1")
                    ? threshold * 3
                    : threshold / 3 * 10;
                if (min > threshold)
                    continue;
                if (max < threshold)
                    break;
                thresholdImportance = threshold;
            }
            return thresholdImportance;
        }
    }
}
