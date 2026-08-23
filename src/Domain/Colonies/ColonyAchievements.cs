using System;
using System.Collections.Generic;
using System.Linq;

namespace YAGO.World.Domain.Colonies
{
    public class ColonyAchievements
    {
        public IReadOnlySet<string> Values => _hashSet;
        private readonly HashSet<string> _hashSet;

        public ColonyAchievements(
            IEnumerable<string> hashSet)
        {
            _hashSet = hashSet.ToHashSet();
        }

        internal static ColonyAchievements CreateNew()
        {
            return new ColonyAchievements(
                hashSet: []);
        }

        public bool HasAchievement(string achievement) => _hashSet.Contains(achievement);

        public void SetAchievement(string achievement)
        {
            if (string.IsNullOrWhiteSpace(achievement))
                throw new ArgumentException("Достижение не может быть пустой строкой.");
            _hashSet.Add(achievement);
        }

        public void RemoveAchievement(string achievement) => _hashSet.Remove(achievement);
    }
}
