using System.Threading.Tasks;

namespace BusinessRegister.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Database setup repository
    /// </summary>
    public interface IDatabaseSetupRepository
    {
        /// <summary>
        /// Test if database connection can be established
        /// </summary>
        Task TestDatabaseConnection();

        /// <summary>
        /// Complete Database migration. Updates database to laters version
        /// </summary>
        Task CompleteDatabaseMigration();
    }
}