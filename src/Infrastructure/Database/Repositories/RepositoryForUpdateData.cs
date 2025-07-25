using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;

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
            var userStoryNodes = _context.StoryDatas.ToList();

            var someChanges = false;
            foreach (var node in userStoryNodes)
            {
                if (node.CurrentStoryNodeId > 0 && node.CurrentStoryNodeId < 10)
                {
                    node.CurrentStoryNodeId = node.CurrentStoryNodeId switch
                    {
                        1 => 10,
                        2 => 20,
                        3 => 30,
                        4 => 40,
                        5 => 41,
                        6 => 42,
                        7 => 53
                    };
                    _context.Update(node);
                    someChanges |= true;
                }
            }
            
            if (someChanges)
                await _context.SaveChangesAsync();
        }
    }
}
