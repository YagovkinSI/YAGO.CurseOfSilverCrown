using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Turns;
using YAGO.World.Infrastructure.Database.Colonies;
using YAGO.World.Infrastructure.Database.Turns;

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
            var (source, target) = GetSourceAndTargetEntities(entity);
            if (target == null)
                _databaseContext.Add(source);
            else
            {
                _databaseContext.Entry(target).CurrentValues.SetValues(source);
                _databaseContext.Update(target);
            }
        }

        private (object source, object? target) GetSourceAndTargetEntities<T>(T entity) where T : IEntity
        {
            object source;
            object? target;
            switch (entity)
            {
                case Colony colony:
                    source = colony.ToEntity();
                    target = _databaseContext.Colonies.Find(colony.Id);
                    break;
                case Turn turn:
                    source = turn.ToEntity();
                    target = _databaseContext.Turns.Find(turn.Id);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return (source, target);
        }
    }
}
