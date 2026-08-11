using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Database;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Infrastructure.Database.Colonies;
using YAGO.World.Infrastructure.Database.Cycles;

namespace YAGO.World.Infrastructure.Database
{
    internal class DatabaseMigrator : IDatabaseMigrator
    {
        private readonly ApplicationDbContext _databaseContext;
        private readonly ILogger<DatabaseMigrator> _logger;

        public DatabaseMigrator(
            ApplicationDbContext databaseContext,
            ILogger<DatabaseMigrator> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task Initialize(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Инициализация базы данных...");

            try
            {
                await _databaseContext.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Этап миграции пройден успешно.");

                await InitilaizeData(cancellationToken);
                _logger.LogInformation("Этап обновления данных БД пройден успешно.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при инициализации базы данных.");
                throw;
            }
        }

        private async Task InitilaizeData(CancellationToken cancellationToken)
        {
            var someChanges = false;

            someChanges |= Wipe("2026-09-01");

            if (someChanges)
                await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        private bool Wipe(string wipeDateString)
        {
            var someChanges = false;
            var wipeDate = DateTime.Parse(wipeDateString).ToUniversalTime();
            if (DateTime.Now < wipeDate)
            {
                _databaseContext.Colonies.ExecuteDelete();
                someChanges = true;
            }

            if (_databaseContext.Users
                .Any(x => x.IsTemporary))
            {
                var temporaryUsers = _databaseContext.Users
                    .Where(x => x.IsTemporary);
                _databaseContext.Users.RemoveRange(temporaryUsers);
                someChanges = true;
            }

            someChanges |= RestoreColonyAndCycles();

            return someChanges;
        }

        private bool RestoreColonyAndCycles()
        {
            var someChanges = false;
            if (_databaseContext.Users
                .Include(x => x.Colonies)
                .Any(x => !x.Colonies!.Any()))
            {
                var usersWithoutColonies = _databaseContext.Users
                    .Include(x => x.Colonies)
                    .Where(x => !x.Colonies!.Any());
                someChanges = CreateColonyAndCycles(someChanges, usersWithoutColonies);
            }

            return someChanges;
        }

        private bool CreateColonyAndCycles(bool someChanges, IQueryable<Users.UserEntity> usersWithoutColonies)
        {
            foreach (var user in usersWithoutColonies)
            {
                var entities = Colony.CreateNew(user.Id);
                foreach (var entity in entities)
                {
                    switch (entity)
                    {
                        case Colony colony:
                            _databaseContext.Add(colony.ToEntity());
                            break;
                        case Cycle cycle:
                            _databaseContext.Add(cycle.ToEntity());
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                someChanges = true;
            }

            return someChanges;
        }
    }
}
