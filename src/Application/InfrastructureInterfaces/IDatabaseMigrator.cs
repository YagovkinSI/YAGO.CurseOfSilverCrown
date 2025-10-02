using System.Threading.Tasks;

namespace YAGO.World.Application.InfrastructureInterfaces
{
    public interface IDatabaseMigrator
    {
        Task Migrate();
    }
}
