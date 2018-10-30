using System;
using System.Data.SqlClient;
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
    }
}