using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record DilemmaSelectResponse(
        IReadOnlyList<ChoiceResponse> Choice,
        string[] ChoiceLabel)
        : DilemmaResponse(Domain.Entities.Episodes.DilemmaType.Select.ToString());
}
