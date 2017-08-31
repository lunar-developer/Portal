using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Modules.Application.Business;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;

namespace Modules.Application.DataAccess
{
    public class ApplicationProvider : DataProvider
    {
        private static readonly string ScriptInsert;
        private static readonly string ScriptUpdate;
        private static readonly string ScriptAutoAssign;
        private static readonly string ScriptUpdatePhase;
        private static readonly string ScriptInsertProcess;
        private static readonly string ScriptInsertLog;
        private static readonly string ScriptInsertScheduleLog;


        static ApplicationProvider()
        {
            int index = 0;

            #region SCRIPT INSERT

            ScriptInsert = $@"
                declare @Step varchar(50) = 'Begin Insert'
                declare @{ApplicationTable.ApplicationID} bigint
                declare @{ApplicationLogTable.ApplicationLogID} bigint

                begin transaction
                begin try
                    set @Step = 'Step 1: Insert Application'
                    insert into dbo.{ApplicationTable.TableName} ({{{index++}}}) values ({{{index++}}})
                    set @{ApplicationTable.ApplicationID} = @@identity

                    
                    set @Step = 'Step 2: Insert Field Integer'
                    if({{{index++}}} > 0)
                        begin
                            insert into dbo.{ApplicationFieldTable.TableInteger} (
                                {ApplicationTable.ApplicationID},
                                {ApplicationFieldTable.FieldName},
                                {ApplicationFieldTable.FieldValue})
                            values {{{index++}}}
                        end


                    set @Step = 'Step 3: Insert Field BigInteger'
                    if({{{index++}}} > 0)
                        begin
                            insert into dbo.{ApplicationFieldTable.TableBigInteger} (
                                {ApplicationTable.ApplicationID},
                                {ApplicationFieldTable.FieldName},
                                {ApplicationFieldTable.FieldValue})
                            values {{{index++}}}
                        end


                    set @Step = 'Step 4: Insert Field String'
                    if({{{index++}}} > 0)
                        begin
                            insert into dbo.{ApplicationFieldTable.TableString} (
                                {ApplicationTable.ApplicationID},
                                {ApplicationFieldTable.FieldName},
                                {ApplicationFieldTable.FieldValue})
                            values {{{index++}}}
                        end


                    set @Step = 'Step 5: Insert Field Unicode String'
                    if({{{index++}}} > 0)
                        begin
                            insert into dbo.{ApplicationFieldTable.TableUnicodeString} (
                                {ApplicationTable.ApplicationID},
                                {ApplicationFieldTable.FieldName},
                                {ApplicationFieldTable.FieldValue})
                            values {{{index++}}}
                        end


                    set @Step = 'Step 6: Insert Log'
                    insert into dbo.{ApplicationLogTable.TableName} (
                        {ApplicationTable.ApplicationID}, 
                        {ApplicationLogTable.LogAction},
                        {ApplicationLogTable.Remark},
                        {ApplicationLogTable.IsHasLogDetail},
                        {ApplicationLogTable.IsSensitiveInfo},
                        {BaseTable.ModifyUserID},
                        {BaseTable.ModifyDateTime}) 
                    values (
                        @{ApplicationTable.ApplicationID},
                        N'Thêm mới',
                        '',
                        0,
                        0,
                        @{BaseTable.ModifyUserID},
                        @{BaseTable.ModifyDateTime})
                    set @{ApplicationLogTable.ApplicationLogID} = @@identity


                    set @Step = 'Step 7: Insert Log Detail'
                    insert into dbo.{ApplicationLogDetailTable.TableName} (
                        {ApplicationLogTable.ApplicationLogID}, {ApplicationTable.ApplicationID},
                        {ApplicationFieldTable.FieldName}, {ApplicationFieldTable.FieldValue})
                    values {{{index}}}


                    set @Step = 'Step 8: Insert Process'
                    insert into dbo.{ApplicationProcessTable.TableName} (
                        {ApplicationTable.ApplicationID}, {ApplicationTable.ProcessID}, {ApplicationTable.PhaseID},
                        {ApplicationProcessTable.PreviousPhaseID}, {ApplicationProcessTable.Remark},
                        {ApplicationTable.ProcessUserID}, {ApplicationTable.ProcessDateTime})
                    select
                        {ApplicationTable.ApplicationID}, {ApplicationTable.ProcessID}, {ApplicationTable.PhaseID},
                        0, '',
                        @{BaseTable.ModifyUserID}, @{BaseTable.ModifyDateTime}
                    from
                        dbo.{ApplicationTable.TableName} with(nolock)               
                    where
                        {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}


                    commit transaction
                    select @{ApplicationTable.ApplicationID}
                end try
                begin catch
                    rollback transaction

                    insert into dbo.SYS_Exception(ErrorCode, ErrorMessage, StackTrace, CreateDateTime)
                    select 
                        Error_Number(), 
                        Error_Message(), 
                        'ApplicationProvider.InsertApplication - ' + @Step,
                        @{BaseTable.ModifyDateTime}

                    select -1
                end catch";

            #endregion

            #region SCRIPT UPDATE

            index = 0;
            ScriptUpdate = $@"
                declare @Step varchar(50) = 'Begin Update'
                declare @{ApplicationLogTable.ApplicationLogID} bigint

                begin transaction
                begin try                
                    set @Step = 'Step 1: Update Application'
                    update dbo.{ApplicationTable.TableName}
                    set {{{index++}}}
                    where {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}

                    
                    set @Step = 'Step 2: Insert Field Integer'
                    if({{{index++}}} > 0)
                        begin
                            delete from dbo.{ApplicationFieldTable.TableInteger}
                            where {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}

                            insert into dbo.{ApplicationFieldTable.TableInteger} (
                                {ApplicationTable.ApplicationID},
                                {ApplicationFieldTable.FieldName},
                                {ApplicationFieldTable.FieldValue})
                            values {{{index++}}}
                        end


                    set @Step = 'Step 3: Insert Field BigInteger'
                    if({{{index++}}} > 0)
                        begin
                            delete from dbo.{ApplicationFieldTable.TableBigInteger}
                            where {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}

                            insert into dbo.{ApplicationFieldTable.TableBigInteger} (
                                {ApplicationTable.ApplicationID}, 
                                {ApplicationFieldTable.FieldName},
                                {ApplicationFieldTable.FieldValue})
                            values {{{index++}}}
                        end


                    set @Step = 'Step 4: Insert Field String'
                    if({{{index++}}} > 0)
                        begin
                            delete from dbo.{ApplicationFieldTable.TableString}
                            where {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}

                            insert into dbo.{ApplicationFieldTable.TableString} (
                                {ApplicationTable.ApplicationID},
                                {ApplicationFieldTable.FieldName},
                                {ApplicationFieldTable.FieldValue})
                            values {{{index++}}}
                        end


                    set @Step = 'Step 5: Insert Field Unicode String'
                    if({{{index++}}} > 0)
                        begin
                            delete from dbo.{ApplicationFieldTable.TableUnicodeString}
                            where {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}

                            insert into dbo.{ApplicationFieldTable.TableUnicodeString} (
                                {ApplicationTable.ApplicationID},
                                {ApplicationFieldTable.FieldName},
                                {ApplicationFieldTable.FieldValue})
                            values {{{index++}}}
                        end


                    set @Step = 'Step 6: Insert Log';
                    insert into dbo.{ApplicationLogTable.TableName} (
                        {ApplicationTable.ApplicationID},
                        {ApplicationLogTable.LogAction},
                        {ApplicationLogTable.Remark},
                        {ApplicationLogTable.IsHasLogDetail},
                        {ApplicationLogTable.IsSensitiveInfo},
                        {BaseTable.ModifyUserID},
                        {BaseTable.ModifyDateTime}) 
                    values (
                        @{ApplicationTable.ApplicationID},
                        N'Cập nhật',
                        '',
                        1,
                        0,
                        @{BaseTable.ModifyUserID},
                        @{BaseTable.ModifyDateTime})
                    set @ApplicationLogID = @@identity


                    set @Step = 'Step 7: Insert Log Detail'
                    insert into dbo.{ApplicationLogDetailTable.TableName} (
                        {ApplicationLogTable.ApplicationLogID},
                        {ApplicationTable.ApplicationID},
                        {ApplicationFieldTable.FieldName},
                        {ApplicationFieldTable.FieldValue})
                    values {{{index}}}


                    commit transaction;
                    select @{ApplicationTable.ApplicationID}
                end try
                begin catch
                    rollback transaction

                    insert into dbo.SYS_Exception(ErrorCode, ErrorMessage, StackTrace, CreateDateTime)
                    select Error_Number(), Error_Message(), 'ApplicationProvider.InsertApplication', @{BaseTable.ModifyDateTime}

                    select -1
                end catch";

            #endregion

            #region SCRIPT AUTO ASSIGN

            index = 0;
            ScriptAutoAssign = $@"
                declare @Step as varchar(250) = 'Begin Auto Assign'
                declare @{ApplicationProcessTable.Remark} as nvarchar(250) = N'Hệ thống tự động phân hồ sơ cho '

                begin transaction
                begin try
                    set @Step = 'Step 01 - Update Application Status'
                    {{{index++}}}

                    set @Step = 'Step 02 - Insert Application Process'
                    {{{index++}}}

                    set @Step = 'Step 03 - Insert Application Log'
                    {{{index++}}}

                    set @Step = 'Step 04 - Insert Schedule Log'
                    {{{index}}}

                    commit transaction
                    select 1
                end try
                begin catch
                    rollback transaction
                    
                    insert into dbo.SYS_Exception(ErrorCode, ErrorMessage, StackTrace, CreateDateTime)
                    select 
                        Error_Number(), 
                        Error_Message(), 
                        'ApplicationProvider.AutoAssign - ' + @Step,
                        @{ApplicationTable.ModifyDateTime}

                    insert into dbo.{ScheduleLogTable.TableName}
			        (
                        {ScheduleLogTable.ScheduleCode},
                        {ScheduleLogTable.LogDate},
                        {ScheduleLogTable.LogMessage},
                        {ScheduleLogTable.IsSuccess},
                        {ScheduleLogTable.CreateDateTime}
                    )
		            values
			            ('AUTO_ASSIGN', @CurrentDate, N'Có lỗi khi xử lý.', 0, @{ApplicationTable.ModifyDateTime})

                    select -1
                end catch
            ";


            index = 0;
            ScriptUpdatePhase = $@"
                update
			        dbo.{ApplicationTable.TableName}
		        set
			        {ApplicationTable.PhaseID} = {{{index++}}},
			        {ApplicationTable.ApplicationStatus} = {{{index++}}},
			        {ApplicationTable.CurrentUserID} = {{{index++}}},
			        {ApplicationTable.ApplicationRemark} = '',
			        {ApplicationTable.ModifyUserID} = @{ApplicationTable.ModifyUserID},
			        {ApplicationTable.ModifyDateTime} = @{ApplicationTable.ModifyDateTime}
		        where
			        {ApplicationTable.ApplicationID} in ({{{index++}}})
		        and {ApplicationTable.PhaseID} = {{{index}}}
            ";


            index = 0;
            ScriptInsertProcess = $@"
                insert into dbo.{ApplicationProcessTable.TableName}
			    (
                    {ApplicationTable.ApplicationID}, 
                    {ApplicationTable.ProcessID},
                    {ApplicationTable.PhaseID},
                    {ApplicationProcessTable.PreviousPhaseID},
                    {ApplicationProcessTable.Remark},
                    {ApplicationTable.ProcessUserID},
                    {ApplicationTable.ProcessDateTime}
                )
		        select
			        {ApplicationTable.ApplicationID},
                    {ApplicationTable.ProcessID},
                    {ApplicationTable.PhaseID},
                    {{{index++}}},
                    @{ApplicationProcessTable.Remark} + N'{{{index++}}}',
                    @{ApplicationTable.ModifyUserID},
                    @{ApplicationTable.ModifyDateTime}
                from
                    dbo.{ApplicationTable.TableName} with(nolock)
                where
                    {ApplicationTable.ApplicationID} in ({{{index++}}})
                and {ApplicationTable.PhaseID} = {{{index}}}
            ";


            index = 0;
            ScriptInsertLog = $@"
                insert into dbo.{ApplicationLogTable.TableName}
			    (
                    {ApplicationTable.ApplicationID},
                    {ApplicationLogTable.LogAction},
                    {ApplicationLogTable.Remark},
                    {ApplicationLogTable.IsHasLogDetail},
                    {ApplicationLogTable.IsSensitiveInfo},
                    {ApplicationTable.ModifyUserID},
                    {ApplicationTable.ModifyDateTime}
                )
		        select
                    {ApplicationTable.ApplicationID},
                    N'{{{index++}}}',
                    @{ApplicationLogTable.Remark} + N'{{{index++}}}',
                    0,
                    0,
                    @{ApplicationTable.ModifyUserID},
                    @{ApplicationTable.ModifyDateTime}
                from
                    dbo.{ApplicationTable.TableName} with(nolock)
                where
                    {ApplicationTable.ApplicationID} in ({{{index++}}})
                and {ApplicationTable.PhaseID} = {{{index}}}
            ";


            ScriptInsertScheduleLog = $@"
                insert into dbo.{ScheduleLogTable.TableName}
			    (
                    {ScheduleLogTable.ScheduleCode},
                    {ScheduleLogTable.LogDate},
                    {ScheduleLogTable.LogMessage},
                    {ScheduleLogTable.IsSuccess},
                    {ScheduleLogTable.CreateDateTime}
                )
		        values
			    (
                    'AUTO_ASSIGN',
                    @CurrentDate,
                    N'{{0}}',
                    1,
                    @{ApplicationTable.ModifyDateTime}
                )
            ";

            #endregion
        }


