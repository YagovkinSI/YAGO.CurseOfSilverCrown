using System.Collections.Generic;

namespace YAGO.World.Host.Models
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
