using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YAGO.World.Host.Database.Events;

namespace YAGO.World.Host.Helpers.Events
{
    internal class EventTextHelper
    {
        private static readonly Dictionary<EventType, Func<EventJsonDomainNameHelper, string>> _eventTextMethods = new()
        {
            { EventType.Idleness, (card) => $"{card.Main} оплачивает расходы двора." },
            { EventType.Growth, (card) => $"{card.Main} производит набор воинов." },
            { EventType.GrowthLevelI, (card) => $"Во владении {card.Main} гарнизон пополнен до 100 человек." },
            { EventType.GrowthLevelII, (card) => $"Во владении {card.Main} собран отряд в 300 воинов." },
            { EventType.GrowthLevelIII, (card) => $"Во владении {card.Main} армия превысила тысячу человек." },
            { EventType.GrowthLevelIV, (card) => $"Во владении {card.Main} армия достигла 3.000 воинов." },
            { EventType.GrowthLevelV, (card) => $"Во владении {card.Main} армия достигла 10.000 воинов." },
            { EventType.FastWarSuccess, FastWarSuccessMainText },
            { EventType.FastWarFail, FastWarFailMainText },
            { EventType.SiegeFail, SiegeFailMainText },
            { EventType.FastRebelionSuccess, FastRebelionSuccessMainText },
            { EventType.FastRebelionFail, FastRebelionFailMainText },
            { EventType.DestroyedUnit, DestroyedUnitMainText },
            { EventType.Investments, (card) => $"Во владении {card.Main} происходит рост экономики." },
            { EventType.InvestmentsLevelI, (card) => $"Во владении {card.Main} замечен значительный рост количества деревень." },
            { EventType.InvestmentsLevelII, (card) => $"Во владении {card.Main} налажены торговые пути с соседями." },
            { EventType.InvestmentsLevelIII, (card) => $"Во владении {card.Main} построен достаточно большой город." },
            { EventType.InvestmentsLevelIV, (card) => $"Во владении {card.Main} город увеличился до уровня мегаполиса." },
            { EventType.InvestmentsLevelV, (card) => $"Во владении {card.Main} мегаполис достиг огромных размеров." },
            { EventType.VasalTax, (card) => $"{card.Vasal} платит налог сюзерену из владения {card.Suzerain.First()}." },
            { EventType.TaxCollection, (card) => $"{card.Main} собирает налоги в своих землях." },
            { EventType.Maintenance, (card) => $"{card.Main} оплачивает расходы на содержание воинов." },
            { EventType.Mutiny, (card) => $"Во владении {card.Main} происходит мятеж. К власти приходят новые силы." },
            { EventType.Corruption, (card) => $"Во владении {card.Main} процветает коррупция." },
            { EventType.Liberation, (card) => $"Лорд владения {card.Main} дарует независимость владению {card.Vasal}." },
            { EventType.ChangeSuzerain, ChangeSuzerain },
            { EventType.VoluntaryOath, VoluntaryOath },
            { EventType.Fortifications, Fortifications },
            { EventType.FortificationsLevelI, FortificationsLevelI },
            { EventType.FortificationsLevelII, FortificationsLevelII },
            { EventType.FortificationsLevelIII, FortificationsLevelIII },
            { EventType.FortificationsLevelIV, FortificationsLevelIV },
            { EventType.FortificationsLevelV, FortificationsLevelV },
            { EventType.FortificationsMaintenance, FortificationsMaintenance },
            { EventType.GoldTransfer, GoldTransfer },
            { EventType.UnitMove, UnitMove },
            { EventType.UnitCantMove, UnitCantMove },
            { EventType.TownFire, TownFire },
            { EventType.CastleFire, (card) => $"В замке правителя владения {card.Main} произошёл крупный пожар." },
            { EventType.Disease, (card) => $"Вспышка смертельной болезни произошла во владении {card.Main}." }
        };

        internal static string GetEventText(EventType eventResultType, EventJsonDomainNameHelper card)
        {
            var method = _eventTextMethods[eventResultType];
            return method(card);
        }

        private static string Fortifications(EventJsonDomainNameHelper card)
        {
            return $"Во владении {card.Main} идёт постройка защитных укреплений.";
        }

