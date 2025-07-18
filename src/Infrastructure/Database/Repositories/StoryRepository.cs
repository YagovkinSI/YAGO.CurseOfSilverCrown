using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
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
            var storyData = await _context.StoryDatas.FirstOrDefaultAsync(s => s.UserId == userId);
            storyData ??= await CreateStoryData(userId, cancellationToken);

            return storyData.ToDomain();
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

        public async Task<StoryNode> UpdateStory(long userId, StoryData storyData, CancellationToken cancellationToken)
        {
            var currentStoryData = await _context.StoryDatas.FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            currentStoryData.CurrentStoryNodeId = storyData.StoreNodeId;
            currentStoryData.StoryDataJson = JsonConvert.SerializeObject(storyData.Data);
            currentStoryData.LastUpdate = DateTime.UtcNow;
            currentStoryData.Name = DateTime.UtcNow.ToLongDateString();

            await _context.SaveChangesAsync(cancellationToken);

            return await GetCurrentStoryNode(userId, cancellationToken);
        }

        public async Task DropStory(long userId, CancellationToken cancellationToken)
        {
            var currentStoryData = await _context.StoryDatas.FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            currentStoryData.CurrentStoryNodeId = 0;
            currentStoryData.StoryDataJson = JsonConvert.SerializeObject(StoryDataImmutable.Empty);
            currentStoryData.LastUpdate = DateTime.UtcNow;
            currentStoryData.Name = DateTime.UtcNow.ToLongDateString();

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<Models.StoryDatas.StoryData> CreateStoryData(long userId, CancellationToken cancellationToken)
        {
            Models.StoryDatas.StoryData storyData = new()
            {
                LastUpdate = DateTime.UtcNow,
                Name = DateTime.UtcNow.ToLongDateString(),
                UserId = userId,
                CurrentStoryNodeId = 0,
                StoryDataJson = JsonConvert.SerializeObject(StoryDataImmutable.Empty)
            };
            _context.Add(storyData);
            await _context.SaveChangesAsync(cancellationToken);
            return storyData;
        }
    }
}
