using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Story;
using YAGO.World.Domain.Story.Extensions;
using YAGO.World.Infrastructure.Database.Models.StoryDatas.Extensions;
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

        public async Task<Domain.Story.StoryData> GetCurrentStoryData(long userId, CancellationToken cancellationToken)
        {
            Models.StoryDatas.StoryData storyData = await _context.StoryDatas.FirstOrDefaultAsync(s => s.UserId == userId);
            storyData ??= await CreateStoryData(userId, cancellationToken);

            return storyData.ToDomain();
        }

        public async Task<StoryNode> GetCurrentStoryNode(long userId, CancellationToken cancellationToken)
        {
            StoryNodeWithResults currentStoryNodeWithResult = await GetCurrentStoryNodeWithResults(userId, cancellationToken);
            return currentStoryNodeWithResult.RemoveResults();
        }

        public async Task<StoryNodeWithResults> GetCurrentStoryNodeWithResults(long userId, CancellationToken cancellationToken)
        {
            StoryData storyData = await GetCurrentStoryData(userId, cancellationToken);

            return StoryDatabase.Nodes[storyData.StoreNodeId];
        }

        public async Task<StoryNode> UpdateStoryNode(long userId, StoryData storyData, CancellationToken cancellationToken)
        {
            Models.StoryDatas.StoryData currentStoryData = await _context.StoryDatas.FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            currentStoryData.CurrentStoryNodeId = storyData.StoreNodeId;
            currentStoryData.StoryDataJson = JsonConvert.SerializeObject(storyData.Data);
            currentStoryData.LastUpdate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return await GetCurrentStoryNode(userId, cancellationToken);
        }

        private async Task<Models.StoryDatas.StoryData> CreateStoryData(long userId, CancellationToken cancellationToken)
        {
            Models.StoryDatas.StoryData storyData = new()
            {
                LastUpdate = DateTime.UtcNow,
                Name = DateTime.UtcNow.ToLongDateString(),
                UserId = userId,
                CurrentStoryNodeId = 0,
                StoryDataJson = JsonConvert.SerializeObject(new StoryDataImmutable(
                    storeNodeId: 0,
                    events: new Dictionary<string, bool>(),
                    personalOpinions: new Dictionary<string, int>()))
            };
            _context.Add(storyData);
            await _context.SaveChangesAsync(cancellationToken);
            return storyData;
        }
    }
}
