using System.Collections.Generic;
using YAGO.World.Host.Models;

namespace YAGO.World.Host.PageModels
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
