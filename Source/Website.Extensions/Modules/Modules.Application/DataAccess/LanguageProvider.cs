using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class LanguageProvider : DataProvider
    {
        private static readonly string ScriptGetAllLanguage =
            $"Select * from dbo.{LanguageTable.TableName} with(nolock) order by {LanguageTable.Name}";

        public List<LanguageData> GetAllLanguage()
        {
            Connector.ExecuteSql<LanguageData, List<LanguageData>>(ScriptGetAllLanguage,
                out List<LanguageData> result);
            return result;
        }


        private static readonly string ScriptGetLanguage = $@"
            Select * from dbo.{LanguageTable.TableName} with(nolock)
            where {LanguageTable.LanguageID} = @{LanguageTable.LanguageID}";

        public LanguageData GetLanguage(string languageID)
        {
            Connector.AddParameter(LanguageTable.LanguageID, SqlDbType.Int, languageID);
            Connector.ExecuteSql(ScriptGetLanguage, out LanguageData result);
            return result;
        }
    }
}