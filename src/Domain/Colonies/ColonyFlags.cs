using System.Collections.Generic;

namespace YAGO.World.Domain.Colonies
{
    /// <summary>
    /// Отметки колонии (флаги)
    /// </summary>
    public class ColonyFlags
    {
        /// <summary>
        /// была ли первая свадьба
        /// </summary>
        public bool FirstWedding { get; private set; }

        /// <summary>
        /// Пройденные эпизоды
        /// </summary>
        public Dictionary<long, string> Episodes { get; private set; }

        public ColonyFlags(
            bool firstWedding,
            Dictionary<long, string> episodes)
        {
            FirstWedding = firstWedding;
            Episodes = episodes;
        }

        public static ColonyFlags CreateNew()
        {
            return new ColonyFlags(
                firstWedding: false,
                episodes: []);
        }

        internal void SetFirstWedding()
        {
            FirstWedding = true;
        }
    }
}
