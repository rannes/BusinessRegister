using System.Threading.Tasks;

namespace BusinessRegister.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Database version related 
    /// </summary>
    public interface IDatabaseVersionRepository
    {
        /// <summary>
        /// Update database version number to new value.
        /// </summary>
        /// <param name="newVersionNumber">New version number what to update to.</param>
        Task UpdateDatabaseVersionNumber(int newVersionNumber);

        /// <summary>
        /// Get current database version
        /// </summary>
        /// <returns>Current database version. -1 if not found.</returns>
        Task<int> GetCurrentDatabaseVersion();
    }
}