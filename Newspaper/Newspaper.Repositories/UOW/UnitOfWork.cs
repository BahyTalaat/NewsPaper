using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Repositories.UOW
{
    public class UnitOfWork<T>:IUnitOfWork<T> where T:DbContext
    {
        private readonly T _dbContext;
        private bool disposed = false;
        public IDbContextTransaction CurrentTransaction { get; private set; }

        public IsolationLevel CurrentTransactionIsolationLevel => CurrentTransaction != null ? CurrentTransaction.GetDbTransaction().IsolationLevel : IsolationLevel.Unspecified;

        public UnitOfWork(T dbContext)
        {
            _dbContext = dbContext;
        }

        public int Save()
        {
            var res = _dbContext.SaveChanges();
            DetachAllEntities();
            return res;
        }

        private void DetachAllEntities()
        {
            EntityEntry[] entityEntries = _dbContext.ChangeTracker.Entries().ToArray();
            foreach (EntityEntry entityEntry in entityEntries)
                entityEntry.State = EntityState.Detached;
        }

        public async Task<int> SaveAsync()
        {
            var res = await _dbContext.SaveChangesAsync();
            DetachAllEntities();
            return res;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IDbContextTransaction BeginTrainsaction(IsolationLevel IsolationLevel = IsolationLevel.Unspecified)
        {
            if (IsolationLevel == IsolationLevel.Unspecified)
                CurrentTransaction = _dbContext.Database.BeginTransaction();
            else
                CurrentTransaction = _dbContext.Database.BeginTransaction(IsolationLevel);
            return CurrentTransaction;
        }

        public IExecutionStrategy CreateExecutionStrategy()
        {
            return _dbContext.Database.CreateExecutionStrategy();
        }
    }
}
