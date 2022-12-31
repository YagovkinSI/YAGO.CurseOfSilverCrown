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
            var mainText = eventStoryResult.EventResultType switch
            {
                enEventResultType.Idleness => new List<string> { $"{card.Main} оплачивает расходы двора." },
                enEventResultType.Growth => new List<string> { $"{card.Main} производит набор воинов." },
                enEventResultType.FastWarSuccess => FastWarSuccessMainText(card),
                enEventResultType.FastWarFail => FastWarFailMainText(card),
                enEventResultType.FastRebelionSuccess => FastRebelionSuccessMainText(card),
                enEventResultType.FastRebelionFail => FastRebelionFailMainText(card),
                enEventResultType.DestroyedUnit => DestroyedUnitMainText(card),
                enEventResultType.Investments =>
                    new List<string> { $"Во владении {card.Main} происходит рост экономики." },
                enEventResultType.VasalTax =>
                    new List<string> { $"{card.Vasal} платит налог сюзерену из владения {card.Suzerain}." },
                enEventResultType.TaxCollection =>
                    new List<string> { $"{card.Main} собирает налоги в своих землях." },
                enEventResultType.Maintenance =>
                    new List<string> { $"{card.Main} оплачивает расходы на содержание воинов." },
                enEventResultType.Mutiny =>
                    new List<string> { $"Во владении {card.Main} происходит мятеж. К власти приходят новые силы." },
                enEventResultType.Corruption => new List<string> { $"Во владении {card.Main} процветает коррупция." },
                enEventResultType.Liberation =>
                    new List<string> { $"Лорд владения {card.Main} дарует независимость владению {card.Vasal}." },
                enEventResultType.ChangeSuzerain => new List<string>
                {
                    $"Лорд владения {card.Main} передаёт вассальное владение " +
                        $"{card.Vasal} в подчинение владению {card.Suzerain}"
                },
                enEventResultType.VoluntaryOath => new List<string>
                {
                    $"Лорд владения {card.Main} добровольно присягает на верность лорду владения " +
                        $"{card.Suzerain}."
                },
                enEventResultType.Fortifications =>
                    new List<string> { $"Во владении {card.Main} идёт постройка защитных укреплений." },
                enEventResultType.FortificationsMaintenance =>
                    new List<string> { $"{card.Main} выделяет средства на поддержание защитных укреплений." },
                enEventResultType.GoldTransfer =>
                    new List<string> { $"{card.Main} отправляет золото во владение {card.Target}." },
                enEventResultType.UnitMove => new List<string>
                {
                    $"Отряд владения {card.Main} перемещается из владения {card.Vasal}" +
                        $" во владение {card.Target}."
                },
                enEventResultType.UnitCantMove => new List<string>
                {
                    $"Отряд владения {card.Main} не нашёл возможности пройти из владения " +
                        $"{card.Vasal} к владению {card.Target}."
                }
            };
            text.AddRange(mainText);
        }

        private static List<string> DestroyedUnitMainText(EventStoryCard card)
        {
            var maintText = new List<string>
            {
                $"Отряд владения {card.Main} не смог отступить в дружественные земли " +
                $"и был полностью уничтожен во владении {card.Target}."
            };
            maintText.AddRange(GetSupports(card));
            return maintText;
        }

        private static List<string> FastRebelionFailMainText(EventStoryCard card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} поднимает мятеж против сюзерена из владения " +
                $"{card.Defender}, но проигрывает и отступает. Главы мятежников казнены."
            };
            maintText.AddRange(GetSupports(card));
            return maintText;
        }

        private static List<string> FastRebelionSuccessMainText(EventStoryCard card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} поднимает мятеж против сюзерена из владения " +
                $"{card.Defender} и объявляет о собственной независимости."
            };

            maintText.AddRange(GetSupports(card));
            return maintText;
        }

        private static List<string> FastWarSuccessMainText(EventStoryCard card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} вторгается в земли владения {card.Defender}" +
                    $" и одерживает верх. Плененный лорд владения {card.Defender}" +
                    $" вынужден дать клятву верности, чтобы сохранить жизнь себе и своей семье."
            };
            maintText.AddRange(GetSupports(card));
            return maintText;
        }

        private static List<string> FastWarFailMainText(EventStoryCard card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} вторгается в земли владения {card.Defender}, но проигрывает и отступает."
            };
            maintText.AddRange(GetSupports(card));
            return maintText;
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

        private static List<string> GetSupports(EventStoryCard card)
        {
            var text = new List<string>();

            if (card.SupporetForAgressor != null)
            {
                var attackText = GetSupportText(card, true);
                text.Add(attackText);
            }

            if (card.SupporetForDefender != null)
            {
                var defenseText = GetSupportText(card, false);
                text.Add(defenseText);
            }

            return text;
        }

        private static string GetSupportText(EventStoryCard card, bool isAgressorSupport)
        {
            var preText = isAgressorSupport
                ? "Нападающему также оказывали поддержку силы "
                : "Защищавшемуся также оказывали поддержку силы ";
            var nameList = isAgressorSupport
                ? card.SupporetForAgressor
                : card.SupporetForDefender;

            var text = new StringBuilder();
            text.Append(preText + $"{(nameList.Count > 1 ? "владений" : "владения")} ");
            text.Append($"{string.Join(", ", nameList)}.\r\n");
            return text.ToString();
        }
    }
}
