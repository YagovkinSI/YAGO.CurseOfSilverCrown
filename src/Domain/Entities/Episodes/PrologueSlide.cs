using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class PrologueSlide : Slide
    {
        public string ContinueButtonName { get; }

        public PrologueSlide(
            string title,
            string imageName,
            string[] text,
            IReadOnlyList<KeyValueParameter> parameters,
            string continueButtonName)
            : base(title, imageName, text, parameters)
        {
            ContinueButtonName = continueButtonName;
        }
    }
}
