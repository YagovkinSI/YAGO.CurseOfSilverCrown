using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSI.CurseOfSilverCrown.Core.MainModels.Events
{
    internal class EventTextHelper
    {
        private static readonly Dictionary<enEventType, Func<EventJsonDomainNameHelper, string>> _eventTextMethods = new()
        {
            { enEventType.Idleness, (card) => $"{card.Main} оплачивает расходы двора." },
            { enEventType.Growth, (card) => $"{card.Main} производит набор воинов." },
            { enEventType.GrowthLevelI, (card) => $"Во владении {card.Main} гарнизон пополнен до 100 человек." },
            { enEventType.GrowthLevelII, (card) => $"Во владении {card.Main} собран отряд в 300 воинов." },
            { enEventType.GrowthLevelIII, (card) => $"Во владении {card.Main} армия превысила тысячу человек." },
            { enEventType.GrowthLevelIV, (card) => $"Во владении {card.Main} армия достигла 3.000 воинов." },
            { enEventType.GrowthLevelV, (card) => $"Во владении {card.Main} армия достигла 10.000 воинов." },
            { enEventType.FastWarSuccess, FastWarSuccessMainText },
            { enEventType.FastWarFail, FastWarFailMainText },
            { enEventType.SiegeFail, SiegeFailMainText },
            { enEventType.FastRebelionSuccess, FastRebelionSuccessMainText },
            { enEventType.FastRebelionFail, FastRebelionFailMainText },
            { enEventType.DestroyedUnit, DestroyedUnitMainText },
            { enEventType.Investments, (card) => $"Во владении {card.Main} происходит рост экономики." },
            { enEventType.InvestmentsLevelI, (card) => $"Во владении {card.Main} замечен значительный рост количества деревень." },
            { enEventType.InvestmentsLevelII, (card) => $"Во владении {card.Main} налажены торговые пути с соседями." },
            { enEventType.InvestmentsLevelIII, (card) => $"Во владении {card.Main} построен достаточно большой город." },
            { enEventType.InvestmentsLevelIV, (card) => $"Во владении {card.Main} город увеличился до уровня мегаполиса." },
            { enEventType.InvestmentsLevelV, (card) => $"Во владении {card.Main} мегаполис достиг огромных размеров." },
            { enEventType.VasalTax, (card) => $"{card.Vasal} платит налог сюзерену из владения {card.Suzerain.First()}." },
            { enEventType.TaxCollection, (card) => $"{card.Main} собирает налоги в своих землях." },
            { enEventType.Maintenance, (card) => $"{card.Main} оплачивает расходы на содержание воинов." },
            { enEventType.Mutiny, (card) => $"Во владении {card.Main} происходит мятеж. К власти приходят новые силы." },
            { enEventType.Corruption, (card) => $"Во владении {card.Main} процветает коррупция." },
            { enEventType.Liberation, (card) => $"Лорд владения {card.Main} дарует независимость владению {card.Vasal}." },
            { enEventType.ChangeSuzerain, ChangeSuzerain },
            { enEventType.VoluntaryOath, VoluntaryOath },
            { enEventType.Fortifications, Fortifications },
            { enEventType.FortificationsLevelI, FortificationsLevelI },
            { enEventType.FortificationsLevelII, FortificationsLevelII },
            { enEventType.FortificationsLevelIII, FortificationsLevelIII },
            { enEventType.FortificationsLevelIV, FortificationsLevelIV },
            { enEventType.FortificationsLevelV, FortificationsLevelV },
            { enEventType.FortificationsMaintenance, FortificationsMaintenance },
            { enEventType.GoldTransfer, GoldTransfer },
            { enEventType.UnitMove, UnitMove },
            { enEventType.UnitCantMove, UnitCantMove },
            { enEventType.TownFire, TownFire },
            { enEventType.CastleFire, (card) => $"В замке правителя владения {card.Main} произошёл крупный пожар." },
            { enEventType.Disease, (card) => $"Вспышка смертельной болезни произошла во владении {card.Main}." }
        };

        internal static string GetEventText(enEventType eventResultType, EventJsonDomainNameHelper card)
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
