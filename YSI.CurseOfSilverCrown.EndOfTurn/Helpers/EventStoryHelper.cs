using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.Utils;
using YSI.CurseOfSilverCrown.EndOfTurn.Event;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Helpers
{
    public static class EventStoryHelper
    {
        public static async Task<List<List<string>>> GetTextStories(ApplicationDbContext context, List<EventStory> eventStories)
        {
            var textStories = new List<List<string>>();
            foreach (var eventStory in eventStories)
            {
                var turn = GameSessionHelper.GetName(context, eventStory.Turn);
                var textStory = await GetTextStoryAsync(context, eventStory);
                var pair = new List<string> { turn };
                pair.AddRange(textStory);
                textStories.Add(pair);
            }
            return textStories;
        }

        public static async Task<List<List<string>>> GetWorldHistory(ApplicationDbContext context)
        {
            var currentTurn = context.Turns
                .Single(t => t.IsActive);

            var organizationEventStories = await context.OrganizationEventStories
                .Where(o => o.TurnId != currentTurn.Id && o.Importance >= 5000)
                .OrderByDescending(o => o.Importance - 200 * o.TurnId)
                .Take(30)
                .OrderByDescending(o => o.EventStoryId)
                .OrderByDescending(o => o.TurnId)
                .ToListAsync();

            var eventStories = GetEventStories(organizationEventStories);

            return await GetTextStories(context, eventStories);
        }

        public static async Task<List<List<string>>> GetWorldHistoryLastRound(ApplicationDbContext context)
        {
            var currentTurn = context.Turns
                .Single(t => t.IsActive);

            var organizationEventStories = await context.OrganizationEventStories
                .Where(e => e.TurnId == currentTurn.Id - 1 && e.Importance >= 5000)
                .OrderByDescending(o => o.EventStoryId)
                .OrderByDescending(o => o.TurnId)
                .ToListAsync();

            var eventStories = GetEventStories(organizationEventStories);

            return await GetTextStories(context, eventStories);
        }

        private static List<EventStory> GetEventStories(List<DomainEventStory> domainEventStories)
        {
            return domainEventStories
                .Select(o => o.EventStory)
                .Distinct()
                .OrderByDescending(o => o.Id)
                .OrderByDescending(o => o.TurnId)
                .ToList();
        }

        private static async Task<List<string>> GetTextStoryAsync(ApplicationDbContext context, EventStory eventStory)
        {
            var text = new List<string>();
            var eventStoryResult = JsonConvert.DeserializeObject<EventStoryResult>(eventStory.EventStoryJson);

            var ids = eventStoryResult.Organizations.Select(e => e.Id);
            var allOrganizations = await context.Domains
                        .Where(c => ids.Contains(c.Id))
                        .ToListAsync();

            var eventStoryCard = GetEventStoryCard(eventStoryResult, allOrganizations);
            FillEventMainText(text, eventStoryResult, eventStoryCard);
            FillEventParameters(text, eventStoryResult, allOrganizations);
            return text;
        }

        private static EventStoryCard GetEventStoryCard(EventStoryResult eventStoryResult,
            List<Domain> allOrganizations)
        {
            var eventStoryCard = new EventStoryCard();
            foreach (var domain in eventStoryResult.Organizations)
            {
                var type = domain.EventOrganizationType;
                var name = allOrganizations.Single(d => d.Id == domain.Id).Name;
                eventStoryCard.TryAddName(type, name);
            }
            return eventStoryCard;
        }

        private static void FillEventMainText(List<string> text, EventStoryResult eventStoryResult,
            EventStoryCard card)
        {
            var mainText = EventStoryTextHelper.GetEventText(eventStoryResult.EventResultType, card);
            text.Add(mainText);
        }

        private static void FillEventParameters(List<string> text, EventStoryResult eventStoryResult,
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
                        ? change.Type == enActionParameter.Coffers
                            ? "Потрачено"
                            : "Потеряно"
                        : "Получено";
                    text.Add($"{EnumHelper<enActionParameter>.GetDisplayValue(change.Type)}: " +
                        $"Было - {ViewHelper.GetSweetNumber(change.Before)}, " +
                        $"{chainging} - {Math.Abs(change.Before - change.After)}, " +
                        $"Стало - {ViewHelper.GetSweetNumber(change.After)}.");
                }
            }
        }
    }
}
