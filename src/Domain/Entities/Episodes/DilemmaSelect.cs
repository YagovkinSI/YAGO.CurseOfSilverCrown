using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.Episodes
{
    internal class DilemmaSelect : Dilemma
    {
        public override DilemmaType DilemmaType => DilemmaType.Select;

        public DilemmaSelect(
            IReadOnlyList<Choice> choice,
            string[]? choiceLabel = null)
            : base(choice, choiceLabel)
        {
        }

    }
}
