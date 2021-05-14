using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Event;
using YSI.CurseOfSilverCrown.Core.Utils;

namespace YSI.CurseOfSilverCrown.Core.Helpers
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

        public static async Task<List<List<string>>> GetWorldHistory(ApplicationDbContext context)
        {
            var organizationEventStories = await context.OrganizationEventStories
                .Include(o => o.EventStory)
                .Include("EventStory.Turn")
                .OrderByDescending(o => o.Importance - 200 * o.TurnId)
                .Take(30)
                .OrderByDescending(o => o.EventStoryId)
                .OrderByDescending(o => o.TurnId)
                .ToListAsync();

            var eventStories = organizationEventStories
                .Select(o => o.EventStory)
                .Distinct()
                .OrderByDescending(o => o.Id)
                .OrderByDescending(o => o.TurnId)
                .ToList();

            return await GetTextStories(context, eventStories);
        }

        public static async Task<List<List<string>>> GetWorldHistoryLastRound(ApplicationDbContext context)
        {
            var currentTurn = context.Turns
                .Single(t => t.IsActive);

            var organizationEventStories = await context.OrganizationEventStories
                .Include(o => o.EventStory)
                .Include("EventStory.Turn")
                .Where(e => e.TurnId == currentTurn.Id - 1 && e.Importance >= 5000)
                .OrderByDescending(o => o.EventStoryId)
                .OrderByDescending(o => o.TurnId)
                .ToListAsync();

            var eventStories = organizationEventStories
                .Select(o => o.EventStory)
                .Distinct()
                .OrderByDescending(o => o.Id)
                .OrderByDescending(o => o.TurnId)
                .ToList();

            return await GetTextStories(context, eventStories);
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
                        $" и одерживает верх. Плененный лорд провинции " +
                        $"{organizations[enEventOrganizationType.Defender].First().Name}" +
                        $" вынужден дать клятву верности, чтобы сохранить жизнь себе и своей семье.");
                    text.AddRange(GetSupports(organizations));
                    break;
                case enEventResultType.FastWarFail:
                    text.Add($"{organizations[enEventOrganizationType.Agressor].First().Name}" +
                        $" внезапно вторгается в земли провинции " +
                        $"{organizations[enEventOrganizationType.Defender].First().Name}" +
                        $", но проигрывает и отступает.");
                    text.AddRange(GetSupports(organizations));
                    break;
                case enEventResultType.FastRebelionSuccess:
                    text.Add($"{organizations[enEventOrganizationType.Agressor].First().Name}" +
                        $" поднимает мятеж против сюзерена из провинции " +
                        $"{organizations[enEventOrganizationType.Defender].First().Name}" +
                        $" и одерживает верх, снимая с себя вассальную присягу.");
                    text.AddRange(GetSupports(organizations));
                    break;
                case enEventResultType.FastRebelionFail:
                    text.Add($"{organizations[enEventOrganizationType.Agressor].First().Name}" +
                        $" поднимает мятеж против сюзерена из провинции " +
                        $"{organizations[enEventOrganizationType.Defender].First().Name}" +
                        $", но проигрывает и отступает. Главы мятежников казнены.");
                    text.AddRange(GetSupports(organizations));
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
                case enEventResultType.Liberation:
                    text.Add($"Лорд провинции {organizations[enEventOrganizationType.Main].First().Name}" +
                        $" дарует независимость провинции {organizations[enEventOrganizationType.Vasal].First().Name}.");
                    break;
                case enEventResultType.ChangeSuzerain:
                    text.Add($"Лорд провинции {organizations[enEventOrganizationType.Main].First().Name}" +
                        $" передаёт вассальную провинцию {organizations[enEventOrganizationType.Vasal].First().Name}" +
                        $" в подчинение провинции {organizations[enEventOrganizationType.Suzerain].First().Name}");
                    break;
                case enEventResultType.VoluntaryOath:
                    text.Add($"Лорд провинции {organizations[enEventOrganizationType.Main].First().Name}" +
                        $" добровольно присягает на верность лорду провинции {organizations[enEventOrganizationType.Suzerain].First().Name}.");
                    break;
                case enEventResultType.Fortifications:
                    text.Add($"В провинции {organizations[enEventOrganizationType.Main].First().Name}" +
                        $" идёт постройка защитных укреплений.");
                    break;
                case enEventResultType.FortificationsMaintenance:
                    text.Add($"{organizations[enEventOrganizationType.Main].First().Name}" +
                        $" выделяет средства на поддержание защитных укреплений.");
                    break;
                case enEventResultType.GoldTransfer:
                    text.Add($"{organizations[enEventOrganizationType.Main].First().Name}" +
                        $" отправляет золото в провинцию {organizations[enEventOrganizationType.Target].First().Name}.");
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

            return text;
        }

        private static List<string> GetSupports(Dictionary<enEventOrganizationType, List<Organization>> organizations)
        {
            var text = new List<string>();

            if (organizations.ContainsKey(enEventOrganizationType.SupporetForAgressor))
            {
                var attackText = new StringBuilder();
                attackText.Append($"Нападающему также оказывали поддержку силы " +
                    $"{(organizations[enEventOrganizationType.SupporetForAgressor].Count > 1 ? "провинций" : "провинции")} ");
                var names = organizations[enEventOrganizationType.SupporetForAgressor].Select(o => o.Name);
                attackText.Append($"{String.Join(", ", names)}.\r\n");
                text.Add(attackText.ToString());
            }

            if (organizations.ContainsKey(enEventOrganizationType.SupporetForDefender))
            {
                var defenseText = new StringBuilder();
                defenseText.Append($"Защищавшемуся также оказывали поддержку силы " +
                    $"{(organizations[enEventOrganizationType.SupporetForDefender].Count > 1 ? "провинций" : "провинции")} ");
                var names = organizations[enEventOrganizationType.SupporetForDefender].Select(o => o.Name);
                defenseText.Append($"{String.Join(", ", names)}.\r\n");
                text.Add(defenseText.ToString());
            }

            return text;
        }
    }
}
