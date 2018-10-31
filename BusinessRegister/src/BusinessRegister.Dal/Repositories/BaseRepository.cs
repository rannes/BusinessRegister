using BusinessRegister.Dal.Models;
using BusinessRegister.Dal.Repositories.Helpers;
using Microsoft.Extensions.Logging;

namespace BusinessRegister.Dal.Repositories
{
    /// <summary>
    /// Base repository to share for most repositories
    /// </summary>
    public abstract class BaseRepository
    {
        /// <summary>
        /// BusinessRegistry connection string database
        /// </summary>
        protected readonly string BusinessRegistryConnectionString;

        /// <summary>
        /// <see cref="ILogger"/> object to log data.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Repository SQL helper
        /// </summary>
        protected readonly RepositorySqlHelper RepositorySqlHelper;

        /// <inheritdoc />
        protected BaseRepository(ConnectionString connectionString, ILogger logger)
        {
            RepositorySqlHelper = new RepositorySqlHelper(connectionString.BusinessRegister, logger);
            BusinessRegistryConnectionString = connectionString.BusinessRegister;
            Logger = logger;
        }
    }
}