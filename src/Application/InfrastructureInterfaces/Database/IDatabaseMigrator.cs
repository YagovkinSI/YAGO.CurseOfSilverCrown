using System.Threading.Tasks;

namespace YAGO.World.Application.InfrastructureInterfaces.Database
{
    public interface IDatabaseMigrator
    {
        Task Migrate();
    }
}
