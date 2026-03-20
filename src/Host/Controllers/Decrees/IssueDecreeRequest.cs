using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Decrees
{
    public record IssueDecreeRequest(
        [IdValidation] long DecreeId);
}
