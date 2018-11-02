using System.Threading.Tasks;
using BusinessRegister.Dal.Models;
using BusinessRegister.Dal.Models.Consts;
using BusinessRegister.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessRegister.Dal.Repositories
{
    /// <inheritdoc cref="IDatabaseVersionRepository" />
    public class DatabaseVersionRepository : BaseRepository, IDatabaseVersionRepository
    {
        /// <inheritdoc />
        public DatabaseVersionRepository(ConnectionString connectionString,
            ILogger logger) : base(connectionString, logger)
        {
        }

        /// <inheritdoc />
        public async Task UpdateDatabaseVersionNumber(int newVersionNumber)
        {
            var query = $@" IF EXISTS (SELECT 1 FROM {TableName.DatabaseVersion})
                            BEGIN
                                UPDATE 
                                    {TableName.DatabaseVersion} 
                                SET 
		                            VersionNo = {newVersionNumber}
                            END ELSE BEGIN 
	                            INSERT INTO {TableName.DatabaseVersion} (VersionNo) 
	                            VALUES ({newVersionNumber})
                            END;";

            await RepositorySqlHelper.ExcecuteNonQueryAsync(query);
        }


        /// <inheritdoc />
        public async Task<short> GetCurrentDatabaseVersion()
        {
            var query = $@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                       WHERE TABLE_NAME = N'{TableName.DatabaseVersion.Replace("[", "").Replace("]", "")}')
                            BEGIN
	                            SELECT VersionNo FROM {TableName.DatabaseVersion}
                            END
                            ELSE
                            BEGIN
	                            SELECT -1
                            END;";

            return (short) await RepositorySqlHelper.ExcecuteNonScalarAsync(query);
        }
    }
}