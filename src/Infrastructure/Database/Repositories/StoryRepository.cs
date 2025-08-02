using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Common;
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

        public async Task<PaginatedResponse<StoryItem>> GetStoryList(long? userId, int page, CancellationToken cancellationToken)
        {
            const int StoryItemPerPage = 5;

            var storyList = await _context.StoryDatas
                .Include(s => s.User)
                .OrderBy(s => s.UserId == userId ? 0 : 1)
                .ThenBy(s => s.CurrentStoryNodeId)
                .ThenBy(s => s.Id)
                .Skip((page - 1) * StoryItemPerPage)
                .Take(StoryItemPerPage)
                .ToListAsync(cancellationToken);
            var data = storyList.Select(s => s.ToStoryItem()).ToArray();

            var total = _context.StoryDatas
                .Count();

            return new PaginatedResponse<StoryItem>(data, total, page, StoryItemPerPage);
        }

        public async Task<StoryFragment> GetStory(long gameSessionId, CancellationToken cancellationToken)
        {
            var storyDataDb = await _context.StoryDatas.FindAsync(new object[] { gameSessionId }, cancellationToken);
            var storyItem = storyDataDb.ToStoryItem();
            var storyData = storyDataDb.ToDomain();
            var cards = GetStoryFragmentSlides(storyData);
            return new StoryFragment(
                storyItem.User,
                storyItem.GameSession,
                storyItem.Title,
                storyItem.Chapter,
                cards
            );
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

        private Slide[] GetStoryFragmentSlides(StoryData storyData)
        {
            var slides = new List<Slide>();
            foreach (var pair in storyData.Data.NodesResults)
            {
                AddFragment(slides, pair.Key);
            }

            if (storyData.Data.NodesResults.Any())
            {
                var lastNodeId = storyData.Data.NodesResults.Last().Key;
                var lastNode = StoryDatabase.Nodes[lastNodeId];
                var lastChoice = lastNode.Choices.Single(c => c.Number == storyData.Data.NodesResults.Last().Value);
                AddFragment(slides, lastChoice.NextStoreNodeId);
            }
            else
            {
                AddFragment(slides, 0);
            }

            return slides.ToArray();
        }

        private static void AddFragment(List<Slide> slides, long nodeId)
        {
            var node = StoryDatabase.Nodes[nodeId];
            foreach (var slide in node.Slides)
            {
                var storyCard = new Slide(slides.Count, slide.Text, slide.ImageName);
                slides.Add(storyCard);
            }
        }
    }
}
