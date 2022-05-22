using Polly;
using SQLite;
using System;
using System.Threading.Tasks;

namespace MyBucketList.Mobile.Infrastructure.Data.Repositories
{
    public class RepositoryBase
    {
        protected DatabaseConnection _databaseConnection;

        public RepositoryBase(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        protected static Task<T> AttemptAndRetry<T>(Func<Task<T>> action, int numRetries = 3)
        {
            return Policy.Handle<SQLiteException>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action);
        }

        protected static Task AttemptAndRetry(Func<Task> action, int numRetries = 3)
        {
            return Policy.Handle<SQLiteException>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action);
        }

        static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromMilliseconds(Math.Pow(2, attemptNumber));
    }
}
