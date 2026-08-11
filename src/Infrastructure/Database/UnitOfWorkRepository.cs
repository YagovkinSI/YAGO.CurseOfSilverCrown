using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Cycles;
using YAGO.World.Infrastructure.Database.Colonies;
using YAGO.World.Infrastructure.Database.Cycles;

namespace YAGO.World.Infrastructure.Database
{
    internal class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public UnitOfWorkRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task SaveInTransactionAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken)
            where T : IEntity
        {
            using var transaction = await _databaseContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                foreach (var entity in entities)
                {
                    SaveContextEnity(entity);
                }

                await _databaseContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private void SaveContextEnity<T>(T entity) where T : IEntity
        {
            switch (entity)
            {
                case Colony colony:
                    var colonySource = colony.ToEntity();
                    var colonyTarget = _databaseContext.Colonies.Find(colony.Id);
                    if (colonyTarget == null)
                        _databaseContext.Add(colonySource);
                    else
                        EntityUpdater.Update(colonySource, colonyTarget);
                    break;
                case Cycle cycle:
                    var cycleSource = cycle.ToEntity();
                    var cycleTarget = _databaseContext.Cycles.Find(cycle.Id);
                    if (cycleTarget == null)
                        _databaseContext.Add(cycleSource);
                    else
                        EntityUpdater.Update(cycleSource, cycleTarget);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
