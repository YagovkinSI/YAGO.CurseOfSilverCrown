using System;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.GameEvents;

namespace YAGO.World.Domain.Entities.Episodes
{
    public class Choice : Slide
    {
        public Guid Id { get; }

        public Choice(
            Guid id,
            string title,
            string imageName,
            string[] text,
            IReadOnlyList<KeyValueParameter> parameters)
            :base (title, imageName, text, parameters)
        {
            Id = id;
        }
    }
}
