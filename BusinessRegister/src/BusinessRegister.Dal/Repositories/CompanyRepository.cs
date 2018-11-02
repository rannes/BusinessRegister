using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessRegister.Dal.Models;
using BusinessRegister.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessRegister.Dal.Repositories
{
    /// <inheritdoc cref="ICompanyRepository" />
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        /// <inheritdoc />
        public CompanyRepository(ConnectionString connectionString,
            ILogger logger) : base(connectionString, logger)
        {
        }

        public async Task Set(IEnumerable<Company> comapnies)
        {

        }
    }
}