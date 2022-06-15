using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Repositories.UOW
{
    public interface IUnitOfWork<T>
    {
        int Save();
        Task<int> SaveAsync();

        IDbContextTransaction BeginTrainsaction(IsolationLevel IsolationLevel = IsolationLevel.Unspecified);
        IDbContextTransaction CurrentTransaction { get; }
        IsolationLevel CurrentTransactionIsolationLevel { get; }

        IExecutionStrategy CreateExecutionStrategy();
    }
}
