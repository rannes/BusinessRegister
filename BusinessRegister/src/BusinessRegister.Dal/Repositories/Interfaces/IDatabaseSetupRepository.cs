using System.Threading.Tasks;

namespace BusinessRegister.Dal.Repositories
{
    /// <summary>
    /// Database setup repository
    /// </summary>
    public interface IDatabaseSetupRepository
    {
        Task CheckIfDatabaseExists();
    }
}