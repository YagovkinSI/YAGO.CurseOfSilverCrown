using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Domain.Entities.Episodes
{
    public abstract class Dilemma
    {
        public abstract DilemmaType DilemmaType { get; }
        public IReadOnlyList<Choice> Choices { get; }
        public string[] ChoiceLabel { get; }

        protected Dilemma(
            IReadOnlyList<Choice> choice,
            string[]? choiceLabel = null)
        {
            if (!choice.Any())
                throw new YagoException("Ошибка формирования эпизода. Дилемма не содержит данных.");

            Choices = choice;
            ChoiceLabel = choiceLabel ?? ["Сделай выбор"];
        }

        public Choice GetChoice(Guid choiceId)
        {
            return Choices.Single(x => x.Id == choiceId);
        }
    }
}
