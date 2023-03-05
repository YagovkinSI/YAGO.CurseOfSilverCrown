using Microsoft.EntityFrameworkCore;
using YSI.CurseOfSilverCrown.Core.Helpers.StartingDatas;

namespace YSI.CurseOfSilverCrown.Core.Database.Sessions
{
    public class Session
    {
        public int Id { get; set; }
        public int StartSeesionTurnId { get; set; }
        public int EndSeesionTurnId { get; set; }
        public int NumberOfGame { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<Session>();
            model.HasKey(m => m.Id);

            model.HasIndex(m => m.StartSeesionTurnId);
            model.HasIndex(m => m.EndSeesionTurnId);

            model.HasData(StartingData.GetFirstGameSession());
        }
    }
}
