using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Users;
using YAGO.World.Host.Controllers.MyUsers;

namespace YAGO.World.Host.Middlewares
{
    public class UserActivityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UserActivityMiddleware> _logger;

        public UserActivityMiddleware(
            RequestDelegate next,
            IServiceProvider serviceProvider,
            ILogger<UserActivityMiddleware> logger)
        {
            _next = next;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);

            await UpdateUserLastActivity(context);
        }

        private async Task UpdateUserLastActivity(HttpContext context)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                if (context.User.IsAuthenticated())
                {
                    var userService = scope.ServiceProvider
                        .GetRequiredService<IUserService>();

                    var userId = context.User.GetUserId();
                    await userService.UpdateLastActivity(userId, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления значения времени полсденего действия пользователя.");
            }
        }
    }
}