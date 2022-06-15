
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Repositories.GenericRepository
{
    public interface IGRepository<T> where T :class
    {
        #region Find Methods
        T Find(params object[] keys);
        Task<T> FindAsync(params object[] keys);
        T Find(Func<T, bool> where);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        #endregion

        #region Add Methods
        object Add(T entity);
        Task AddAsync(T t);
        void AddRange(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities);
        #endregion

        #region Count Methods
        int Count(bool withDeleted = false);
        Task<int> CountAsync(bool withDeleted = false);
        #endregion

        #region Remove Methods
        EntityEntry<T> Remove(T entity, bool hard = false);
        void RemoveRange(IEnumerable<T> entities, bool hard = false);

        void Truncate();

        #endregion

        #region Get Methods
        IQueryable<T> GetAll(bool withDeleted = false);
        Task<IQueryable<T>> GetAllAsync(bool withDeleted = false);
        IQueryable<T> GetAll(Expression<Func<T, bool>> where, bool withDeleted = false);
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> where, bool withDeleted = false);
        IQueryable<object> GetAll(Expression<Func<T, bool>> where, Expression<Func<T, object>> select, bool withDeleted = false);
        Task<IQueryable<object>> GetAllAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> select, bool withDeleted = false);
        IQueryable<T> GetAllIncluding(Expression<Func<T, bool>> where, bool withDeleted = false, params Expression<Func<T, object>>[] includeProperties);
        Task<IQueryable<T>> GetAllIncludingAsync(Expression<Func<T, bool>> where, bool withDeleted = false, params Expression<Func<T, object>>[] includeProperties);

        T GetFirst(bool withDeleted = false);
        Task<T> GetFirstAsync(bool withDeleted = false);
        T GetLast(bool withDeleted = false);
        Task<T> GetLastAsync(bool withDeleted = false);
        T GetFirst(Expression<Func<T, bool>> where, bool withDeleted = false);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> where, bool withDeleted = false);
        T GetFirst(Expression<Func<T, bool>> where, bool withDeleted = false, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> where, bool withDeleted = false, params Expression<Func<T, object>>[] includeProperties);
        T GetLast(Expression<Func<T, bool>> where, bool withDeleted = false);
        T GetLast(Expression<Func<T, bool>> where, bool withDeleted = false, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetLastAsync(Expression<Func<T, bool>> where, bool withDeleted = false);

        #endregion

        #region Update Method
        EntityEntry<T> Update(T entity);
        #endregion

        #region Release Unmanaged Resources
        void Dispose(bool disposing);
        #endregion

        #region GetMinimum Methods
        T GetMinimum(bool withDeleted = false);
        Task<T> GetMinimumAsync(bool withDeleted = false);
        object GetMinimum(Expression<Func<T, object>> selector, bool withDeleted = false);
        Task<object> GetMinimumAsync(Expression<Func<T, object>> selector, bool withDeleted = false);

        #endregion

        #region GetMaximum Methods
        T GetMaximum(bool withDeleted = false);
        Task<T> GetMaximumAsync(bool withDeleted = false);
        object GetMaximum(Expression<Func<T, object>> selector, bool withDeleted = false);
        Task<object> GetMaximumAsync(Expression<Func<T, object>> selector, bool withDeleted = false);
        #endregion

        bool Any(Expression<Func<T, bool>> where, bool withDeleted = false);
    }
}
