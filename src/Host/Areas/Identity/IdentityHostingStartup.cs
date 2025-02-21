using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(YAGO.World.Host.Areas.Identity.IdentityHostingStartup))]
namespace YAGO.World.Host.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}