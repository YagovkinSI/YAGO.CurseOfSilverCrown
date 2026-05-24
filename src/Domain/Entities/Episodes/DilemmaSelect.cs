using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class DilemmaSelect : Dilemma
    {
        public override DilemmaType DilemmaType => DilemmaType.Select;
        public IReadOnlyList<Choice> Choices { get; }
        public string[] ChoiceLabel { get; }

        public DilemmaSelect(
            IReadOnlyList<Choice> choice,
            string[]? choiceLabel = null)
            : base()
        {
            if (!choice.Any())
                throw new YagoException("Ошибка формирования эпизода. Дилемма не содержит данных.");

            Choices = choice;
            ChoiceLabel = choiceLabel ?? ["Сделай выбор"];
        }

        public Choice GetChoice(string choiceId)
        {
            return Choices.Single(x => x.Id == choiceId);
        }
    }
}
