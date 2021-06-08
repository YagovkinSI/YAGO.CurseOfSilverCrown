using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class GameSession
    {
        public int Id { get; set; }
        public int StartSeesionTurnId { get; set; }
        public int EndSeesionTurnId { get; set; }
        public int NumberOfGame { get; set; }
    }
}
