﻿using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class ApplicationFieldProvider : DataProvider
    {
        private static readonly string ScriptGetAllFields = $@"
            select * from dbo.{ApplicationFieldTable.DatabaseTableName} with(nolock)";

        public List<ApplicationFieldData> GetAllFields()
        {
            Connector.ExecuteSql<ApplicationFieldData, List<ApplicationFieldData>>(
                ScriptGetAllFields, out List<ApplicationFieldData> result);
            return result;
        }

        private static readonly string ScriptGetField = $@"
            {ScriptGetAllFields} where {ApplicationFieldTable.FieldName} = @{ApplicationFieldTable.FieldName}";

        public ApplicationFieldData GetField(string fieldName)
        {
            Connector.AddParameter(ApplicationFieldTable.FieldName, SqlDbType.VarChar, fieldName);
            Connector.ExecuteSql(ScriptGetField, out ApplicationFieldData result);
            return result;
        }
    }
}