        public long InsertApplication(
            int userID,
            ICollection<FieldData> listApplication,
            ICollection<FieldData> listInteger,
            ICollection<FieldData> listBigInteger,
            ICollection<FieldData> listString,
            ICollection<FieldData> listUnicodeString,
            ICollection<FieldData> listLog)
        {
            List<string> listField = new List<string>();
            List<string> listValue = new List<string>();
            foreach (FieldData fieldInfo in listApplication)
            {
                listField.Add(fieldInfo.FieldName);
                listValue.Add(fieldInfo.FieldValue);
            }

            string script = string.Format(ScriptInsert,
                string.Join(CharacterEnum.Comma, listField), string.Join(CharacterEnum.Comma, listValue),
                listInteger.Count, BuildScript(listInteger),
                listBigInteger.Count, BuildScript(listBigInteger),
                listString.Count, BuildScript(listString),
                listUnicodeString.Count, BuildScript(listUnicodeString),
                BuildLogScript(listLog));

            Connector.AddParameter(BaseTable.ModifyUserID, SqlDbType.Int, userID);
            Connector.AddParameter(BaseTable.ModifyDateTime, SqlDbType.BigInt,
                DateTime.Now.ToString(PatternEnum.DateTime));
            Connector.ExecuteSql(script, out string result);
            return long.Parse(result);
        }