        private static string FortificationsLevelI(EventJsonDomainNameHelper card)
        {
            return $"Во владении {card.Main} завершено строитесльство небольшого деревянного замка с частоколом.";
        }

        private static string FortificationsLevelII(EventJsonDomainNameHelper card)
        {
            return $"Во владении {card.Main} завершено строитесльство каменного донжона.";
        }

        private static string FortificationsLevelIII(EventJsonDomainNameHelper card)
        {
            return $"Во владении {card.Main} завершено строитесльство каменных стен замка.";
        }

        private static string FortificationsLevelIV(EventJsonDomainNameHelper card)
        {
            return $"Во владении {card.Main} завершено строительство второго ряда стен и рва.";
        }

        private static string FortificationsLevelV(EventJsonDomainNameHelper card)
        {
            return $"Во владении {card.Main} завершено строительство неприступного замка.";
        }

        private static string FortificationsMaintenance(EventJsonDomainNameHelper card)
        {
            return $"{card.Main} выделяет средства на поддержание защитных укреплений.";
        }

        private static string GoldTransfer(EventJsonDomainNameHelper card)
        {
            return $"{card.Main} отправляет золото во владение {card.Target}.";
        }

        private static string TownFire(EventJsonDomainNameHelper card)
        {
            return $"В главном городе владения {card.Main} поризошёл крупный пожар.";
        }

        private static string ChangeSuzerain(EventJsonDomainNameHelper card)
        {
            return $"Лорд владения {card.Main} передаёт вассальное владение " +
                    $"{card.Vasal} в подчинение владению {card.Suzerain.First()}";
        }

        private static string VoluntaryOath(EventJsonDomainNameHelper card)
        {
            return $"Лорд владения {card.Main} добровольно присягает на верность лорду владения " +
                    $"{card.Suzerain.First()}.";
        }

        private static string UnitMove(EventJsonDomainNameHelper card)
        {
            return $"Отряд владения {card.Main} перемещается из владения {card.Vasal}" +
                    $" во владение {card.Target}.";
        }

        private static string UnitCantMove(EventJsonDomainNameHelper card)
        {
            return $"Отряд владения {card.Main} не был пропущен к владению {card.Target} " +
                    $"и остался во владении {card.Vasal}.";
        }

        private static string DestroyedUnitMainText(EventJsonDomainNameHelper card)
        {
            var maintText = new List<string>
            {
                $"Отряд владения {card.Main} не смог отступить в дружественные земли " +
                $"и был полностью уничтожен во владении {card.Target}."
            };
            maintText.AddRange(GetSupports(card));
            return string.Join(" ", maintText);
        }

        private static string FastRebelionFailMainText(EventJsonDomainNameHelper card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} поднимает мятеж против сюзерена из владения " +
                $"{card.Defender}, но проигрывает и отступает. Главы мятежников казнены."
            };
            maintText.AddRange(GetSupports(card));
            return string.Join(" ", maintText);
        }

        private static string FastRebelionSuccessMainText(EventJsonDomainNameHelper card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} поднимает мятеж против сюзерена из владения " +
                $"{card.Defender} и объявляет о собственной независимости."
            };

            maintText.AddRange(GetSupports(card));
            return string.Join(" ", maintText);
        }

        private static string FastWarSuccessMainText(EventJsonDomainNameHelper card)
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

        private static string FastWarFailMainText(EventJsonDomainNameHelper card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} вторгается в земли владения {card.Defender}, но отступает после поражения в боях."
            };
            maintText.AddRange(GetSupports(card));
            return string.Join(" ", maintText);
        }

        private static string SiegeFailMainText(EventJsonDomainNameHelper card)
        {
            var maintText = new List<string>
            {
                $"{card.Agressor} вторгается в земли владения {card.Defender}, но отсутупает после безуспешной осады и шутрма."
            };
            maintText.AddRange(GetSupports(card));
            return string.Join(" ", maintText);
        }

        private static List<string> GetSupports(EventJsonDomainNameHelper card)
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

        private static string GetSupportText(EventJsonDomainNameHelper card, bool isAgressorSupport)
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
