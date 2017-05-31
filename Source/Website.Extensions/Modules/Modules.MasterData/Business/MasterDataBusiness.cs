using System;
using System.Collections.Generic;
using System.Data;
using Modules.MasterData.DataAccess;
using Modules.MasterData.Database;
using Website.Library.Global;

namespace Modules.MasterData.Business
{
    public static class MasterDataBusiness
    {
        public static DataTable LoadTableSetting(int userID)
        {
            return new MasterDataProvider().LoadTableSetting(userID);
        }

        public static DataTable LoadTablePermission(int userID, string tableID)
        {
            return new MasterDataProvider().LoadTablePermission(userID, tableID);
        }

        public static DataSet LoadTableData(int userID, string tableID, string connectionName,
            string databaseName, string schemaName, string tableName)
        {
            DataSet dsResult = new DataSet();
            DataTable tablePermission = LoadTablePermission(userID, tableID);
            if (tablePermission.Rows.Count == 0)
            {
                return dsResult;
            }

            dsResult.Tables.Add(tablePermission);
            dsResult.Tables.Add(LoadTableData(connectionName, databaseName, schemaName, tableName));
            return dsResult;
        }

        public static DataTable LoadTableData(
            string connectionName, string databaseName, string schemaName, string tableName)
        {
            return new MasterDataProvider(connectionName).LoadTableData(databaseName, schemaName, tableName);
        }

        public static DataSet LoadRowData(string tableID, Dictionary<string, string> parameterDictionary)
        {
            // Query configuration data (connection, database, fields, ...)
            MasterDataProvider provider = new MasterDataProvider();
            DataSet dsResult = provider.LoadTableInfo(tableID);
            if (dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            // Load row data
            DataRow row = dsResult.Tables[0].Rows[0];
            string connectionName = row[MasterDataTable.ConnectionName].ToString();
            string databaseName = row[MasterDataTable.DatabaseName].ToString();
            string schemaName = row[MasterDataTable.SchemaName].ToString();
            string tableName = row[MasterDataTable.TableName].ToString();

            DataSet dataSet = new MasterDataProvider(connectionName)
                .LoadData(databaseName, schemaName, tableName, parameterDictionary);
            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                DataTable dataTable = dataSet.Tables[i].Copy();
                dataTable.TableName = $"Table{i + 2}";
                dsResult.Tables.Add(dataTable);
            }
            dataSet.Dispose();
            return dsResult;
        }

        public static DataTable LoadOptionData(string sql)
        {
            try
            {
                return new MasterDataProvider().LoadOptionData(sql);
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
                return new DataTable();
            }
        }

        public static int InsertData(string connectionName, string databaseName, string schemaName, string tableName,
            Dictionary<string, string> dataDictionary)
        {
            return new MasterDataProvider(connectionName).InsertData(databaseName, schemaName, tableName,
                dataDictionary);
        }

        public static bool UpdateData(string connectionName, string databaseName, string schemaName, string tableName,
            List<string> listKey, Dictionary<string, string> dataDictionary)
        {
            return new MasterDataProvider(connectionName).UpdateData(databaseName, schemaName, tableName,
                listKey, dataDictionary);
        }

        public static bool DeleteData(string connectionName, string databaseName, string schemaName, string tableName,
            Dictionary<string, string> dataDictionary)
        {
            return new MasterDataProvider(connectionName).DeleteData(databaseName, schemaName, tableName,
                dataDictionary);
        }
    }
}