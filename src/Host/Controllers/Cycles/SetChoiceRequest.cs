using System;

namespace YAGO.World.Host.Controllers.Cycles
{
    public record SetChoiceRequest(
        string EventId,
        string DilemmaResolving);
}
