using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Story;
using YAGO.World.Infrastructure.Database.Models.StoryDatas;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class RepositoryForUpdateData : IRepositoryForUpdateData
    {
        private readonly ApplicationDbContext _context;

        public RepositoryForUpdateData(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Update(CancellationToken cancellationToken)
        {

        }
    }
}
