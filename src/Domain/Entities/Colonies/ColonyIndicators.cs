namespace YAGO.World.Domain.Entities.Colonies
{
    /// <summary>
    /// Индикаторы колонии
    /// </summary>
    public class ColonyIndicators
    {
        /// <summary>
        /// Эффект от праздника
        /// </summary>
        public double FestivalEffect { get; private set; }

        /// <summary>
        /// Текущая неделя
        /// </summary>
        public int CurrentWeek { get; private set; }

        /// <summary>
        /// была ли первая свадьба
        /// </summary>
        public bool FirstWedding { get; private set; }

        public ColonyIndicators(
            double festivalEffect, 
            int currentWeek, 
            bool firstWedding)
        {
            FestivalEffect = festivalEffect;
            CurrentWeek = currentWeek;
            FirstWedding = firstWedding;
        }

        public static ColonyIndicators CreateNew()
        {
            return new ColonyIndicators(
                festivalEffect: 0,
                currentWeek: 0,
                firstWedding: false);
        }

        public double MoodTotalCacl()
        {
            var moodTotal = 52.0;
            moodTotal += FestivalEffect;
            return moodTotal;
        }

        internal void AddFestivalEffect(double festivalEffect)
        {
            FestivalEffect += festivalEffect;
        }

        internal void SetFirstWedding()
        {
            FirstWedding = true;
        }

        internal void AddCurrentWeek()
        {
            CurrentWeek++;
        }
    }
}
