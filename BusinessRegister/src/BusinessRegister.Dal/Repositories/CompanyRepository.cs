using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BusinessRegister.Dal.Exceptions;
using BusinessRegister.Dal.Models;
using BusinessRegister.Dal.Models.Consts;
using BusinessRegister.Dal.Repositories.Extensions;
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

        /// <inheritdoc />
        public async Task SetBulk(IEnumerable<Company> companies)
        {
            var companiesList = companies?.ToList() ?? new List<Company>();

            if (companiesList.Count == 0)
                throw new BrArgumentException("Companies list must have at least 1 value.", ResultCode.CompaniesListIsEmpty);

            var companiesSqlDataAsWhole = companiesList.ToTableTypeCompanies();
            var companiesSqlDataBulks = companiesSqlDataAsWhole.SplitList();

            foreach (var companiesSqlDataBulk in companiesSqlDataBulks)
            {
                if (!companiesSqlDataBulk.Any())
                    continue;

                var query = $@" MERGE {TableName.Company} AS t --Target
                            USING @tableTypeRows AS s --Source
                            ON (t.BusinessCode = s.BusinessCode) 
                            WHEN MATCHED THEN 
	                            UPDATE SET
		                            t.[Name] = s.[Name],
		                            t.BusinessCode = s.BusinessCode,
		                            t.VatNo = s.VatNo,
		                            t.[Status] = s.[Status],
		                            t.FullAddress = s.FullAddress,
		                            t.[Url] = s.[Url]
                            WHEN NOT MATCHED BY TARGET THEN 
                                INSERT ([Name], BusinessCode, VatNo, [Status], FullAddress, [Url]) 
                                VALUES (s.[Name], s.BusinessCode, s.VatNo, s.[Status], s.FullAddress, s.[Url]);";

                var parameters = new object[]
                {
                    new SqlParameter("tableTypeRows", SqlDbType.Structured)
                    {
                        TypeName = TableType.Company,
                        Value = companiesSqlDataBulk
                    }
                };

                await RepositorySqlHelper.ExcecuteNonQueryAsync(query, parameters);
            }


        }
    }
}