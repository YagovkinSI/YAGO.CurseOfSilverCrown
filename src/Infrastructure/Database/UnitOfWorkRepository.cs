using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Infrastructure.Database.Colonies;

namespace YAGO.World.Infrastructure.Database
{
    internal class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly ApplicationDbContext _databaseContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWorkRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            if (_transaction != null)
                throw new InvalidOperationException("Транзакция уже начата.");
            _transaction = await _databaseContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task<T> Add<T>(IEntity<T> domainEntity, CancellationToken cancellationToken)
        {
            CheckTransaction();

            var source = ToEntity<T>(domainEntity);
            _databaseContext.Add(source);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            return source.Id;
        }

        public async Task Update<T>(IEntity<T> domainEntity, CancellationToken cancellationToken)
        {
            CheckTransaction();

            var source = ToEntity<T>(domainEntity);
            var target = FindInDatabase(domainEntity)
                ?? throw new YagoNotFoundException(nameof(domainEntity), domainEntity.Id?.ToString() ?? "NULL");
            _databaseContext.Entry(target).CurrentValues.SetValues(source);
            _databaseContext.Update(target);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            await GetTransaction().CommitAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            await GetTransaction().RollbackAsync(cancellationToken);
        }

        private void CheckTransaction()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Транзакция не начата. Необходимо вызвать BeginTransactionAsync.");
        }

        private IDbContextTransaction GetTransaction()
        {
            return _transaction ?? throw new InvalidOperationException("Транзакция не начата. Необходимо вызвать BeginTransactionAsync.");
        }

        private static IEntity<T> ToEntity<T>(IEntity domainEntity)
        {
            return domainEntity switch
            {
                Colony colony => (IEntity<T>)colony.ToEntity(),
                _ => throw new NotImplementedException(),
            };
        }

        private IEntity? FindInDatabase(IEntity domainEntity)
        {
            return domainEntity switch
            {
                Colony colony => _databaseContext.Colonies.Find(colony.Id),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
