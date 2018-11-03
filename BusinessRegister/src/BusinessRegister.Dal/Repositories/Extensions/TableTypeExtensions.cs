using System;
using System.Collections.Generic;
using System.Data;
using BusinessRegister.Dal.Models;
using BusinessRegister.Dal.Models.Consts;
using Microsoft.SqlServer.Server;

namespace BusinessRegister.Dal.Repositories.Extensions
{
    /// <summary>
    /// Extensions to turn certain data to SqlDataRecords for TableTypes
    /// </summary>
    public static class TableTypeExtensions
    {
        /// <summary>
        /// Split list to chunks
        /// </summary>
        /// <typeparam name="T">List to split</typeparam>
        /// <param name="listToSplit"></param>
        /// <param name="size">Size of the chunks</param>
        /// <returns>Splited list</returns>
        public static List<List<T>> SplitList<T>(this List<T> listToSplit, int size = 10000)
        {
            var list = new List<List<T>>();
            for (var i = 0; i < listToSplit.Count; i += size)
                list.Add(listToSplit.GetRange(i, Math.Min(size, listToSplit.Count - i)));
            return list;
        }

        /// <summary>
        /// To TableType <see cref="TableType.Company"/>
        /// </summary>
        /// <param name="companies">Collection of data to change</param>
        /// <returns>List of <see cref="SqlDataRecord"/> objects</returns>
        public static List<SqlDataRecord> ToTableTypeCompanies(this IList<Company> companies)
        {
            if (companies == null || companies.Count == 0)
                return null;

            var returnList =  new List<SqlDataRecord>();
            foreach (var company in companies)
            {
                var sqlDataRecord = new SqlDataRecord(
                    new SqlMetaData("Name", SqlDbType.NVarChar, 400),
                    new SqlMetaData("BusinessCode", SqlDbType.NVarChar, 50),
                    new SqlMetaData("VatNo", SqlDbType.NVarChar, 200),
                    new SqlMetaData("Status", SqlDbType.VarChar, 20),
                    new SqlMetaData("FullAddress", SqlDbType.NVarChar, 1024),
                    new SqlMetaData("Url", SqlDbType.VarChar, 200)
                );

                sqlDataRecord.SetSqlString(sqlDataRecord.GetOrdinal("Name"), company.CompanyName);
                sqlDataRecord.SetSqlString(sqlDataRecord.GetOrdinal("BusinessCode"), company.BusinessCode);
                sqlDataRecord.SetSqlString(sqlDataRecord.GetOrdinal("VatNo"), company.VatNo);
                sqlDataRecord.SetSqlString(sqlDataRecord.GetOrdinal("Status"), company.Status.ToString());
                sqlDataRecord.SetSqlString(sqlDataRecord.GetOrdinal("FullAddress"), company.CompanyAddress.FullAddress);
                sqlDataRecord.SetSqlString(sqlDataRecord.GetOrdinal("Url"), company.UrlOfAriregister);

                returnList.Add(sqlDataRecord);
            }

            return returnList;
        }
    }
}