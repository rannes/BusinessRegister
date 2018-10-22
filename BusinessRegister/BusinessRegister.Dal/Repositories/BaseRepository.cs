using BusinessRegister.Dal.Models;
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

        /// <inheritdoc />
        protected BaseRepository(ConnectionString connectionString, ILogger logger)
        {
            BusinessRegistryConnectionString = connectionString.BusinessRegister;
            Logger = logger;
        }
    }
}