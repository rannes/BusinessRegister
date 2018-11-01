using System;
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
        private readonly IDatabaseVersionRepository _databaseVersionRepository;

        /// <inheritdoc />
        public DatabaseSetupRepository(ConnectionString connectionString,
            ILogger logger) : base(connectionString, logger)
        {
            _databaseVersionRepository = new DatabaseVersionRepository(connectionString, logger);
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
        public async Task DatabaseMigration()
        {
            var dbVersion = await _databaseVersionRepository.GetCurrentDatabaseVersion();

            if (dbVersion < 0)
                await DatabaseInitialSetup();
            if (dbVersion < 1)
                await UpgradeToVersion1();
        }

        /// <summary>
        /// Database intial Setup.
        /// Create first required table: <see cref="TableName.DatabaseVersion"/> and populate it.
        /// Also limits it with trigger so it cannot never be empty table.
        /// Sets Database version to 0
        /// </summary>
        private async Task DatabaseInitialSetup()
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createTable = $@"CREATE TABLE {TableName.DatabaseVersion}(
	                                    [VersionNo] [smallint] NOT NULL
                                    ) ON [PRIMARY];";

                await RepositorySqlHelper.ExcecuteNonQueryAsync(createTable);

                var createTrigger = $@"
-- =============================================
-- Author:		Rannes Pärn
-- Create date: {DateTime.Now:dd.MM.yyyy}
-- Description:	Instead of trigger for DatabaseVersion so it can not have more than 1 row and do not allow Deletion for the value.
-- =============================================
CREATE TRIGGER [dbo].[BusinessRegister.DBVer_InsteadOfInsUpDel] 
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

                await _databaseVersionRepository.UpdateDatabaseVersionNumber(0);
                transaction.Complete();
            }
        }

        private async Task UpgradeToVersion1()
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                const string query = "";
                await RepositorySqlHelper.ExcecuteNonQueryAsync(query);

                //await _databaseVersionRepository.UpdateDatabaseVersionNumber(1);
                transaction.Complete();
            }
        }


    }
}