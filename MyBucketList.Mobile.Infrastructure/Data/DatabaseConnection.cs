using SQLite;
using System;
using System.IO;

namespace MyBucketList.Mobile.Infrastructure.Data
{
    public class DatabaseConnection
    {
        private string _dbPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bucketeer.db3");

        public SQLiteConnection SyncDb { get; }
        public SQLiteAsyncConnection AsyncDb { get; }

        private readonly TimeSpan _busyTimeout = new TimeSpan(0, 0, 0, 5);

        public DatabaseConnection()
        {
            if (AsyncDb == null)
            {
                AsyncDb = new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex);
                AsyncDb.SetBusyTimeoutAsync(_busyTimeout);
            }

            SyncDb ??= new SQLiteConnection(_dbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex)
            {
                BusyTimeout = _busyTimeout
            };
        }
    }
}
