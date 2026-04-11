using System.Collections.Generic;

namespace YAGO.World.Domain.Entities.Episodes
{
    internal class DilemmaTextInput : Dilemma
    {
        public override DilemmaType DilemmaType => DilemmaType.TextInput;

        public DilemmaTextInput(
            IReadOnlyList<Choice> choice,
            string[]? choiceLabel = null)
            : base(choice, choiceLabel)
        {
        }
    }
}
