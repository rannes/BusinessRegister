using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using BusinessRegister.Dal.Exceptions;
using BusinessRegister.Dal.Models;
using BusinessRegister.Dal.Models.Consts;
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
                using (var connection = new SqlConnection(BusinessRegistryConnectionString))
                {
                    await connection.OpenAsync();
                }
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
            //if (dbVersion <= 1)
            //    await SetDatabaseVersion1();
        }

        /// <summary>
        /// Get current database version
        /// </summary>
        /// <returns>Current database version</returns>
        private async Task<int> GetCurrentDatabaseVersion()
        {
            var query = $@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                       WHERE TABLE_NAME = N'{TableName.DatabaseVersion}')
                            BEGIN
	                            SELECT VersionNo FROM {TableName.DatabaseVersion}
                            END
                            ELSE
                            BEGIN
	                            SELECT 0
                            END;";

            return (int) await RepositorySqlHelper.ExcecuteNonScalarAsync(query);
        }

        private async Task SetDatabaseInitalSetup()
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createTable = $@"CREATE TABLE {TableName.DatabaseVersion}(
	                                    [VersionNo] [smallint] NOT NULL
                                    ) ON [PRIMARY];";

                await RepositorySqlHelper.ExcecuteNonQueryAsync(createTable);

                var createTrigger = $@"CREATE TRIGGER [dbo].[BusinessRegister.DBVer_InsteadOfInsUpDel] 
                                        ON [dbo].{TableName.DatabaseVersion} INSTEAD OF INSERT, UPDATE, DELETE AS 
                                BEGIN
	                                SET NOCOUNT ON;
	                                IF EXISTS (SELECT 1 FROM inserted) --INSERT OR UPDATE
	                                BEGIN
		                                IF NOT EXISTS (SELECT 1 FROM {TableName.DatabaseVersion})
		                                BEGIN
			                                INSERT INTO {TableName.DatabaseVersion} (VersionNo)
			                                VALUES ((SELECT MAX(VersionNo) FROM inserted))
		                                END ELSE 
		                                IF NOT EXISTS (SELECT 1 FROM {TableName.DatabaseVersion} WHERE VersionNo > (SELECT MAX(VersionNo) FROM inserted))
		                                BEGIN
			                                UPDATE 
				                                {TableName.DatabaseVersion} 
			                                SET 
				                                VersionNo = (SELECT MAX(VersionNo) FROM inserted)
		                                END			
	                                END;
	                                --IGNORE IF DELETE
                                END;";

                await RepositorySqlHelper.ExcecuteNonQueryAsync(createTrigger);

                transaction.Complete();
            }
        }

        private async Task SetDatabaseVersion1()
        {
            const string query = "";
            await RepositorySqlHelper.ExcecuteNonQueryAsync(query);
        }

    }
}