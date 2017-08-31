using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class EducationProvider : DataProvider
    {
        public List<EducationData> GetList()
        {
            Connector.ExecuteSql<EducationData, List<EducationData>>(
                "dbo.APP_SP_GetEducation", out List<EducationData> outList);
            return outList;
        }


        public EducationData GetItem(string code)
        {
            Connector.AddParameter(EducationTable.EducationCode, SqlDbType.VarChar, code);
            Connector.ExecuteSql("dbo.APP_SP_GetEducation", out EducationData result);
            return result;
        }
    }
}
