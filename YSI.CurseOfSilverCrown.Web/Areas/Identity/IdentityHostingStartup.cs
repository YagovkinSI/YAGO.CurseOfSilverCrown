using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YSI.CurseOfSilverCrown.Core.Database.EF;

[assembly: HostingStartup(typeof(YSI.CurseOfSilverCrown.Web.Areas.Identity.IdentityHostingStartup))]
namespace YSI.CurseOfSilverCrown.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}