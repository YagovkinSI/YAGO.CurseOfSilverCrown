using System;

namespace YAGO.World.Host.Controllers.Turns
{
    public record SetChoiceRequest(
        string EventId,
        string DilemmaResolving);
}
