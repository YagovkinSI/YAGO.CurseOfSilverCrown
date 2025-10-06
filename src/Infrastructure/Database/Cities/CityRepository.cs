using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cities;
using YAGO.World.Domain.Cities;
using YAGO.World.Infrastructure.Database.Users;

namespace YAGO.World.Infrastructure.Database.Cities
{
    internal class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        private readonly List<string> _cityNames;
        private readonly Random _random = new();

        public CityRepository(
            ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;

            _cityNames = LoadCityNamesFromFile();
        }

        public async Task<City?> Find(long id, CancellationToken cancellationToken)
        {
            var cityEntity = await _databaseContext.Cities
                .FindAsync([id], cancellationToken);
            return cityEntity?.ToDomain();
        }

        public async Task<City?> FindByUser(long userId, CancellationToken cancellationToken)
        {
            var cityEntity = await _databaseContext.Cities
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
            return cityEntity?.ToDomain();
        }

        public async Task<City> Create(long userId, string name, string description, CancellationToken cancellationToken)
        {
            var cityEntity = CityEntity.CreateNew(userId, name, description);

            _databaseContext.Add(cityEntity);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return cityEntity.ToDomain();
        }

        public Task<string[]> GetRandomCityNames(int count, CancellationToken cancellationToken)
        {
            if (count <= 0 || count > _cityNames.Count)
            {
                count = Math.Min(25, _cityNames.Count);
            }

            var result = new List<string>();
            var availableIndexes = Enumerable.Range(0, _cityNames.Count).ToList();
            for (int i = 0; i < count && availableIndexes.Count > 0; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var randomIndex = _random.Next(availableIndexes.Count);
                var nameIndex = availableIndexes[randomIndex];
                result.Add(_cityNames[nameIndex]);

                availableIndexes.RemoveAt(randomIndex);
            }

            return Task.FromResult(result.ToArray());
        }

        private List<string> LoadCityNamesFromFile()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            var filePath = Path.Combine(assemblyDirectory!, "Database", "Cities", "city-names.json");
            var json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<CityNamesData>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return data!.Names;
        }
    }
}