
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newspaper.Data.DataContext;
using Newspaper.Data.DbModels;
using Newspaper.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Repositories.NewspaperRepositories.Generic
{
    public class NewspaperGenericRepository<T>: IGRepository<T> where T: BaseEntity
    {
        #region Private fields
        protected readonly DbContext _dbContext;
        private bool _disposed = false;
        #endregion

        #region Constructor
        public NewspaperGenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion
        #region Add Methods
        /// <summary>
        /// Insert single entity 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual object Add(T entity)
        {
            return _dbContext.Set<T>().Add(entity).Entity;
        }

        /// <summary>
        /// Insert single entity asynchronously
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(T t)
        {
            await _dbContext.Set<T>().AddAsync(t);
        }

        /// <summary>
        /// Insert list of entities
        /// </summary>
        /// <param name="entities"></param>
        public virtual void AddRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
        }

        /// <summary>
        /// Insert list of entities asynchronously
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }
        #endregion

        #region Count Methods
        /// <summary>
        /// Retrieve the count of currently exisiting records
        /// </summary>
        /// <returns></returns>
        public int Count(bool withDeleted = false)
        {
            var count = _dbContext.Set<T>().Count();
            if (!withDeleted)
                count = _dbContext.Set<T>().Where(x => x.IsDeleted).Count();

            return count;
        }

        /// <summary>
        /// Retrieve the count of currently exisiting records asynchronously
        /// </summary>
        /// <returns></returns>

        public Task<int> CountAsync(bool withDeleted = false)
        {
            var count = _dbContext.Set<T>().CountAsync();
            if (!withDeleted)
                count = _dbContext.Set<T>().Where(x => x.IsDeleted).CountAsync();

            return count;
        }

        #endregion

        #region Minimum Methods
        /// <summary>
        /// Returns the minimum value of generic IQueryable 
        /// </summary>
        /// <returns></returns>
        public T GetMinimum(bool withDeleted = false)
        {
            var min = _dbContext.Set<T>().Min();
            if (!withDeleted)
                min = _dbContext.Set<T>().Where(x => x.IsDeleted).Min();

            return min;
        }

        /// <summary>
        /// Returns the minimum value of generic IQueryable asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<T> GetMinimumAsync(bool withDeleted = false)
        {
            var min = _dbContext.Set<T>().MinAsync();
            if (!withDeleted)
                min = _dbContext.Set<T>().Where(x => x.IsDeleted).MinAsync();

            return min;
        }

        /// <summary>
        /// Returns the minimum value of generic IQueryable using given key
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public object GetMinimum(Expression<Func<T, object>> selector, bool withDeleted = false)
        {
            var min = _dbContext.Set<T>().Min(selector);
            if (!withDeleted)
                min = _dbContext.Set<T>().Where(x => x.IsDeleted).Min(selector);

            return min;
        }

        /// <summary>
        /// Returns the minimum value of generic IQueryable using given key asynchronously
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Task<object> GetMinimumAsync(Expression<Func<T, object>> selector, bool withDeleted = false)
        {
            var min = _dbContext.Set<T>().MinAsync(selector);
            if (!withDeleted)
                min = _dbContext.Set<T>().Where(x => x.IsDeleted).MinAsync(selector);

            return min;
        }
        #endregion

        #region Maximum Methods
        /// <summary>
        /// Returns the maximum value of generic IQueryable
        /// </summary>
        /// <returns></returns>
        public T GetMaximum(bool withDeleted = false)
        {
            var max = _dbContext.Set<T>().Max();
            if (!withDeleted)
                max = _dbContext.Set<T>().Where(x => x.IsDeleted).Max();

            return max;
        }

        /// <summary>
        /// Returns the maximum value of generic IQueryable asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<T> GetMaximumAsync(bool withDeleted = false)
        {
            var max = _dbContext.Set<T>().MaxAsync();
            if (!withDeleted)
                max = _dbContext.Set<T>().Where(x => x.IsDeleted).MaxAsync();

            return max;
        }

        /// <summary>
        /// Returns the maximum value of generic IQueryable using given key
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public object GetMaximum(Expression<Func<T, object>> selector, bool withDeleted = false)
        {
            var max = _dbContext.Set<T>().Max(selector);
            if (!withDeleted)
                max = _dbContext.Set<T>().Where(x => x.IsDeleted).Max(selector);

            return max;
        }

        /// <summary>
        /// Returns the maximum value of generic IQueryable using given key asynchronously
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public Task<object> GetMaximumAsync(Expression<Func<T, object>> selector, bool withDeleted = false)
        {
            var max = _dbContext.Set<T>().MaxAsync(selector);
            if (!withDeleted)
                max = _dbContext.Set<T>().Where(x => x.IsDeleted).MaxAsync(selector);

            return max;
        }
        #endregion

        #region Find Methods
        /// <summary>
        /// Searches for record(s) using given keys
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public T Find(params object[] keys)
        {
            return _dbContext.Set<T>().Find(keys);
        }

        /// <summary>
        /// Searches for record(s) using given condition
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T Find(Func<T, bool> where)
        {
            return _dbContext.Set<T>().Find(where);
        }

        /// <summary>
        /// Searches for record(s) using given keys asynchronously
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<T> FindAsync(params object[] keys)
        {
            return await _dbContext.Set<T>().FindAsync(keys);
        }

        /// <summary>
        /// Searches for record(s) that match(es) a given condition
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _dbContext.Set<T>().FindAsync(match);
        }
        #endregion

        #region Get Methods
        /// <summary>
        /// Retrieve all records
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll(bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking();
            if (!withDeleted)
                return query.Where(x => !x.IsDeleted);

            return query;
        }

        /// <summary>
        /// Retrieve all records based on a given condition
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> where, bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(where);
            if (!withDeleted)
                return query.Where(x => !x.IsDeleted);

            return query.AsQueryable();
        }

        /// <summary>
        /// Retrieve all records based on a given condition and key
        /// </summary>
        /// <param name="where"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        public virtual IQueryable<object> GetAll(Expression<Func<T, bool>> where, Expression<Func<T, object>> select, bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(where);
            if (!withDeleted)
                return query.Where(x => !x.IsDeleted);

            return query.Select(select).AsQueryable();
        }

        /// <summary>
        /// Retrieve all records asynchronously
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IQueryable<T>> GetAllAsync(bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(x => !x.IsDeleted);
            if (!withDeleted)
                return query.Where(x => !x.IsDeleted);

            var res = await query.ToListAsync();
            return res.AsQueryable();
        }

        /// <summary>
        /// Retrieve all records based on a given condition asynchronously
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> expression, bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(expression);
            if (!withDeleted)
                return query.Where(x => !x.IsDeleted);

            return (await query.ToListAsync()).AsQueryable();
        }

        /// <summary>
        /// Retrieve all records based on a given condition and selector asynchronously
        /// </summary>
        /// <param name="where"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        public virtual async Task<IQueryable<object>> GetAllAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> select, bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(where);
            if (!withDeleted)
                return query.Where(x => !x.IsDeleted);

            return (await query.Select(select).ToListAsync()).AsQueryable();
        }

        /// <summary>
        /// Retrieve all records with set of properties
        /// </summary>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IQueryable<T> GetAllIncluding(Expression<Func<T, bool>> where, bool withDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();

            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                queryable = queryable.Include(includeProperty);

            if (!withDeleted)
                queryable = queryable.Where(x => !x.IsDeleted);

            return queryable.Where(where).AsNoTracking();
        }

        /// <summary>
        /// Retrieve all records with set of properties asynchronously
        /// </summary>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public async Task<IQueryable<T>> GetAllIncludingAsync(Expression<Func<T, bool>> where, bool withDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var queryable = _dbContext.Set<T>().AsNoTracking().Where(where);
            if (!withDeleted)
                queryable = queryable.Where(x => !x.IsDeleted);

            // need more investigation to avoid actual exeution by tolist()
            return (await queryable.Include(includeProperties.ToString()).ToListAsync()).AsQueryable();
        }

        /// <summary>
        /// Retrieve the first record
        /// </summary>
        /// <returns></returns>
        public T GetFirst(bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking();
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Retrieve the first record asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<T> GetFirstAsync(bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking();
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieve the first record based on a given condition
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T GetFirst(Expression<Func<T, bool>> where, bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(where);
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Retrieve the first record based on a given condition asynchronously
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> where, bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(where);
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get first record with include
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public T GetFirst(Expression<Func<T, bool>> where, bool withDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(where);
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                query = query.Include<T, object>(includeProperty);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Get first record async with include
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public Task<T> GetFirstAsync(Expression<Func<T, bool>> where, bool withDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(where);
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                query = query.Include<T, object>(includeProperty);

            return query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieve the last record
        /// </summary>
        /// <returns></returns>
        public T GetLast(bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking();
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            return query.LastOrDefault();
        }

        /// <summary>
        /// Retrieve the last record asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<T> GetLastAsync(bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking();
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            return await query.LastOrDefaultAsync();
        }

        /// <summary>
        /// Retrieve the last record based on a given condition
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T GetLast(Expression<Func<T, bool>> where, bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(where);
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            return query.LastOrDefault();
        }

        public T GetLast(Expression<Func<T, bool>> where, bool withDeleted = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(where);
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                query = query.Include<T, object>(includeProperty);

            return query.LastOrDefault();
        }

        /// <summary>
        /// Retrieve the last record based on a given condition asynchronously
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<T> GetLastAsync(Expression<Func<T, bool>> where, bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking().Where(where);
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            return await query.LastOrDefaultAsync();
        }
        #endregion



        #region Remove Methods
        /// <summary>
        /// Logically or physically deleting record based on the entity type
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual EntityEntry<T> Remove(T entity, bool hard = false)
        {
            if (!hard)
            {
                entity.IsDeleted = true;
                return _dbContext.Update(entity);
            }
            else
                return _dbContext.Remove(entity);
        }

        /// <summary>
        /// Logically or physically deleting list of records based on the entity type
        /// </summary>
        /// <param name="entities"></param>
        public virtual void RemoveRange(IEnumerable<T> entities, bool hard = false)
        {
            if (!hard)
            {
                foreach (var item in entities)
                {
                    item.IsDeleted = true;

                    _dbContext.Update(item);
                }
            }
            else
                _dbContext.Set<T>().RemoveRange(entities);
        }

        public virtual void Truncate()
        {
            var sqlCmd = $"TRUNCATE TABLE {typeof(T).Name}" + "s";
            _dbContext.Database.ExecuteSqlRaw(sqlCmd);
        }
        #endregion

        #region Update Method
        /// <summary>
        /// Update record data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual EntityEntry<T> Update(T entity)
        {
            return _dbContext.Update(entity);
        }
        #endregion



        #region Helper Methods

        /// <summary>
        /// Generic filter
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private IQueryable<T> Filter<TProperty>(IQueryable<T> dbSet,
            Expression<Func<T, TProperty>> property, TProperty value)
            where TProperty : IComparable
        {

            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
            {

                throw new ArgumentException("Property expected", "property");
            }

            Expression left = property.Body;
            Expression right = Expression.Constant(value, typeof(TProperty));
            Expression searchExpression = Expression.Equal(left, right);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(
                searchExpression, new ParameterExpression[] { property.Parameters.Single() });

            return dbSet.Where(lambda);
        }
        #endregion

        #region Release Unmanaged Resources
        /// <summary>
        /// Release un managed resources from memeory
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public bool Any(Expression<Func<T, bool>> where, bool withDeleted = false)
        {
            var query = _dbContext.Set<T>().AsNoTracking();
            if (!withDeleted)
                query = query.Where(x => !x.IsDeleted);

            return query.Any(where);
        }

        #endregion

        #region Enum Helpers
        private enum OrderByType
        {

            Ascending,
            Descending
        }
        #endregion
    }
}