        public long UpdateApplication(
            int userID,
            string applicationID,
            ICollection<FieldData> listApplication,
            ICollection<FieldData> listInteger,
            ICollection<FieldData> listBigInteger,
            ICollection<FieldData> listString,
            ICollection<FieldData> listUnicodeString,
            ICollection<FieldData> listLog)
        {
            List<string> listField = new List<string>();
            foreach (FieldData fieldInfo in listApplication)
            {
                listField.Add($"{fieldInfo.FieldName} = {fieldInfo.FieldValue}");
            }

            string script = string.Format(ScriptUpdate,
                string.Join(CharacterEnum.Comma, listField),
                listInteger.Count, BuildScript(listInteger),
                listBigInteger.Count, BuildScript(listBigInteger),
                listString.Count, BuildScript(listString),
                listUnicodeString.Count, BuildScript(listUnicodeString),
                BuildLogScript(listLog));

            Connector.AddParameter(ApplicationTable.ApplicationID, SqlDbType.BigInt, applicationID);
            Connector.AddParameter(BaseTable.ModifyUserID, SqlDbType.Int, userID);
            Connector.AddParameter(BaseTable.ModifyDateTime, SqlDbType.BigInt,
                DateTime.Now.ToString(PatternEnum.DateTime));
            Connector.ExecuteSql(script, out string result);
            return long.Parse(result);
        }

