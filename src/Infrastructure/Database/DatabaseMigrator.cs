using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Database;
using YAGO.World.Infrastructure.Database.Buildings;

namespace YAGO.World.Infrastructure.Database
{
    internal class DatabaseMigrator : IDatabaseInitializer
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

        public async Task InitilaizeData(CancellationToken cancellationToken)
        {
            var someChanges = false;

            if (_databaseContext.Buildings.Count() < 3)
            {
                var buildingEntities = new BuildingEntity[]
                {
                    new BuildingEntity(
                        1, "Семейный модуль", 1250, 25, 110, 200, 160,
                        [
                            "Небольшие, но обустроенные квартиры-студии для рабочих семей. Есть место для личных вещей и отдыха после смены. Такие условия помогают сохранить здоровье и лояльность колонистов."
                        ]),
                    new BuildingEntity(
                        2, "Стандартный модуль", 1250, 25, 120, 0, 200,
                        [
                            "Функциональные жилые капсулы с койко-местом, умывальником и небольшим складом для личных вещей. Всё необходимое для восстановления сил перед следующей рабочей сменой."
                        ]),
                    new BuildingEntity(
                        3, "Казарменный модуль", 1250, 25, 130, -200, 240,
                        [
                            "Спальные ниши, общие душевые и столовая. Личное пространство сведено к минимуму. Подходит для временных рабочих или тех, кому нечего терять."
                        ]),
                };

                _databaseContext.AddRange(buildingEntities);
                someChanges = true;
            }

            if (someChanges)
                await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
