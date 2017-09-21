using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;
using Website.Library.Enum;

namespace Modules.Forex.DataAccess
{
    class CurrencyRateProvider:DataProvider
    {
        #region SQL string
        private static readonly string SQL = @"
                BEGIN
                    BEGIN TRY
                        BEGIN TRANSACTION

                            {0};

                        COMMIT TRANSACTION;
                        SELECT 1, N'{1}' AS MESSAGE;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION
                        SELECT 0, error_message() AS MESSAGE
                    END CATCH
                END";
        #endregion
        private readonly string ScriptAll = $@"Select * from dbo.{CurrencyRateTable.TableName} with(nolock)";
        public List<CurrencyRateData> GetAll()
        {
            Connector.ExecuteSql<CurrencyRateData, List<CurrencyRateData>>(ScriptAll,out List <CurrencyRateData> outList);
            return outList;
        }
        private readonly string Script = $@"Select * from dbo.{CurrencyRateTable.TableName} with(nolock) 
                                    where {CurrencyRateTable.CurrencyCode} = @{CurrencyRateTable.CurrencyCode}";
        public CurrencyRateData GetItem(string key)
        {
            Connector.AddParameter(CurrencyRateTable.CurrencyCode,SqlDbType.VarChar, key);
            Connector.ExecuteSql(Script, out CurrencyRateData data);
            return data;
        }

        public DataTable AddItem(Dictionary<string, SQLParameterData> conditionDictionary)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("StatusID");
            dtResult.Columns.Add("Message");
            DataRow dr = dtResult.NewRow();
            dr["StatusID"] = "-1";
            dr["Message"] = "Chức năng chưa được thực hiện";
            dtResult.Rows.Add(dr);
            dtResult.AcceptChanges();
            return dtResult;
        }
        public DataTable RemoveItem(string key)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("StatusID");
            dtResult.Columns.Add("Message");
            DataRow dr = dtResult.NewRow();
            dr["StatusID"] = "-1";
            dr["Message"] = "Chức năng chưa được thực hiện";
            dtResult.Rows.Add(dr);
            dtResult.AcceptChanges();
            return dtResult;
        }

        public DataTable UpdateItem(string key, Dictionary<string, SQLParameterData> conditionDictionary)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("StatusID");
            dtResult.Columns.Add("Message");
            DataRow dr = dtResult.NewRow();
            dr["StatusID"] = "-1";
            dr["Message"] = "Chức năng chưa được thực hiện";
            dtResult.Rows.Add(dr);
            dtResult.AcceptChanges();
            return dtResult;
        }

        private static readonly string ScriptInsertUpload = $@"
            INSERT INTO [dbo].[FX_CurrencyUploadShadow]
                   ({CurrencyRateTable.CurrencyCode}
                   ,{CurrencyRateTable.Rate}
                   ,{CurrencyRateTable.MasterRate}
                   ,{CurrencyRateTable.MarginMinProfit}
                   ,{CurrencyRateTable.MarginLimit}
                   ,{CurrencyRateTable.IsDisable}
                   ,{CurrencyRateTable.ModifiedDate}
                   ,{CurrencyRateTable.ModifiedUserID})
        ";

        private bool ValidateItemUpload(CurrencyRateData item, out string message)
        {
            if (string.IsNullOrWhiteSpace(item.CurrencyCode))
            {
                message = "Cặp tỷ giá không được bỏ trống";
                return false;
            }
            if (!double.TryParse(item.Rate, out double rate))
            {
                message = $"'Rate ' {item.Rate} không hợp lệ";
                return false;
            }
            if (!double.TryParse(item.MasterRate, out double masterRate))
            {
                message = $"'MasterRate ' {item.MasterRate} không hợp lệ";
                return false;
            }
            if (!double.TryParse(item.MarginMinProfit, out double marginMinProfit))
            {
                message = $"'MarginMinProfit ' {item.MarginMinProfit} không hợp lệ";
                return false;
            }
            if (!double.TryParse(item.MarginLimit, out double marginLimit))
            {
                message = $"'MarginLimit ' {item.MarginLimit} không hợp lệ";
                return false;
            }
            if (string.IsNullOrWhiteSpace(item.IsDisable) ||
                !int.TryParse(item.IsDisable, out int isDisable) ||
                isDisable > 1 || isDisable < 0)
            {
                message = $"'IsDisable ' {item.IsDisable} không hợp lệ";
                return false;
            }
            message = string.Empty;
            return true;
        }
        public bool InsertAndReview(List<CurrencyRateData> listData,int userID,  out string message)
        {
            if (listData == null || listData.Count == 0)
            {
                message = "Không tìm thấy dữ liệu từ file import";
                return false;
            }
            StringBuilder script = new StringBuilder();
            script.Append("TRUNCATE TABLE [dbo].[FX_CurrencyUploadShadow];");
            List<string> listSQL = new List<string>();
            string modifiedDataTime = DateTime.Now.ToString(PatternEnum.DateTime);
            int count = 0;
            foreach (CurrencyRateData item in listData)
            {
                count++;
                if(ValidateItemUpload(item, out message) == false)
                {
                    message = $"{message} dòng {count}";
                    return false;
                }
                listSQL.Add($@"('{item.CurrencyCode}',{item.Rate},{item.MasterRate},
                     {item.MarginMinProfit}, {item.MarginLimit},
                     {item.IsDisable}, {modifiedDataTime},{userID})");

                if (listSQL.Count < 1000)
                {
                    continue;
                }
                script.Append($"{ScriptInsertUpload} values {string.Join(",", listSQL.ToArray())}");
                listSQL = new List<string>();
            }
            script.Append($"{ScriptInsertUpload} values {string.Join(",", listSQL.ToArray())}");
            string executeScript = string.Format(SQL, script,
                "Upload dữ liệu thành công");
            Connector.ExecuteSql(executeScript, out DataTable dtResult);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }

        public DataTable GetDataUpload()
        {
            string script = $@"select {CurrencyRateTable.CurrencyCode} as [Cặp ngoại tệ],
                                    {CurrencyRateTable.Rate} as [BigFigure],
                                    {CurrencyRateTable.MasterRate} as [Tỷ giá trung tâm],
                                    {CurrencyRateTable.MarginMinProfit} as [Biên độ lời tối thiểu],
                                    {CurrencyRateTable.MarginLimit} as [Limit],
                                    {CurrencyRateTable.IsDisable} AS [IsDisable]
                            from [dbo].[FX_CurrencyUploadShadow] with(nolock);";
            Connector.ExecuteSql(script, out DataTable dtResult);
            return dtResult;
        }
        public bool ApprovalChangeCurrency(out string message)
        {
            StringBuilder script = new StringBuilder();
            script.Append($"insert into [dbo].[FX_CurrencyRateHistory] select * from dbo.{CurrencyRateTable.TableName};");
            script.Append($"TRUNCATE TABLE dbo.{CurrencyRateTable.TableName};");
            script.Append(
                $"insert into dbo.{CurrencyRateTable.TableName} select * from [dbo].[FX_CurrencyUploadShadow];");
            script.Append("TRUNCATE TABLE [dbo].[FX_CurrencyUploadShadow];");
            string executeScript = string.Format(SQL, script,
                "Duyệt, cập nhật giá thành công");
            Connector.ExecuteSql(executeScript, out DataTable dtResult);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
    }
}
