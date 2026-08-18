using System.Security.Claims;
using YAGO.World.Domain.Common.Exceptions;

namespace YAGO.World.Host.Controllers.Common.Extensions
{
    public static class UserClaimsPrincipalExtensions
    {
        public static bool IsAuthenticated(this ClaimsPrincipal userClaimsPrincipal)
        {
            return userClaimsPrincipal?.Identity?.IsAuthenticated ?? false;
        }

        public static long GetUserId(this ClaimsPrincipal userClaimsPrincipal)
        {
            if (userClaimsPrincipal?.Identity?.IsAuthenticated != true)
                throw new YagoNotAuthorizedException();

            var userIdClaim = userClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new YagoException("Не найдены данные пользователя 'Claim'");

            if (string.IsNullOrWhiteSpace(userIdClaim.Value))
                throw new YagoException("Данные пользователя 'Claim' пусты.");

            if (!long.TryParse(userIdClaim.Value, out var userId))
                throw new YagoException($"Ошибка определения идентификатора пользователья из значения '{userIdClaim.Value}'");

            return userId;
        }
    }
}
