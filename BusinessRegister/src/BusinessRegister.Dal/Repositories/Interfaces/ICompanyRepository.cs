using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessRegister.Dal.Models;

namespace BusinessRegister.Dal.Repositories.Interfaces
{
    /// <summary>
    /// Company related data repository
    /// </summary>
    public interface ICompanyRepository
    {
        /// <summary>
        /// Insert new companies and update existsing ones. Based on Reg.No.
        /// </summary>
        /// <param name="comapnies"></param>
        /// <returns></returns>
        Task SetBulk(IEnumerable<Company> comapnies);
    }
}