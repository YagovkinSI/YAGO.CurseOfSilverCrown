using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Dilemma
    {
        public IReadOnlyList<Choice> Choices { get; }
        public ChoiceType ChoiceType { get; }
        public string[] ChoiceLabel { get; }

        public Dilemma(
            IReadOnlyList<Choice> choice,
            ChoiceType choiceType = ChoiceType.Select,
            string[]? choiceLabel = null)
        {
            if (!choice.Any())
                throw new YagoException("Ошибка формирования эпизода. Дилемма не содержит данных.");

            Choices = choice;
            ChoiceType = choiceType;
            ChoiceLabel = choiceLabel ?? ["Сделай выбор"];
        }

        public Choice GetChoice(Guid choiceId)
        {
            return Choices.Single(x => x.Id == choiceId);
        }
    }
}
