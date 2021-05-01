using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn.Event
{
    public static class EventStoryHelper
    {
        public static async Task<List<List<string>>> GetTextStories(ApplicationDbContext context, List<EventStory> eventStories)
        {
            var textStories = new List<List<string>>();
            foreach (var eventStory in eventStories)
            {
                var turn = eventStory.Turn.Name;
                var textStory = await GetTextStoryAsync(context, eventStory);
                var pair = new List<string> { turn };
                pair.AddRange(textStory);
                textStories.Add(pair);
            }
            return textStories;
        }

        private static async Task<List<string>> GetTextStoryAsync(ApplicationDbContext context, EventStory eventStory)
        {
            var text = new List<string>();
            var eventStoryResult = JsonConvert.DeserializeObject<EventStoryResult>(eventStory.EventStoryJson);

            var ids = eventStoryResult.Organizations.Select(e => e.Id);
            var allOrganizations = await context.Organizations
                        .Where(c => ids.Contains(c.Id))
                        .ToListAsync();

            var organizations = new Dictionary<enEventOrganizationType, List<Organization>>();
            foreach (var organization in eventStoryResult.Organizations)
            {
                if (!organizations.ContainsKey(organization.EventOrganizationType))
                    organizations.Add(organization.EventOrganizationType,
                        allOrganizations.Where(o => o.Id == organization.Id).ToList());
                else organizations[organization.EventOrganizationType].Add(allOrganizations.Single(o => o.Id == organization.Id));
            }

            switch (eventStoryResult.EventResultType)
            {
                case enEventResultType.Idleness:
                    text.Add($"{organizations[enEventOrganizationType.Main].First().Name}" +
                        $" оплачивает расходы двора.");
                    break;
                case enEventResultType.Growth:
                    text.Add($"{organizations[enEventOrganizationType.Main].First().Name} " +
                        $" производит набор воинов.");
                    break;
                case enEventResultType.FastWarSuccess:
                    text.Add($"{organizations[enEventOrganizationType.Agressor].First().Name}" +
                        $" внезапно вторгается в земли провинции " +
                        $"{organizations[enEventOrganizationType.Defender].First().Name}" +
                        $" и одерживает верх, принуждая побеждённых преклонить колени.");
                    break;
                case enEventResultType.FastWarFail:
                    text.Add($"{organizations[enEventOrganizationType.Agressor].First().Name}" +
                        $" внезапно вторгается в земли провинции " +
                        $"{organizations[enEventOrganizationType.Defender].First().Name}" +
                        $", но проигрывает и отсутпает.");
                    break;
                case enEventResultType.FastRebelionSuccess:
                    text.Add($"{organizations[enEventOrganizationType.Agressor].First().Name}" +
                        $" поднимает мятеж против сюзерена из провинции " +
                        $"{organizations[enEventOrganizationType.Defender].First().Name}" +
                        $" и одерживает верх, снимая с себя вассальную присягу.");
                    break;
                case enEventResultType.FastRebelionFail:
                    text.Add($"{organizations[enEventOrganizationType.Agressor].First().Name}" +
                        $" поднимает мятеж против сюзерена из провинции " +
                        $"{organizations[enEventOrganizationType.Defender].First().Name}" +
                        $", но проигрывает и отступает. Главы мятежников казнены.");
                    break;
                case enEventResultType.Investments:
                    text.Add($"В провинции {organizations[enEventOrganizationType.Main].First().Name}" +
                        $" происходит рост экономики.");
                    break;
                case enEventResultType.VasalTax:
                    text.Add($"{organizations[enEventOrganizationType.Vasal].First().Name}" +
                        $" платит налог сюзерену из провинции " +
                        $"{organizations[enEventOrganizationType.Suzerain].First().Name}.");
                    break;
                case enEventResultType.TaxCollection:
                    text.Add($"{organizations[enEventOrganizationType.Main].First().Name}" +
                        $" собирает налоги в своих землях.");
                    break;
                case enEventResultType.Maintenance:
                    text.Add($"{organizations[enEventOrganizationType.Main].First().Name}" +
                        $" оплачивает расходы на содержание воинов.");
                    break;
                case enEventResultType.Mutiny:
                    text.Add($"В провинции {organizations[enEventOrganizationType.Main].First().Name}" +
                        $" происходит мятеж. К власти приходят новые силы.");
                    break;
                case enEventResultType.Corruption:
                    text.Add($"В провинции {organizations[enEventOrganizationType.Main].First().Name}" +
                        $" процветает коррупция.");
                    break;
            }

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
                        ? "Потеряно" 
                        : "Получено";
                    text.Add($"{EnumHelper<enEventParametrChange>.GetDisplayValue(change.Type)}: Было - {change.Before}, " +
                        $"{chainging} - {Math.Abs(change.Before - change.After)}, " +
                        $"Стало - {change.After}.");
                }    
            }

            return text;
        }
    }
}
