using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Helpers
{
    internal class EventStoryTextHelper
    {
        private static readonly Dictionary<enEventResultType, Func<EventStoryCard, string>> _eventTextMethods = new()
        {
            { enEventResultType.Idleness, (card) => $"{card.Main} оплачивает расходы двора." },
            { enEventResultType.Growth, (card) => $"{card.Main} производит набор воинов." },
            { enEventResultType.GrowthLevelI, (card) => $"Во владении {card.Main} гарнизон пополнен до 100 человек." },
            { enEventResultType.GrowthLevelII, (card) => $"Во владении {card.Main} собран отряд в 300 воинов." },
            { enEventResultType.GrowthLevelIII, (card) => $"Во владении {card.Main} армия превысила тысячу человек." },
            { enEventResultType.GrowthLevelIV, (card) => $"Во владении {card.Main} армия достигла 3.000 воинов." },
            { enEventResultType.GrowthLevelV, (card) => $"Во владении {card.Main} армия достигла 10.000 воинов." },
            { enEventResultType.FastWarSuccess, FastWarSuccessMainText },
            { enEventResultType.FastWarFail, FastWarFailMainText },
            { enEventResultType.SiegeFail, SiegeFailMainText },
            { enEventResultType.FastRebelionSuccess, FastRebelionSuccessMainText },
            { enEventResultType.FastRebelionFail, FastRebelionFailMainText },
            { enEventResultType.DestroyedUnit, DestroyedUnitMainText },
            { enEventResultType.Investments, (card) => $"Во владении {card.Main} происходит рост экономики." },
            { enEventResultType.InvestmentsLevelI, (card) => $"Во владении {card.Main} замечен значительный рост количества деревень." },
            { enEventResultType.InvestmentsLevelII, (card) => $"Во владении {card.Main} налажены торговые пути с соседями." },
            { enEventResultType.InvestmentsLevelIII, (card) => $"Во владении {card.Main} построен достаточно большой город." },
            { enEventResultType.InvestmentsLevelIV, (card) => $"Во владении {card.Main} город увеличился до уровня мегаполиса." },
            { enEventResultType.InvestmentsLevelV, (card) => $"Во владении {card.Main} мегаполис достиг огромных размеров." },
            { enEventResultType.VasalTax, (card) => $"{card.Vasal} платит налог сюзерену из владения {card.Suzerain.First()}." },
            { enEventResultType.TaxCollection, (card) => $"{card.Main} собирает налоги в своих землях." },
            { enEventResultType.Maintenance, (card) => $"{card.Main} оплачивает расходы на содержание воинов." },
            { enEventResultType.Mutiny, (card) => $"Во владении {card.Main} происходит мятеж. К власти приходят новые силы." },
            { enEventResultType.Corruption, (card) => $"Во владении {card.Main} процветает коррупция." },
            { enEventResultType.Liberation, (card) => $"Лорд владения {card.Main} дарует независимость владению {card.Vasal}." },
            { enEventResultType.ChangeSuzerain, ChangeSuzerain },
            { enEventResultType.VoluntaryOath, VoluntaryOath },
            { enEventResultType.Fortifications, Fortifications },
            { enEventResultType.FortificationsLevelI, FortificationsLevelI },
            { enEventResultType.FortificationsLevelII, FortificationsLevelII },
            { enEventResultType.FortificationsLevelIII, FortificationsLevelIII },
            { enEventResultType.FortificationsLevelIV, FortificationsLevelIV },
            { enEventResultType.FortificationsLevelV, FortificationsLevelV },
            { enEventResultType.FortificationsMaintenance, FortificationsMaintenance },
            { enEventResultType.GoldTransfer, GoldTransfer },
            { enEventResultType.UnitMove, UnitMove },
            { enEventResultType.UnitCantMove, UnitCantMove },
            { enEventResultType.TownFire, TownFire },
            { enEventResultType.CastleFire, (card) => $"В замке правителя владения {card.Main} произошёл крупный пожар." },
            { enEventResultType.Disease, (card) => $"Вспышка смертельной болезни произошла во владении {card.Main}." }
        };

        internal static string GetEventText(enEventResultType eventResultType, EventStoryCard card)
        {
            var method = _eventTextMethods[eventResultType];
            return method(card);
        }

        private static string Fortifications(EventStoryCard card)
        {
            return $"Во владении {card.Main} идёт постройка защитных укреплений.";
        }

        private static string FortificationsLevelI(EventStoryCard card)
        {
            return $"Во владении {card.Main} завершено строитесльство небольшого деревянного замка с частоколом.";
        }

        private static string FortificationsLevelII(EventStoryCard card)
        {
            return $"Во владении {card.Main} завершено строитесльство каменного донжона.";
        }

        private static string FortificationsLevelIII(EventStoryCard card)
        {
            return $"Во владении {card.Main} завершено строитесльство каменных стен замка.";
        }

        private static string FortificationsLevelIV(EventStoryCard card)
        {
            return $"Во владении {card.Main} завершено строительство второго ряда стен и рва.";
        }

        private static string FortificationsLevelV(EventStoryCard card)
        {
            return $"Во владении {card.Main} завершено строительство неприступного замка.";
        }

        private static string FortificationsMaintenance(EventStoryCard card)
        {
            return $"{card.Main} выделяет средства на поддержание защитных укреплений.";
        }

        private static string GoldTransfer(EventStoryCard card)
        {
            return $"{card.Main} отправляет золото во владение {card.Target}.";
        }

        private static string TownFire(EventStoryCard card)
        {
            return $"В главном городе владения {card.Main} поризошёл крупный пожар.";
        }

        private static string ChangeSuzerain(EventStoryCard card)
        {
            return $"Лорд владения {card.Main} передаёт вассальное владение " +
                    $"{card.Vasal} в подчинение владению {card.Suzerain.First()}";
        }

        private static string VoluntaryOath(EventStoryCard card)
        {
            return $"Лорд владения {card.Main} добровольно присягает на верность лорду владения " +
                    $"{card.Suzerain.First()}.";
        }

        private static string UnitMove(EventStoryCard card)
        {
            return $"Отряд владения {card.Main} перемещается из владения {card.Vasal}" +
                    $" во владение {card.Target}.";
        }

        private static string UnitCantMove(EventStoryCard card)
        {
            return $"Отряд владения {card.Main} не был пропущен к владению {card.Target} " +
                    $"и остался во владении {card.Vasal}.";
        }

        private static string DestroyedUnitMainText(EventStoryCard card)
        {
            var maintText = new List<string>
            {
                $"Отряд владения {card.Main} не смог отступить в дружественные земли " +
                $"и был полностью уничтожен во владении {card.Target}."
            };
            maintText.AddRange(GetSupports(card));
            return string.Join(" ", maintText);
        }

        private static string FastRebelionFailMainText(EventStoryCard card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} поднимает мятеж против сюзерена из владения " +
                $"{card.Defender}, но проигрывает и отступает. Главы мятежников казнены."
            };
            maintText.AddRange(GetSupports(card));
            return string.Join(" ", maintText);
        }

        private static string FastRebelionSuccessMainText(EventStoryCard card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} поднимает мятеж против сюзерена из владения " +
                $"{card.Defender} и объявляет о собственной независимости."
            };

            maintText.AddRange(GetSupports(card));
            return string.Join(" ", maintText);
        }

        private static string FastWarSuccessMainText(EventStoryCard card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} вторгается в земли владения {card.Defender}" +
                    $" и одерживает верх. Плененный лорд владения {card.Defender}" +
                    $" вынужден дать клятву верности, чтобы сохранить жизнь себе и своей семье."
            };
            maintText.AddRange(GetSupports(card));
            return string.Join(" ", maintText);
        }

        private static string FastWarFailMainText(EventStoryCard card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} вторгается в земли владения {card.Defender}, но отступает после поражения в боях."
            };
            maintText.AddRange(GetSupports(card));
            return string.Join(" ", maintText);
        }

        private static string SiegeFailMainText(EventStoryCard card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} вторгается в земли владения {card.Defender}, но отсутупает после безуспешной осады и шутрма."
            };
            maintText.AddRange(GetSupports(card));
            return string.Join(" ", maintText);
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
            text.Append($"{string.Join(", ", nameList)}.");
            return text.ToString();
        }
    }
}
