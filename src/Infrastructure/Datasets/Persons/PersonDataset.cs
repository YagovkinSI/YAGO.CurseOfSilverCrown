using System.Collections.Generic;
using System.Linq;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Persons;
using YAGO.World.Infrastructure.Datasets.Common;

namespace YAGO.World.Infrastructure.Datasets.Persons
{
    internal static class PersonDataset
    {
        public static IReadOnlyList<Person> All =>
        [
            new Person(
                PersonConstants.Camilla,
                "Камилла Селезнёва",
                ImageSet.Camilla,
                WikiArticleConstants.GameplayCamilla),
        ];

        public static Person Get(string code)
        {
            var person = All.SingleOrDefault(x => x.Code == code)
                ?? throw new YagoNotFoundException(nameof(Person), code.ToString());
            return person;
        }
    }
}