        public DataSet LoadApplication(string applicationID, int userID)
        {
            Connector.AddParameter(ApplicationTable.ApplicationID, SqlDbType.BigInt, applicationID);
            Connector.AddParameter(BaseTable.UserID, SqlDbType.Int, userID);
            Connector.ExecuteProcedure("dbo.APP_SP_LoadApplication", out DataSet result);
            return result;
        }

        public DataTable SearchApplication(Dictionary<string, SQLParameterData> conditionDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in conditionDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.APP_SP_SearchApplication", out DataTable result);
            return result;
        }

        public long ProcessApplication(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.APP_SP_ProcessApplication", out string result);
            return long.Parse(result);
        }

        public DataSet GetAutoAssignData()
        {
            Connector.ExecuteProcedure("dbo.APP_SP_GetAutoAssignData", out DataSet result);
            return result;
        }

        public bool AutoAssign(int processUserID,
            List<AutoAssignData> listAssignData)
        {
            StringBuilder updateScript = new StringBuilder();
            StringBuilder insertProcessScript = new StringBuilder();
            StringBuilder insertLogScript = new StringBuilder();
            StringBuilder insertScheduleLog = new StringBuilder();
            foreach (AutoAssignData item in listAssignData)
            {
                List<UserAssignData> listUserAssignData = item.ListUserAssignData;
                string currentPhaseID = item.CurrentPhaseID;
                string targetPhaseID = item.TargetPhaseID;
                string applicationStatus = PhaseBussiness.GetPhaseStatus(targetPhaseID);
                const string logAction = "Phân hồ sơ";
                string logMessage = item.LogMessage;
                foreach (UserAssignData userAssignData in listUserAssignData)
                {
                    string userID = userAssignData.UserID;
                    string userName = userAssignData.UserName;


                    updateScript.AppendLine(string.Format(ScriptUpdatePhase,
                        targetPhaseID, applicationStatus, userID, userAssignData.AssignData, currentPhaseID));
                    insertProcessScript.AppendLine(string.Format(ScriptInsertProcess,
                        currentPhaseID, userName, userAssignData.AssignData, targetPhaseID));
                    insertLogScript.AppendLine(string.Format(ScriptInsertLog,
                        logAction, userName, userAssignData.AssignData, targetPhaseID));
                }
                insertScheduleLog.AppendLine(string.Format(ScriptInsertScheduleLog, logMessage));
            }

            string script = string.Format(ScriptAutoAssign,
                updateScript, insertProcessScript, insertLogScript, insertScheduleLog);

            Connector.AddParameter("CurrentDate", SqlDbType.Int, DateTime.Now.ToString(PatternEnum.Date));
            Connector.AddParameter(ApplicationTable.ModifyUserID, SqlDbType.Int, processUserID);
            Connector.AddParameter(
                ApplicationTable.ModifyDateTime, SqlDbType.BigInt, DateTime.Now.ToString(PatternEnum.DateTime));
            Connector.ExecuteSql(script, out string result);
            return result == "1";
        }


