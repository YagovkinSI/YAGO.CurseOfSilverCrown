using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Cycles
{
    public record AttackColonyRequest(
            [IdValidation] long TargetColonyId);
}
