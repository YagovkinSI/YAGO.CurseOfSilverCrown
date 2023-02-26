using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Core.MainModels;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class GameSession
    {
        public int Id { get; set; }
        public int StartSeesionTurnId { get; set; }
        public int EndSeesionTurnId { get; set; }
        public int NumberOfGame { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<GameSession>();
            model.HasKey(m => m.Id);

            model.HasIndex(m => m.StartSeesionTurnId);
            model.HasIndex(m => m.EndSeesionTurnId);

            model.HasData(StartingData.GetFirstGameSession());
        }
    }
}
