using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.EndOfTurn.Helpers
{
    internal class EventStoryCard
    {
        private readonly Dictionary<enEventOrganizationType, string> dict = new Dictionary<enEventOrganizationType, string>();
        private readonly Dictionary<enEventOrganizationType, List<string>> dictForMultyple =
            new Dictionary<enEventOrganizationType, List<string>>();
        private readonly enEventOrganizationType[] Multyple = new enEventOrganizationType[]
        {
            enEventOrganizationType.SupporetForAgressor,
            enEventOrganizationType.SupporetForDefender,
            enEventOrganizationType.Suzerain
        };
        public string Main => TryGetName(enEventOrganizationType.Main);
        public List<string> Suzerain => TryGetMultypleName(enEventOrganizationType.Suzerain);
        public string Vasal => TryGetName(enEventOrganizationType.Vasal);
        public string Target => TryGetName(enEventOrganizationType.Target);
        public string Agressor => TryGetName(enEventOrganizationType.Agressor);
        public string Defender => TryGetName(enEventOrganizationType.Defender);
        public List<string> SupporetForAgressor => TryGetMultypleName(enEventOrganizationType.SupporetForAgressor);
        public List<string> SupporetForDefender => TryGetMultypleName(enEventOrganizationType.SupporetForDefender);

        private string TryGetName(enEventOrganizationType type)
        {
            dict.TryGetValue(type, out var val);
            return val ?? "ОШИБКА";
        }

        private List<string> TryGetMultypleName(enEventOrganizationType type)
        {
            dictForMultyple.TryGetValue(type, out var val);
            return val;
        }

        internal void TryAddName(enEventOrganizationType type, string name)
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
