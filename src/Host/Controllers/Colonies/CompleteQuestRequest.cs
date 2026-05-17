using System;

namespace YAGO.World.Host.Controllers.Colonies
{
    public record CompleteQuestRequest(
        Guid Id,
        string DilemmaResolving);
}
