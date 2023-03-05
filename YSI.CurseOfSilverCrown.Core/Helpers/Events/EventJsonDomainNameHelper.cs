using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.Events;

namespace YSI.CurseOfSilverCrown.Core.Helpers.Events
{
    internal class EventJsonDomainNameHelper
    {
        private readonly Dictionary<EventParticipantType, string> dict = new Dictionary<EventParticipantType, string>();
        private readonly Dictionary<EventParticipantType, List<string>> dictForMultyple =
            new Dictionary<EventParticipantType, List<string>>();
        private readonly EventParticipantType[] Multyple = new EventParticipantType[]
        {
            EventParticipantType.SupporetForAgressor,
            EventParticipantType.SupporetForDefender,
            EventParticipantType.Suzerain
        };
        public string Main => TryGetName(EventParticipantType.Main);
        public List<string> Suzerain => TryGetMultypleName(EventParticipantType.Suzerain);
        public string Vasal => TryGetName(EventParticipantType.Vasal);
        public string Target => TryGetName(EventParticipantType.Target);
        public string Agressor => TryGetName(EventParticipantType.Agressor);
        public string Defender => TryGetName(EventParticipantType.Defender);
        public List<string> SupporetForAgressor => TryGetMultypleName(EventParticipantType.SupporetForAgressor);
        public List<string> SupporetForDefender => TryGetMultypleName(EventParticipantType.SupporetForDefender);

        private string TryGetName(EventParticipantType type)
        {
            dict.TryGetValue(type, out var val);
            return val ?? "ОШИБКА";
        }

        private List<string> TryGetMultypleName(EventParticipantType type)
        {
            dictForMultyple.TryGetValue(type, out var val);
            return val;
        }

        internal void TryAddName(EventParticipantType type, string name)
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
