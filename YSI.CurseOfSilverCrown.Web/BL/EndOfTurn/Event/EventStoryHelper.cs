using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Web.Enums;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;
using YSI.CurseOfSilverCrown.Web.Models.Enums;

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

            var organizations = eventStoryResult.Organizations
                .ToDictionary(
                    o => o.EventOrganizationType,
                    o => allOrganizations
                        .Where(c => c.Id == o.Id)
                        .ToList());

            switch (eventStoryResult.EventResultType)
            {
                case Enums.enEventResultType.Idleness:
                    text.Add($"{organizations[Enums.enEventOrganizationType.Main].First().Name}" +
                        $" оплачивает расходы двора.");
                    break;
                case Enums.enEventResultType.Growth:
                    text.Add($"{organizations[Enums.enEventOrganizationType.Main].First().Name} " +
                        $" производит набор воинов.");
                    break;
                case Enums.enEventResultType.FastWarSuccess:
                    text.Add($"{organizations[Enums.enEventOrganizationType.Agressor].First().Name}" +
                        $" внезапно вторгается в земли провинции " +
                        $"{organizations[Enums.enEventOrganizationType.Defender].First().Name}" +
                        $" и одерживает верх, принуждая побеждённых преклонить колени.");
                    break;
                case Enums.enEventResultType.FastWarFail:
                    text.Add($"{organizations[Enums.enEventOrganizationType.Agressor].First().Name}" +
                        $" внезапно вторгается в земли провинции " +
                        $"{organizations[Enums.enEventOrganizationType.Defender].First().Name}" +
                        $", но проигрывает и отсутпает.");
                    break;
                case Enums.enEventResultType.FastRebelionSuccess:
                    text.Add($"{organizations[Enums.enEventOrganizationType.Agressor].First().Name}" +
                        $" поднимает мятеж против сюзерена из провинции " +
                        $"{organizations[Enums.enEventOrganizationType.Defender].First().Name}" +
                        $" и одерживает верх, снимая с себя вассальную присягу.");
                    break;
                case Enums.enEventResultType.FastRebelionFail:
                    text.Add($"{organizations[Enums.enEventOrganizationType.Agressor].First().Name}" +
                        $" поднимает мятеж против сюзерена из провинции " +
                        $"{organizations[Enums.enEventOrganizationType.Defender].First().Name}" +
                        $", но проигрывает и отступает. Главы мятежников казнены.");
                    break;
                case Enums.enEventResultType.Investments:
                    text.Add($"В провинции {organizations[Enums.enEventOrganizationType.Main].First().Name}" +
                        $" происходит рост экономики.");
                    break;
                case Enums.enEventResultType.VasalTax:
                    text.Add($"{organizations[Enums.enEventOrganizationType.Vasal].First().Name}" +
                        $" платит налог сюзерену из провинции " +
                        $"{organizations[Enums.enEventOrganizationType.Suzerain].First().Name}.");
                    break;
                case Enums.enEventResultType.TaxCollection:
                    text.Add($"{organizations[Enums.enEventOrganizationType.Main].First().Name}" +
                        $" собирает налоги в своих землях.");
                    break;
                case Enums.enEventResultType.Maintenance:
                    text.Add($"{organizations[Enums.enEventOrganizationType.Main].First().Name}" +
                        $" оплачивает расходы на содержание воинов.");
                    break;
                case Enums.enEventResultType.Mutiny:
                    text.Add($"В провинции {organizations[Enums.enEventOrganizationType.Main].First().Name}" +
                        $" происходит мятеж. К власти приходят новые силы.");
                    break;
                case Enums.enEventResultType.Corruption:
                    text.Add($"В провинции {organizations[Enums.enEventOrganizationType.Main].First().Name}" +
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
