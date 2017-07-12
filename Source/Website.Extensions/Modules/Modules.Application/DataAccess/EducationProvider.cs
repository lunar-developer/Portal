using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class EducationProvider:DataProvider
    {
        private static readonly string ScriptAll = $@"Select * 
                                            from dbo.{EducationTable.TableName} with(nolock)";
        public List<EducationData> GetList()
        {
            Connector.ExecuteSql<EducationData, List<EducationData>>(ScriptAll, out List<EducationData> outList);
            return outList;
        }


        private static readonly string Script = $@"
            Select * 
            from dbo.{EducationTable.TableName} with(nolock) 
            where {EducationTable.EducationCode} = @{EducationTable.EducationCode}";
        public EducationData GetItem(string id)
        {
            Connector.AddParameter(EducationTable.EducationCode, SqlDbType.VarChar, id);
            Connector.ExecuteSql(Script, out EducationData result);
            return result;
        }
    }
}
