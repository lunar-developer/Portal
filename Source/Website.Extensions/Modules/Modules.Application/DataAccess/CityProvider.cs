using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class CityProvider : DataProvider
    {
        private static readonly string ScriptGetAllCity =
            $"select * from dbo.{CityTable.TableName} with(nolock)";

        public List<CityData> GetAllCity()
        {
            Connector.ExecuteSql<CityData, List<CityData>>(ScriptGetAllCity, out List<CityData> result);
            return result;
        }

        private static readonly string ScriptGetCity =
            ScriptGetAllCity + $" where {CityTable.CityCode} = @{CityTable.CityCode}";

        public CityData GetCity(string cityCode)
        {
            Connector.AddParameter(CityTable.CityCode, SqlDbType.VarChar, cityCode);
            Connector.ExecuteSql(ScriptGetCity, out CityData result);
            return result;
        }
    }
}