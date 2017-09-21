using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Modules.Forex.Enum;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace Modules.Forex.DataAccess
{
    class ExchangeRateProvider:DataProvider
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

        private readonly string ScrtipAll = $@"Select * from dbo.{ExchangeRateTable.TableName} with(nolock)";
        public List<ExchangeRateData> GetAll()
        {
            Connector.ExecuteSql<ExchangeRateData, List<ExchangeRateData>>(ScrtipAll, out List<ExchangeRateData> outList);
            return outList;
        }
        public List<ExchangeRateGridData> GetGridData()
        {
            Connector.AddParameter(ExchangeRateGridFieldEnum.CurrencyCode, SqlDbType.VarChar, "-1");
            Connector.ExecuteProcedure<ExchangeRateGridData, List<ExchangeRateGridData>>("FX_GetExchangeRateLastest", out List<ExchangeRateGridData> outList);
            return outList;
        }

        public ExchangeRateGridData GetGridItem(string key)
        {
            Connector.AddParameter(ExchangeRateGridFieldEnum.CurrencyCode,SqlDbType.VarChar, key);
            Connector.ExecuteProcedure("dbo.FX_GetExchangeRateLastest", out ExchangeRateGridData outData);
            return outData;
        }

        public DataTable AddItem(Dictionary<string, SQLParameterData> conditionDictionary)
        {
            DataTable dtResult = GetDataTableMessage;
            DataRow dr = dtResult.NewRow();
            dr["StatusID"] = "-1";
            dr["Message"] = "Chức năng chưa được thực hiện";
            dtResult.Rows.Add(dr);
            dtResult.AcceptChanges();
            return dtResult;
        }
        public DataTable RemoveItem(string key)
        {
            DataTable dtResult = GetDataTableMessage;
            DataRow dr = dtResult.NewRow();
            dr["StatusID"] = "-1";
            dr["Message"] = "Chức năng chưa được thực hiện";
            dtResult.Rows.Add(dr);
            dtResult.AcceptChanges();
            return dtResult;
        }

        private static string GetUpdateRateScript(string rate, string dealTime, string currencyCode,
            string transactionTypeID, string remark, string isDisable, string modifiedUserID, string modifiedDateTime)
        {

            return $"Insert into dbo.FX_ExchangeRateHistory " +
                                $"({ExchangeRateTable.CurrencyCode},{ExchangeRateTable.TransactionTypeID}," +
                                $"{ExchangeRateTable.Rate},{ExchangeRateTable.DealTime}," +
                                $"PreviousModifiedDateTime,PreviousModifedUserID,{ExchangeRateTable.Remark}, " +
                                $"HistoryDateTime,{ExchangeRateTable.IsDisable}) " +
                            $"Select " +
                                $"{ExchangeRateTable.CurrencyCode},{ExchangeRateTable.TransactionTypeID}," +
                                $"{ExchangeRateTable.Rate},{ExchangeRateTable.DealTime}," +
                                $"{ExchangeRateTable.ModifiedDateTime} AS PreviousModifiedDateTime," +
                                $"{ExchangeRateTable.ModifedUserID} AS PreviousModifedUserID," +
                                $"{ExchangeRateTable.Remark}, {modifiedDateTime} AS HistoryDateTimeFrom, " +
                                $"{ExchangeRateTable.IsDisable} " +
                            $"FROM  dbo.{ExchangeRateTable.TableName} " +
                            $"WHERE {ExchangeRateTable.CurrencyCode} = '{currencyCode}' AND {ExchangeRateTable.TransactionTypeID} ={transactionTypeID};" +
                            $"Update dbo.{ExchangeRateTable.TableName} " +
                                $"SET {ExchangeRateTable.Rate} = {rate}," +
                                    $"{ExchangeRateTable.PreviousRate} = (CASE WHEN {ExchangeRateTable.Rate} != {rate} THEN {ExchangeRateTable.Rate} ELSE {ExchangeRateTable.PreviousRate} END)," +
                                    $"{ExchangeRateTable.DealTime} = {dealTime}," +
                                    $"{ExchangeRateTable.ModifedUserID} = {modifiedUserID}," +
                                    $"{ExchangeRateTable.ModifiedDateTime} = {modifiedDateTime}, " +
                                    $"{ExchangeRateTable.Remark} = N'{remark}', " +
                                    $"{ExchangeRateTable.IsDisable} = '{isDisable}' " +
                                $"WHERE {ExchangeRateTable.CurrencyCode} = '{currencyCode}' AND {ExchangeRateTable.TransactionTypeID} ={transactionTypeID};";
        }
        public DataTable UpdateExchangeRate(ExchangeRateGridData dataUpdate, ExchangeRateGridData currentData,
            string remark,string modifiedUserID, string modifiedDateTime)
        {
            List<string> scriptList = new List<string>();

            if (!dataUpdate.BuyRateFT.Equals(currentData.BuyRateFT) ||
                !dataUpdate.DealTimeBuyFT.Equals(currentData.DealTimeBuyFT) ||
                !dataUpdate.IsDisableBuyFT.Equals(currentData.IsDisableBuyFT))
            {
                scriptList.Add(GetUpdateRateScript(dataUpdate.BuyRateFT,dataUpdate.DealTimeBuyFT,
                    dataUpdate.CurrencyCode,TransactionTypeEnum.BuyByFundTranfer.ToString(),
                    remark,dataUpdate.IsDisableBuyFT, modifiedUserID, modifiedDateTime));
            }
            if (!dataUpdate.SellRateFT.Equals(currentData.SellRateFT) ||
                !dataUpdate.DealTimeSellFT.Equals(currentData.DealTimeSellFT) ||
                !dataUpdate.IsDisableSellFT.Equals(currentData.IsDisableSellFT))
            {
                scriptList.Add(GetUpdateRateScript(dataUpdate.SellRateFT, dataUpdate.DealTimeSellFT,
                    dataUpdate.CurrencyCode, TransactionTypeEnum.SellByFundTranfer.ToString(),
                    remark, dataUpdate.IsDisableSellFT, modifiedUserID, modifiedDateTime));
            }
            if (!dataUpdate.BuyRateCash.Equals(currentData.BuyRateCash) ||
                !dataUpdate.DealTimeBuyCash.Equals(currentData.DealTimeBuyCash) ||
                !dataUpdate.IsDisableBuyCash.Equals(currentData.IsDisableBuyCash))
            {
                scriptList.Add(GetUpdateRateScript(dataUpdate.BuyRateCash, dataUpdate.DealTimeBuyCash,
                    dataUpdate.CurrencyCode, TransactionTypeEnum.BuyByCash.ToString(),
                    remark, dataUpdate.IsDisableBuyCash, modifiedUserID, modifiedDateTime));
            }
            if (!dataUpdate.SellRateCash.Equals(currentData.SellRateCash) ||
                !dataUpdate.DealTimeSellCash.Equals(currentData.DealTimeSellCash) ||
                !dataUpdate.IsDisableSellCash.Equals(currentData.IsDisableSellCash))
            {
                scriptList.Add(GetUpdateRateScript(dataUpdate.SellRateCash, dataUpdate.DealTimeSellCash,
                    dataUpdate.CurrencyCode, TransactionTypeEnum.SellByCash.ToString(),
                    remark, dataUpdate.IsDisableSellCash, modifiedUserID, modifiedDateTime));
            }
            DataTable dtResult;

            if (scriptList.Count > 0)
            {
                try
                {
                    string script = string.Format(SQL, string.Join(" ", scriptList.ToArray()),
                        "Cập nhật giá thành công");

                    Connector.ExecuteSql(script, out dtResult);
                }
                catch (Exception e)
                {
                    dtResult = GetDataTableMessage;
                    DataRow dr = dtResult.NewRow();
                    dr["StatusID"] = "-1";
                    dr["Message"] = $"Cập nhật dữ liệu xảy ra lỗi \n {e}";
                    dtResult.Rows.Add(dr);
                    dtResult.AcceptChanges();
                }
            }
            else
            {
                dtResult = GetDataTableMessage;
                DataRow dr = dtResult.NewRow();
                dr["StatusID"] = "1";
                dr["Message"] = "Không có dữ liệu thay đổi để tiến hành quá trình cập nhật";
                dtResult.Rows.Add(dr);
                dtResult.AcceptChanges();
            }
            
            return dtResult;
        }

        private DataTable GetDataTableMessage
        {
            get
            {
                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("StatusID");
                dtResult.Columns.Add("Message");
                return dtResult;
            }
        }

        private static readonly string ScriptInsertUpload = $@"
            INSERT INTO [dbo].[FX_ExchangeRateUploadShadow]
                   ({ExchangeRateTable.CurrencyCode}
                   ,{ExchangeRateTable.TransactionTypeID}
                   ,{ExchangeRateTable.Rate}
                   ,{ExchangeRateTable.DealTime}
                   ,{ExchangeRateTable.IsDisable}
                   ,{ExchangeRateTable.ModifiedDateTime}
                   ,{ExchangeRateTable.ModifedUserID})
        ";

        private bool ValidateItemUpload(ExchangeRateData item, out string message)
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
            if (!int.TryParse(item.DealTime, out int dealtime))
            {
                message = $"'DealTime ' {item.DealTime} không hợp lệ";
                return false;
            }
            if (!int.TryParse(item.TransactionTypeID, out int tranType) ||
                tranType > 4  || tranType < 1)
            {
                message = $"'Loại giao dịch (TransactionTypeID) ' {item.TransactionTypeID} không hợp lệ, " +
                    "TransactionTypeID phải có giá trị 1, 2, 3 hoặc 4";
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

        private ExchangeRateData GetExchangeRateDataByImport(List<ExchangeRateData> listData, 
            string currencyCode, string transactionTypeID, out int rownumber)
        {
            int count = 0;
            foreach (ExchangeRateData item in listData)
            {
                count++;
                if (item.CurrencyCode.Equals(currencyCode) && item.TransactionTypeID.Equals(transactionTypeID))
                {
                    rownumber = count;
                    return item;
                }
            }
            rownumber = 0;
            return null;
        }
        public bool InsertAndReview(List<ExchangeRateData> listData, int userID, out string message)
        {
            if (listData == null || listData.Count == 0)
            {
                message = "Không tìm thấy dữ liệu từ file import";
                return false;
            }
            List<CurrencyRateData> listCurrency = CacheBase.Receive<CurrencyRateData>();
            if (listCurrency == null || listCurrency.Count == 0)
            {
                message = "Không tim thấy dữ liệu giá ngoại tệ, vui lòng thực hiện cập nhật giá ngoại tệ trước giá mua bán sau.";
                return false;
            }
            List<TransactionTypeData> listTransactionType = CacheBase.Receive<TransactionTypeData>();
            if (listTransactionType == null || listTransactionType.Count == 0)
            {
                message = "Không tim thấy dữ liệu loại giao dịch, vui lòng liên hệ người quản trị.";
                return false;
            }
            if (listData.Count > listCurrency.Count * listTransactionType.Count)
            {
                message = $"Dữ liệu import chưa chính xác, số lượng giá ngoại tệ {listData.Count} > {listCurrency.Count} x {listTransactionType.Count}, " +
                    "Vui lòng kiểm tra lại dữ liệu import, hoặc kiểm tra thứ tự cần cập nhật giá ngoại tệ trước, giá mua bán sau.";
                return false;
            }
            StringBuilder script = new StringBuilder();
            script.Append("TRUNCATE TABLE [dbo].[FX_ExchangeRateUploadShadow];");
            List<string> listSQL = new List<string>();
            string modifiedDataTime = DateTime.Now.ToString(PatternEnum.DateTime);

            foreach (CurrencyRateData currency in listCurrency)
            {
                foreach (TransactionTypeData transactionType in listTransactionType)
                {
                    ExchangeRateData exchange = GetExchangeRateDataByImport(listData, currency.CurrencyCode,
                        transactionType.ID, out int rownumber);
                    if (exchange != null)
                    {
                        if (ValidateItemUpload(exchange, out message) == false)
                        {
                            message = $"{message} dòng {rownumber}";
                            return false;
                        }
                        listSQL.Add($@"('{currency.CurrencyCode}',{transactionType.ID},{exchange.Rate},
                            {exchange.DealTime},{exchange.IsDisable}, {modifiedDataTime},{userID})");
                    }
                    else
                    {
                        listSQL.Add($@"('{currency.CurrencyCode}',{transactionType.ID},0,
                            0,1, {modifiedDataTime},{userID})");
                    }
                    if (listSQL.Count < 1000)
                    {
                        continue;
                    }
                    script.Append($"{ScriptInsertUpload} values {string.Join(",", listSQL.ToArray())}");
                    listSQL = new List<string>();
                }
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
            string script = $@"select {ExchangeRateTable.CurrencyCode} as [Cặp ngoại tệ],
                                    {ExchangeRateTable.TransactionTypeID} as [Mã loại giao dịch],
                                    {ExchangeRateTable.Rate} as [Bid ask Pips],
                                    {ExchangeRateTable.DealTime} as [Thời gian],
                                    {ExchangeRateTable.IsDisable} AS [IsDisable]
                            from [dbo].[FX_ExchangeRateUploadShadow] with(nolock);";
            Connector.ExecuteSql(script, out DataTable dtResult);
            return dtResult;
        }
        public bool ApprovalChangeCurrency(out string message)
        {
            string modifiedDataTime = DateTime.Now.ToString(PatternEnum.DateTime);
            Connector.AddParameter(ExchangeRateTable.ModifiedDateTime,SqlDbType.BigInt, modifiedDataTime);
            Connector.ExecuteProcedure("FX_UploadExchangeRate", out DataTable dtResult);
            message = dtResult?.Rows[0][1]?.ToString();
            return dtResult?.Rows[0][0]?.ToString() == "1";
        }
    }
}
