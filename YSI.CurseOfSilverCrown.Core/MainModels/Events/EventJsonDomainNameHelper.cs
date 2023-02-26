using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.MainModels.EventDomains;

namespace YSI.CurseOfSilverCrown.Core.MainModels.Events
{
    internal class EventJsonDomainNameHelper
    {
        private readonly Dictionary<enEventDomainType, string> dict = new Dictionary<enEventDomainType, string>();
        private readonly Dictionary<enEventDomainType, List<string>> dictForMultyple =
            new Dictionary<enEventDomainType, List<string>>();
        private readonly enEventDomainType[] Multyple = new enEventDomainType[]
        {
            enEventDomainType.SupporetForAgressor,
            enEventDomainType.SupporetForDefender,
            enEventDomainType.Suzerain
        };
        public string Main => TryGetName(enEventDomainType.Main);
        public List<string> Suzerain => TryGetMultypleName(enEventDomainType.Suzerain);
        public string Vasal => TryGetName(enEventDomainType.Vasal);
        public string Target => TryGetName(enEventDomainType.Target);
        public string Agressor => TryGetName(enEventDomainType.Agressor);
        public string Defender => TryGetName(enEventDomainType.Defender);
        public List<string> SupporetForAgressor => TryGetMultypleName(enEventDomainType.SupporetForAgressor);
        public List<string> SupporetForDefender => TryGetMultypleName(enEventDomainType.SupporetForDefender);

        private string TryGetName(enEventDomainType type)
        {
            dict.TryGetValue(type, out var val);
            return val ?? "ОШИБКА";
        }

        private List<string> TryGetMultypleName(enEventDomainType type)
        {
            dictForMultyple.TryGetValue(type, out var val);
            return val;
        }

        internal void TryAddName(enEventDomainType type, string name)
        {
            if (Multyple.Any(t => t == type))
            {
                if (!dictForMultyple.ContainsKey(type))
                    dictForMultyple.Add(type, new List<string>());
                dictForMultyple[type].Add(name);
            }
            else
            {
                if (dict.ContainsKey(type))
                    throw new Exception($"Более одного домена с типом {type}");
                dict.Add(type, name);
            }

        }
    }
}
