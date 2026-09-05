using System;
using System.Collections.Generic;
using System.Linq;

namespace YAGO.World.Domain.Colonies
{
    public class UnlockedWikiArticles
    {
        public IReadOnlyDictionary<string, bool> Values => _values;
        private readonly Dictionary<string, bool> _values;

        public UnlockedWikiArticles(
            IEnumerable<KeyValuePair<string, bool>> values)
        {
            _values = values.ToDictionary(x => x.Key, x => x.Value);
        }

        internal static UnlockedWikiArticles CreateNew()
        {
            return new UnlockedWikiArticles(
                values: []);
        }

        public bool IsUnlocked(string code) => _values.ContainsKey(code);

        public bool IsRead(string code) => _values.TryGetValue(code, out var isRead) && isRead;

        public void MarkRead(string code)
        {
            AddUnlocked(code);
            _values[code] = true;
        }

        public void AddUnlocked(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Код статьи не может быть пустой строкой.");
            _values.TryAdd(code, false);
        }
    }
}
