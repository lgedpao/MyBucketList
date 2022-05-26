using MyBucketList.Core.Interfaces.Data.Repositories;
using Polly;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyBucketList.Mobile.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private Lazy<DatabaseConnection> _lazyDBConnection;

        public Repository(Lazy<DatabaseConnection> lazyDBConnection)
        {
            _lazyDBConnection = lazyDBConnection;
        }

        protected DatabaseConnection _dbConnection => _lazyDBConnection.Value;

        public async Task<int> DeleteItem(T entity)
        {
            return await AttemptAndRetry(() => _dbConnection.AsyncDb.DeleteAsync(entity)).ConfigureAwait(false);
        }

        public async Task<int> DeleteAllItems()
        {
            return await AttemptAndRetry(() => _dbConnection.AsyncDb.DeleteAllAsync<T>()).ConfigureAwait(false);
        }

        public async Task<int> Execute(string sqlText)
        {
            return await AttemptAndRetry(() => _dbConnection.AsyncDb.ExecuteAsync(sqlText)).ConfigureAwait(false);
        }

        public async Task<IList<T>> Get()
        {
            return await AttemptAndRetry(() => _dbConnection.AsyncDb.Table<T>().ToListAsync()).ConfigureAwait(false);
        }

        public async Task<IList<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            return await AttemptAndRetry(async () =>
            {
                var query = _dbConnection.AsyncDb.Table<T>();

                if (predicate != null)
                    query = query.Where(predicate);

                if (orderBy != null)
                    query = query.OrderBy(orderBy);

                return await query.ToListAsync();
            }).ConfigureAwait(false);
        }

        public async Task<T> Get(string id)
        {
            return await AttemptAndRetry(() => _dbConnection.AsyncDb.FindAsync<T>(id)).ConfigureAwait(false);
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            return await AttemptAndRetry(() => _dbConnection.AsyncDb.FindAsync<T>(predicate)).ConfigureAwait(false);
        }

        public async Task<int> InsertItem(T entity)
        {
            return await AttemptAndRetry(() => _dbConnection.AsyncDb.InsertAsync(entity)).ConfigureAwait(false);
        }

        public async Task<int> InsertAllItems(IEnumerable<T> list)
        {
            return await AttemptAndRetry(() => _dbConnection.AsyncDb.InsertAllAsync(list)).ConfigureAwait(false);
        }

        public async Task<IList<T>> Query(string query, params object[] args)
        {
            return await AttemptAndRetry(() => _dbConnection.AsyncDb.QueryAsync<T>(query, args)).ConfigureAwait(false);
        }

        public async Task<int> UpdateItem(T entity)
        {
            return await AttemptAndRetry(() => _dbConnection.AsyncDb.UpdateAsync(entity)).ConfigureAwait(false);
        }

        public async Task<int> UpdateAllItems(IEnumerable<T> entities)
        {
            return await AttemptAndRetry(() => _dbConnection.AsyncDb.UpdateAllAsync(entities)).ConfigureAwait(false);
        }

        protected Task<TResult> AttemptAndRetry<TResult>(Func<Task<TResult>> action, int numRetries = 3)
        {
            return Policy.Handle<SQLiteException>().WaitAndRetryAsync(numRetries, PollyRetryAttempt).ExecuteAsync(action);
        }

        protected TResult AttemptAndRetry<TResult>(Func<TResult> action, int numRetries = 3)
        {
            return Policy.Handle<SQLiteException>().WaitAndRetry(numRetries, PollyRetryAttempt).Execute(action);
        }

        private TimeSpan PollyRetryAttempt(int attemptNumber) => TimeSpan.FromMilliseconds(Math.Pow(2, attemptNumber));
    }
}
