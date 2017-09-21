using System;
using System.Collections.Generic;
using System.Data;
using Modules.MasterData.Database;
using Website.Library.DataAccess;
using Website.Library.Enum;
using Website.Library.Global;

namespace Modules.MasterData.DataAccess
{
    public class MasterDataProvider : DataProvider
    {
        public MasterDataProvider(string connectionName = ConnectionEnum.SiteModules)
            : base(connectionName)
        {
        }

        public DataTable LoadTableSetting(int userID)
        {
            Connector.AddParameter(MasterDataTable.UserID, SqlDbType.Int, userID);
            Connector.ExecuteProcedure("dbo.MD_SP_LoadTableSetting", out DataTable result);
            return result;
        }

        public DataTable LoadTableFieldSetting(string tableID)
        {
            Connector.AddParameter(MasterDataTable.TableID, SqlDbType.Int, tableID);
            Connector.ExecuteProcedure("dbo.MD_SP_LoadTableFieldSetting", out DataTable result);
            return result;
        }

        public DataTable LoadTablePermission(int userID, string tableID)
        {
            Connector.AddParameter(MasterDataTable.UserID, SqlDbType.Int, userID);
            Connector.AddParameter(MasterDataTable.TableID, SqlDbType.Int, tableID);
            Connector.ExecuteProcedure("dbo.MD_SP_LoadTablePermission", out DataTable result);
            return result;
        }
        
        public DataTable LoadTableData(string databaseName, string schemaName, string tableName)
        {
            string script = $"select * from {databaseName}.{schemaName}.{tableName} with(nolock)";
            Connector.ExecuteSql(script, out DataTable result);
            return result;
        }

        public DataSet LoadTableInfo(string tableID)
        {
            Connector.AddParameter(MasterDataTable.TableID, SqlDbType.Int, tableID);
            Connector.ExecuteProcedure("dbo.MD_SP_LoadTableInfo", out DataSet result);
            return result;
        }


        private const string ScriptQueryField = @"
            select
                columns.name as FieldName,
                columns.is_identity as IsIdentity,
                Convert(bit, case
					when index_columns.index_column_id is NULL then 0
					else 1
				end) as IsPrimaryKey,
                case
                    when columns.precision > 0 then columns.precision
                    else columns.max_length
                end as MaxLength,
                systypes.name as DataType
	        from
                {0}.sys.objects with(nolock)
	        inner join
                {0}.sys.columns with(nolock)
                    on objects.object_id = columns.object_id
            inner join
                {0}.sys.systypes with(nolock)
                    on systypes.xusertype = columns.user_type_id
            left join
				{0}.sys.index_columns with(nolock)
					on  index_columns.object_id = objects.object_id
					and index_columns.column_id = columns.column_id
	        where
                objects.name = '{1}'
	        and objects.type_desc = 'USER_TABLE';
        ";

        private const string ScriptQueryData = @"
            select
                *
            from
                {0}.{1}.{2} with(nolock)
            where
                1 = 1
            and {3}
        ";

        public DataSet LoadData(string databaseName, string schemaName, string tableName,
            Dictionary<string, string> dictionary)
        {
            Connector.AllowDbNull = true;   // Avoid NULL auto convert to Zero (0)

            // Field Info
            string script = string.Format(ScriptQueryField, databaseName, tableName);

            // Field Value
            if (dictionary != null && dictionary.Keys.Count > 0)
            {
                List<string> list = new List<string>();
                foreach (KeyValuePair<string, string> field in dictionary)
                {
                    list.Add($"{field.Key} = @{field.Key}");
                    Connector.AddParameter(field.Key, SqlDbType.NVarChar, field.Value);
                }
                script = script + string.Format(ScriptQueryData,
                    databaseName, schemaName, tableName, string.Join(" And ", list));
            }
            
            Connector.ExecuteSql(script, out DataSet result);
            return result;
        }

        public DataTable LoadOptionData(string sql)
        {
            Connector.ExecuteSql(sql, out DataTable result);
            return result;
        }


        private const string ScriptInsertData = @"
            insert into {0}.{1}.{2} ({3})
            values ({4});
            select @@identity";

        public int InsertData(string databaseName, string schemaName, string tableName,
            Dictionary<string, string> dataDictionary)
        {
            try
            {
                List<string> listInsert = new List<string>();
                foreach (KeyValuePair<string, string> field in dataDictionary)
                {
                    listInsert.Add($"@{field.Key}");
                    Connector.AddParameter(field.Key, SqlDbType.NVarChar, field.Value);
                }
                string script = string.Format(ScriptInsertData, databaseName, schemaName, tableName,
                    string.Join(", ", dataDictionary.Keys), string.Join(", ", listInsert));
                Connector.ExecuteSql(script, out string value);
                int.TryParse(value, out int result);
                return result;
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
                return -1;
            }
        }


        private const string ScriptUpdateData = @"
            update
                {0}.{1}.{2}
            set
                {3}
            where
                {4}
        ";

        public bool UpdateData(string databaseName, string schemaName, string tableName,
            List<string> listKey, Dictionary<string, string> dataDictionary)
        {
            try
            {
                List<string> listUpdate = new List<string>();
                List<string> listCondition = new List<string>();
                foreach (KeyValuePair<string, string> field in dataDictionary)
                {
                    string data = $"{field.Key} = @{field.Key}";
                    if (listKey.Contains(field.Key))
                    {
                        listCondition.Add(data);
                    }
                    else
                    {
                        listUpdate.Add(data);
                    }

                    Connector.AddParameter(field.Key, SqlDbType.NVarChar, field.Value);
                }

                string script = string.Format(ScriptUpdateData, databaseName, schemaName, tableName,
                    string.Join(", ", listUpdate), string.Join(" And ", listCondition));
                Connector.ExecuteSql(script);
                return true;
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
                return false;
            }
        }


        private const string ScriptDeleteData = @"
            delete
                {0}.{1}.{2}            
            where
                {3}
        ";

        public bool DeleteData(string databaseName, string schemaName, string tableName,
            Dictionary<string, string> dataDictionary)
        {
            try
            {
                List<string> listCondition = new List<string>();
                foreach (KeyValuePair<string, string> field in dataDictionary)
                {
                    string data = $"{field.Key} = @{field.Key}";
                    listCondition.Add(data);
                    Connector.AddParameter(field.Key, SqlDbType.NVarChar, field.Value);
                }

                string script = string.Format(ScriptDeleteData, databaseName, schemaName, tableName,
                    string.Join(" And ", listCondition));
                Connector.ExecuteSql(script);
                return true;
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
                return false;
            }
        }
    }
}