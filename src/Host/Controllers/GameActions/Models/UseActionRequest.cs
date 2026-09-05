using System.ComponentModel.DataAnnotations;

namespace YAGO.World.Host.Controllers.GameActions.Models
{
    public record UseActionRequest(
        [Required] GameActionType Type,
        string? Code,
        string? Value);
}