        public DataTable GetVSaleKitApplication(string uniqueID)
        {
            Connector.AddParameter(ApplicationTable.UniqueID, SqlDbType.VarChar, uniqueID);
            Connector.ExecuteProcedure("dbo.APP_SP_GetVSaleKitApplication", out DataTable result);
            return result;
        }


        private static string BuildScript(ICollection<FieldData> listField)
        {
            if (listField.Count <= 0)
            {
                return $"(@{ApplicationTable.ApplicationID}, NULL, NULL)";
            }

            List<string> listData = new List<string>();
            foreach (FieldData fieldInfo in listField)
            {
                listData.Add($"(@{ApplicationTable.ApplicationID}, '{fieldInfo.FieldName}', {fieldInfo.FieldValue})");
            }
            return string.Join(CharacterEnum.Comma, listData);
        }

        private static string BuildLogScript(ICollection<FieldData> listField)
        {
            if (listField.Count <= 0)
            {
                return $"(@{ApplicationLogTable.ApplicationLogID}, @{ApplicationTable.ApplicationID}, NULL, NULL)";
            }

            List<string> listData = new List<string>();
            foreach (FieldData fieldInfo in listField)
            {
                listData.Add($@"(
                    @{ApplicationLogTable.ApplicationLogID},
                    @{ApplicationTable.ApplicationID},
                    '{fieldInfo.FieldName}',
                    {fieldInfo.FieldValue})");
            }
            return string.Join(CharacterEnum.Comma, listData);
        }

        public void LinkApplication(string uniqueID, string functionName, string applicationID)
        {
            Connector.AddParameter(ApplicationTable.UniqueID, SqlDbType.VarChar, uniqueID);
            Connector.AddParameter("FunctionName", SqlDbType.VarChar, functionName);
            Connector.AddParameter(ApplicationTable.ApplicationID, SqlDbType.BigInt, applicationID);
            Connector.ExecuteProcedure("dbo.APP_SP_LinkApplication");
        }

        public int SupplyDocument(string uniqueID, string functionName)
        {
            Connector.AddParameter(ApplicationTable.UniqueID, SqlDbType.VarChar, uniqueID);
            Connector.AddParameter("FunctionName", SqlDbType.VarChar, functionName);
            Connector.ExecuteProcedure("dbo.APP_SP_SupplyDocument", out string result);
            return int.Parse(result);
        }
    }
}