using MyBucketList.Mobile.Infrastructure.Data.Models;
using MyBucketList.Mobile.Infrastructure.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace MyBucketList.Mobile.Infrastructure.Data
{
    public class Database : RepositoryBase
    {
        #region Fields

        const int _databaseVersion = 1;

        #endregion

        #region Ctor

        public Database(DatabaseConnection databaseConnection) : base(databaseConnection) { }

        #endregion

        #region Methods

        public async Task InitializeDatabase()
        {
            await CreateTables();

            int currentDbVersion = await GetDatabaseVersion();

            if (currentDbVersion < _databaseVersion)
                await UpgradeDatabase();
        }

        private async Task<int> GetDatabaseVersion()
        {
            return await AttemptAndRetry(() => _databaseConnection.AsyncDb.ExecuteScalarAsync<int>("PRAGMA user_version")).ConfigureAwait(false);
        }

        private async Task UpgradeDatabase()
        {
            //the first time ever we get this value after the database creation
            //this should be equals 0. but it's ok and will perform the correct
            //updates in the switch.
            int currentDbVersion = await GetDatabaseVersion();

            if (currentDbVersion < _databaseVersion)
            {
                //we have to ignore the current database updates, so start from the next
                int startUpgradingFrom = currentDbVersion + 1;
                //if we are are, database upgrade is needed
                switch (startUpgradingFrom)
                {
                    case 1: //starting version
                        InitializeData();
                        goto case 2;
                    case 2:
                        UpgradeFrom1To2();
                        goto case 3;
                    case 3:
                        UpgradeFrom2To3();
                        goto case 4;
                    case 4: //ecc.. ecc..
                        break;
                    default:
                        //if we are here something with the update went wrong,
                        //deleting and recreating the database is the only
                        //possible action to perform
                        throw new Exception("something went really wrong");
                }

                await SetDatabaseToVersion(_databaseVersion);
            }
        }

        private void InitializeData()
        {

        }

        private static void UpgradeFrom1To2() { }

        private static void UpgradeFrom2To3() { }

        private async Task<int> SetDatabaseToVersion(int version)
        {
            return await AttemptAndRetry(() => _databaseConnection.AsyncDb.ExecuteAsync("PRAGMA user_version = " + version)).ConfigureAwait(false);
        }

        private async Task CreateTables()
        {
            await AttemptAndRetry(async () =>
            {
                await _databaseConnection.AsyncDb.CreateTableAsync<BucketListItemData>();

                await _databaseConnection.AsyncDb.ExecuteAsync("PRAGMA foreign_keys = ON");
            }).ConfigureAwait(false);
        }

        public async Task DeleteTables()
        {
            await AttemptAndRetry(async () =>
            {
                await _databaseConnection.AsyncDb.DropTableAsync<BucketListItemData>();
            }).ConfigureAwait(false);
        }

        #endregion
    }
}
