using System.Collections.Generic;

namespace YAGO.World.Host.Controllers.Episodes
{
    public record DilemmaResponse(
        IReadOnlyList<ChoiceResponse> Choice,
        string ChoiceType,
        string[] ChoiceLabel);
}
