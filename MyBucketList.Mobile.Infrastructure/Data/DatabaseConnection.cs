using SQLite;
using System;
using System.IO;

namespace MyBucketList.Mobile.Infrastructure.Data
{
    public class DatabaseConnection
    {
        #region Fields

        private string _dbPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "croni.db3");

        #endregion

        public SQLiteConnection SyncDb { get; }
        public SQLiteAsyncConnection AsyncDb { get; }

        private readonly TimeSpan _busyTimeout = new TimeSpan(0, 0, 0, 5);


        public DatabaseConnection(string name, string path)
        {
            var dbPath = Path.Combine(path, name);

            if (AsyncDb == null)
            {
                AsyncDb = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex);
                AsyncDb.SetBusyTimeoutAsync(_busyTimeout);
            }

            SyncDb ??= new SQLiteConnection(dbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex)
            {
                BusyTimeout = _busyTimeout
            };
        }
    }
}
