using System.Collections.Generic;
using YSI.CurseOfSilverCrown.Web.Models;

namespace YSI.CurseOfSilverCrown.Web.PageModels
{
    public class HomePageModel
    {
        public HomePageModel(List<Card> cards, bool showNextTurnButton)
        {
            Cards = cards;
            ShowNextTurnButton = showNextTurnButton;
        }

        public bool ShowNextTurnButton { get; }

        public List<Card> Cards { get; }
    }
}
