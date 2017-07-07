using System;
using System.Collections.Generic;
using System.Data;
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


        static ApplicationProvider()
        {
            int index = 0;

            #region INSERT SCRIPT

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
                        N'Thêm Mới',
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
                        N'Cập Nhật',
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

        public int ProcessApplication(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.APP_SP_ProcessApplication", out string result);
            return int.Parse(result);
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
    }
}