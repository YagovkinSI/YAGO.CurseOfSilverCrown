using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using YAGO.World.Host.Models;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Errors;

namespace YAGO.World.Host.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ErrorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerFeature>();
                var error = new Error
                {
                    Message = exceptionHandler?.Error?.Message,
                    TypeFullName = exceptionHandler?.Error?.GetType()?.FullName,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    StackTrace = exceptionHandler?.Error?.StackTrace
                };
                _ = _context.Add(error);
                _ = _context.SaveChangesAsync();
            }
            catch
            {

            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
