using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BusinessRegister.Dal.Models;
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

        public async Task CheckIfDatabaseExists()
        {
            const string query = @"";

            try
            {
                using (var connection = new SqlConnection(BusinessRegistryConnectionString))
                using (var command = new SqlCommand())
                {
                    await connection.OpenAsync();
                    command.Connection = connection;

                    command.CommandText = query;

                    await command.ExecuteNonQueryAsync();
                }

            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                throw;
            }
        }
    }
}