using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record DilemmaResponse(
        string DilemmaType,
        IReadOnlyList<ChoiceResponse> Choice,
        string[] ChoiceLabel);
}
