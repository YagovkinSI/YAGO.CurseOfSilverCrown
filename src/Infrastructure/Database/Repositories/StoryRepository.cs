using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Story;
using YAGO.World.Domain.Story.Extensions;
using YAGO.World.Infrastructure.Database.Models.StoryDatas;
using YAGO.World.Infrastructure.Database.Resources;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly ApplicationDbContext _context;

        public StoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StoryDataImmutable> GetCurrentStoryData(long userId, CancellationToken cancellationToken)
        {
            var storyData = await _context.StoryDatas.FirstOrDefaultAsync(s => s.UserId == userId);
            if (storyData == null)
                storyData = await CreateStoryData(userId, cancellationToken);

            return JsonConvert.DeserializeObject<StoryDataImmutable>(storyData.StoryDataJson);
        }

        public async Task<StoryNode> GetCurrentStoryNode(long userId, CancellationToken cancellationToken)
        {
            var currentStoryNodeWithResult = await GetCurrentStoryNodeWithResults(userId, cancellationToken);
            return currentStoryNodeWithResult.RemoveResults();
        }

        public async Task<StoryNodeWithResults> GetCurrentStoryNodeWithResults(long userId, CancellationToken cancellationToken)
        {
            var storyData = await GetCurrentStoryData(userId, cancellationToken);

            return StoryDatabase.Nodes[storyData.StoreNodeId];
        }

        public async Task<StoryNode> UpdateStoryNode(long userId, StoryDataImmutable storyData, CancellationToken cancellationToken)
        {
            var currentStoryData = await _context.StoryDatas.FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            currentStoryData.StoryDataJson = JsonConvert.SerializeObject(storyData);
            currentStoryData.LastUpdate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return await GetCurrentStoryNode(storyData.StoreNodeId, cancellationToken);
        }

        private async Task<StoryData> CreateStoryData(long userId, CancellationToken cancellationToken)
        {
            var storyData = new StoryData()
            {
                LastUpdate = DateTime.UtcNow,
                Name = DateTime.UtcNow.ToLongDateString(),
                UserId = userId,
                CurrentStoryNodeId = 0,
                StoryDataJson = JsonConvert.SerializeObject(new StoryDataImmutable(
                    storeNodeId: 0, 
                    events: new Dictionary<string, bool>()))
            };
            _context.Add(storyData);
            await _context.SaveChangesAsync(cancellationToken);
            return storyData;
        }
    }
}
