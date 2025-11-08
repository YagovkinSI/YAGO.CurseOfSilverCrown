using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Infrastructure.Database
{
    internal class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public UnitOfWorkRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task UpdateInTransactionAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken)
            where T : IEntity
        {
            using var transaction = await _databaseContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                foreach (var entity in entities)
                {
                    UpdateContextEnity(entity);
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

        private void UpdateContextEnity<T>(T entity) where T : IEntity
        {
            switch (entity)
            {
                case Colony colony:
                    var colonyEntity = _databaseContext.Colonies.Find(colony.Id)
                        ?? throw new YagoNotFoundException(nameof(Colony), colony.Id);
                    colonyEntity.Update(colony);
                    _databaseContext.Update(colonyEntity);
                    break;
                case Cycle cycle:
                    var cycleEntity = _databaseContext.Cycles.Find(cycle.Id)
                        ?? throw new YagoNotFoundException(nameof(Cycle), cycle.Id);
                    cycleEntity.Update(cycle);
                    _databaseContext.Update(cycleEntity);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
