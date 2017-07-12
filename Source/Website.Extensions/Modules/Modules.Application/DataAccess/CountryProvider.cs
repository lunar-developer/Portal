using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class CountryProvider: DataProvider
    {
        private static readonly string ScriptGetAllCountry =
            $"select * from dbo.{CountryTable.TableName} with(nolock) order by {CountryTable.Name}";

        public List<CountryData> GetAllCountry()
        {
            Connector.ExecuteSql<CountryData, List<CountryData>>(ScriptGetAllCountry, out List<CountryData> result);
            return result;
        }


        private static readonly string ScriptGetCountry = $@"
            select * from dbo.{CountryTable.TableName} with(nolock)
            where {CountryTable.CountryCode} = @{CountryTable.CountryCode}";

        public CountryData GetCountry(string countryCode)
        {
            Connector.AddParameter(CountryTable.CountryCode, SqlDbType.VarChar, countryCode);
            Connector.ExecuteSql(ScriptGetCountry, out CountryData result);
            return result;
        }
    }
}