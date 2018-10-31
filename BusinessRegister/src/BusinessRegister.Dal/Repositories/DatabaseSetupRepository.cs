using System;
using System.Threading.Tasks;
using BusinessRegister.Dal.Exceptions;
using BusinessRegister.Dal.Models;
using BusinessRegister.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessRegister.Dal.Repositories
{
    /// <inheritdoc cref="IDatabaseSetupRepository" />
    public class DatabaseSetupRepository : BaseRepository, IDatabaseSetupRepository
    {
        /// <inheritdoc />
        public DatabaseSetupRepository(ConnectionString connectionString,
            ILogger logger) : base(connectionString, logger)
        {
        }

        /// <inheritdoc />
        public async Task TestDatabaseConnection()
        {
            try
            {
                await RepositorySqlHelper.ExcecuteNonQueryAsync("");
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                if (e.Message.Contains("Cannot open database"))
                    throw new BrInvalidOperationException("Database connection could not be opened. Might be invalid Database name", 
                        ResultCode.DatabaseConnectionFailedToOpen, e);
                if (e.Message.Contains("A connection was successfully established with the server, but then an error occurred during the login process."))
                    throw new BrInvalidOperationException("Username or Password is invalid.", ResultCode.UsernameOrPasswordIsInvalid, e);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task CompleteDatabaseMigration()
        {
            var dbVersion = await GetCurrentDatabaseVersion();

            if (dbVersion == 0)
                await SetDatabaseInitalSetup();
            if (dbVersion <= 1)
                await SetDatabaseVersion1();
        }

        /// <summary>
        /// Get current database version
        /// </summary>
        /// <returns>Current database version</returns>
        private async Task<int> GetCurrentDatabaseVersion()
        {
            const string query = @"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                               WHERE TABLE_NAME = N'BusinessRegister.DBVer')
                                    BEGIN
	                                    SELECT Version FROM BusinessRegister.DBVer
                                    END
                                    ELSE
                                    BEGIN
	                                    SELECT 0
                                    END;";

            return (int) await RepositorySqlHelper.ExcecuteNonScalarAsync(query);
        }

        private async Task SetDatabaseInitalSetup()
        {
            const string query = "";
            await RepositorySqlHelper.ExcecuteNonQueryAsync(query);
        }

        private async Task SetDatabaseVersion1()
        {
            const string query = "";
            await RepositorySqlHelper.ExcecuteNonQueryAsync(query);
        }

    }
}