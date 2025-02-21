using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace YAGO.World.Infrastructure.Database
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private const string CONNECTION_STRING = "Host=localhost;Port=5432;Database=yagoworld;Username=user;Password=password";

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(CONNECTION_STRING);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}