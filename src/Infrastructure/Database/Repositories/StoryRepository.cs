using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Dtos;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Fragments;
using YAGO.World.Domain.Slides;
using YAGO.World.Domain.Stories;
using YAGO.World.Domain.Story;
using YAGO.World.Infrastructure.Database.Models.StoryDatas;
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

        public async Task<Story> GetCurrentStoryData(long userId, CancellationToken cancellationToken)
        {
            var storyData = await _context.StoryDatas.FirstOrDefaultAsync(s => s.UserId == userId);
            storyData ??= await CreateStoryData(userId, cancellationToken);

            return storyData.ToDomain();
        }

        public async Task<Playthrough> GetCurrentChapter(long userId, CancellationToken cancellationToken)
        {
            var storyData = await GetCurrentStoryData(userId, cancellationToken);
            var slides = GetChapterSlides(storyData);

            var currentFragment = await GetCurrentFragment(userId, cancellationToken);
            var choices = currentFragment.NextFragmentIds
                .Select(f => StoryDatabase.Fragments[f])
                .Select(f => new StoryChoice(f.Id, f.ChoiceText))
                .ToArray();

            var currentSlideIndex = slides.Length - currentFragment.Slides.Length;

            return new Playthrough(
                storyData.Id,
                currentFragment.Id,
                chapterNumber: 1,
                title: "Обычное поручение",
                slides,
                currentSlideIndex,
                choices);
        }

        private static Slide[] GetChapterSlides(Story storyData)
        {
            var currentChapterFragmentIds = storyData.LastStoryChapter.FragmentIds;

            var slides = currentChapterFragmentIds
                .SelectMany(id => StoryDatabase.Fragments[id].Slides)
                .ToArray();

            return slides;
        }

        public async Task<Fragment> GetCurrentFragment(long userId, CancellationToken cancellationToken)
        {
            var storyData = await GetCurrentStoryData(userId, cancellationToken);
            var lastFragmentId = storyData.LastStoryChapter.FragmentIds[^1];
            return StoryDatabase.Fragments[lastFragmentId];
        }

        public async Task<Playthrough> UpdateStory(long userId, Story storyData, CancellationToken cancellationToken)
        {
            var currentStoryData = await _context.StoryDatas.FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            var data = new StoryDataImmutable(storyData.LastStoryChapter.FragmentIds.ToList());
            currentStoryData.StoryDataJson = JsonConvert.SerializeObject(data);

            currentStoryData.LastUpdate = DateTime.UtcNow;
            currentStoryData.Name = DateTime.UtcNow.ToLongDateString();

            await _context.SaveChangesAsync(cancellationToken);

            return await GetCurrentChapter(userId, cancellationToken);
        }

        public async Task DropStory(long userId, CancellationToken cancellationToken)
        {
            var currentStoryData = await _context.StoryDatas.FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            currentStoryData.CurrentStoryNodeId = 1;
            currentStoryData.StoryDataJson = JsonConvert.SerializeObject(StoryDataImmutable.New);
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
                CurrentStoryNodeId = 1,
                StoryDataJson = JsonConvert.SerializeObject(StoryDataImmutable.New)
            };
            _context.Add(storyData);
            await _context.SaveChangesAsync(cancellationToken);
            return storyData;
        }

        private Slide[] GetStoryFragmentSlides(Story storyData)
        {
            var slides = new List<Slide>();
            foreach (var fragmentId in storyData.LastStoryChapter.FragmentIds)
            {
                AddFragment(slides, fragmentId);
            }

            return slides.ToArray();
        }

        private static void AddFragment(List<Slide> slides, long nodeId)
        {
            var node = StoryDatabase.Fragments[nodeId];
            foreach (var slide in node.Slides)
            {
                var storyCard = new Slide(slides.Count, slide.Text, slide.ImageName);
                slides.Add(storyCard);
            }
        }
    }
}
