using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BusinessRegister.Dal.Repositories.Helpers
{
    /// <summary>
    /// Sql execution helpers.
    /// </summary>
    public class RepositorySqlHelper
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        /// <inheritdoc />
        public RepositorySqlHelper(string connectionString, ILogger logger)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        /// <summary>
        /// Execute Query without any result. Throw on exception
        /// </summary>
        /// <param name="query">Sql query to be executed</param>
        public async Task ExcecuteNonQueryAsync(string query)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    if (connection.State != ConnectionState.Open)
                        await connection.OpenAsync();

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Execute Query and returns first column first row value. Throw on exception
        /// </summary>
        /// <param name="query">Sql query to be executed</param>
        /// <returns>Object from First column first row</returns>
        public async Task<object> ExcecuteNonScalarAsync(string query)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    if (connection.State != ConnectionState.Open)
                        await connection.OpenAsync();

                    return await command.ExecuteScalarAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}