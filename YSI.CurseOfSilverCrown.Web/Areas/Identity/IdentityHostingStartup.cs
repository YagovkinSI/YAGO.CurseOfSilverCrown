using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(YSI.CurseOfSilverCrown.Web.Areas.Identity.IdentityHostingStartup))]
namespace YSI.CurseOfSilverCrown.Web.Areas.Identity
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