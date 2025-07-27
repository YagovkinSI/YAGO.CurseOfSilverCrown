using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Story;
using YAGO.World.Infrastructure.Database.Models.StoryDatas.Extensions;

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
                var notValid = false;
                try
                {
                    var storyData = node.ToDomain();
                    if (storyData.Data.NodesResults == null)
                        notValid = true;

                    if (node.CurrentStoryNodeId > 0 && !storyData.Data.NodesResults.Any())
                        notValid = true;
                }
                catch
                {
                    notValid = true;
                }

                if (notValid)
                {
                    node.CurrentStoryNodeId = 0;
                    node.StoryDataJson = JsonConvert.SerializeObject(StoryDataImmutable.Empty);
                    node.LastUpdate = DateTime.UtcNow;
                    node.Name = DateTime.UtcNow.ToLongDateString();

                    _context.Update(node);
                    someChanges |= true;
                }
            }

            if (someChanges)
                await _context.SaveChangesAsync();
        }
    }
}
