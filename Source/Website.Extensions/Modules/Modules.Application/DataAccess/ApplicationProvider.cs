using System;
using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.Database;
using Website.Library.Enum;

namespace Modules.Application.DataAccess
{
    public class ApplicationProvider : DataProvider
    {
        private static int AutoNum;

        #region SCRIPT INSERT

        private static readonly string ScriptInsert = $@"
            declare @Step varchar(50) = 'Begin Insert'
            declare @{ApplicationTable.ApplicationID} bigint
            declare @{ApplicationLogTable.ApplicationLogID} bigint

            begin try                        
                begin transaction
                set @Step = 'Step 1: Insert Application'
                insert into dbo.{ApplicationTable.TableName} ({GetAutoNum(true)}) values ({GetAutoNum()})
                set @{ApplicationTable.ApplicationID} = @@identity

                    
                set @Step = 'Step 2: Insert Field Integer'
                if({GetAutoNum()} > 0)
                    begin
                        insert into dbo.{ApplicationFieldTable.TableInteger}
                            ({ApplicationTable.ApplicationID}, {ApplicationFieldTable.FieldName}, {ApplicationFieldTable.FieldValue})
                        values {GetAutoNum()}
                    end


                set @Step = 'Step 3: Insert Field BigInteger'
                if({GetAutoNum()} > 0)
                    begin
                        insert into dbo.{ApplicationFieldTable.TableBigInteger}
                            ({ApplicationTable.ApplicationID}, {ApplicationFieldTable.FieldName}, {ApplicationFieldTable.FieldValue})
                        values {GetAutoNum()}
                    end


                set @Step = 'Step 4: Insert Field String'
                if({GetAutoNum()} > 0)
                    begin
                        insert into dbo.{ApplicationFieldTable.TableString}
                            ({ApplicationTable.ApplicationID}, {ApplicationFieldTable.FieldName}, {ApplicationFieldTable.FieldValue})
                        values {GetAutoNum()}
                    end


                set @Step = 'Step 5: Insert Field Unicode String'
                if({GetAutoNum()} > 0)
                    begin
                        insert into dbo.{ApplicationFieldTable.TableUnicodeString}
                            ({ApplicationTable.ApplicationID}, {ApplicationFieldTable.FieldName}, {ApplicationFieldTable.FieldValue})
                        values {GetAutoNum()}
                    end


                set @Step = 'Step 6: Insert Log'
                insert into dbo.{ApplicationLogTable.TableName} 
                    (
                        {ApplicationTable.ApplicationID}, {ApplicationLogTable.ActionCode}, {ApplicationLogTable.Remark},
                        {ApplicationLogTable.IsHasLogDetail}, {BaseTable.ModifyUserID}, {BaseTable.ModifyDateTime}
                    ) 
                values
                    (@{ApplicationTable.ApplicationID}, 'INSERT', '', 0, @{BaseTable.ModifyUserID}, @{BaseTable.ModifyDateTime});
                set @{ApplicationLogTable.ApplicationLogID} = @@identity


                set @Step = 'Step 7: Insert Log Detail'
                insert into dbo.{ApplicationLogDetailTable.TableName}
                    (
                        {ApplicationLogTable.ApplicationLogID}, {ApplicationTable.ApplicationID},
                        {ApplicationFieldTable.FieldName}, {ApplicationFieldTable.FieldValue}
                    )
                values {GetAutoNum()}


                set @Step = 'Step 8: Insert Process'
                insert into dbo.{ApplicationProcessTable.TableName}
                    (
                        {ApplicationTable.ApplicationID}, {ApplicationTable.ProcessID}, {ApplicationTable.PhaseID},
                        {ApplicationProcessTable.PreviousPhaseID}, {ApplicationProcessTable.Remark},
                        {ApplicationTable.ProcessUserID}, {ApplicationTable.ProcessDateTime}
                    )
                select
                    {ApplicationTable.ApplicationID}, {ApplicationTable.ProcessID}, {ApplicationTable.PhaseID},
                    0, '',
                    {ApplicationTable.ProcessUserID}, {ApplicationTable.ProcessDateTime}
                from
                    dbo.{ApplicationTable.TableName} with(nolock)               
                where
                    {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}


                commit transaction;
                select @{ApplicationTable.ApplicationID}, '' as Message;
            end try
            begin catch
                rollback transaction
                select 0, @Step + char(10) + char(13) + error_message() as Message
            end catch";

        #endregion

        #region SCRIPT UPDATE

        private static readonly string ScriptUpdate = $@"
            declare @Step varchar(50) = 'Begin Update'
            declare @{ApplicationLogTable.ApplicationLogID} bigint

            begin try                        
                begin transaction
                set @Step = 'Step 1: Update Application'
                update dbo.{ApplicationTable.TableName}
                set {GetAutoNum(true)}
                where {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}

                    
                set @Step = 'Step 2: Insert Field Integer'
                if({GetAutoNum()} > 0)
                    begin
                        delete from dbo.{ApplicationFieldTable.TableInteger}
                        where {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}

                        insert into dbo.{ApplicationFieldTable.TableInteger}
                            ({ApplicationTable.ApplicationID}, {ApplicationFieldTable.FieldName}, {ApplicationFieldTable.FieldValue})
                        values {GetAutoNum()}
                    end


                set @Step = 'Step 3: Insert Field BigInteger'
                if({GetAutoNum()} > 0)
                    begin
                        delete from dbo.{ApplicationFieldTable.TableBigInteger}
                        where {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}

                        insert into dbo.{ApplicationFieldTable.TableBigInteger}
                            ({ApplicationTable.ApplicationID}, {ApplicationFieldTable.FieldName}, {ApplicationFieldTable.FieldValue})
                        values {GetAutoNum()}
                    end


                set @Step = 'Step 4: Insert Field String'
                if({GetAutoNum()} > 0)
                    begin
                        delete from dbo.{ApplicationFieldTable.TableString}
                        where {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}

                        insert into dbo.{ApplicationFieldTable.TableString}
                            ({ApplicationTable.ApplicationID}, {ApplicationFieldTable.FieldName}, {ApplicationFieldTable.FieldValue})
                        values {GetAutoNum()}
                    end


                set @Step = 'Step 5: Insert Field Unicode String'
                if({GetAutoNum()} > 0)
                    begin
                        delete from dbo.{ApplicationFieldTable.TableUnicodeString}
                        where {ApplicationTable.ApplicationID} = @{ApplicationTable.ApplicationID}

                        insert into dbo.{ApplicationFieldTable.TableUnicodeString}
                            ({ApplicationTable.ApplicationID}, {ApplicationFieldTable.FieldName}, {ApplicationFieldTable.FieldValue})
                        values {GetAutoNum()}
                    end


                set @Step = 'Step 6: Insert Log';
                insert into dbo.{ApplicationLogTable.TableName} 
                    (
                        {ApplicationTable.ApplicationID}, {ApplicationLogTable.ActionCode}, {ApplicationLogTable.Remark},
                        {ApplicationLogTable.IsHasLogDetail}, {BaseTable.ModifyUserID}, {BaseTable.ModifyDateTime}
                    ) 
                values
                    (
                        @{ApplicationTable.ApplicationID}, 'UPDATE', '',
                        1, @{BaseTable.ModifyUserID}, @{BaseTable.ModifyDateTime}
                    )
                set @ApplicationLogID = @@identity


                set @Step = 'Step 7: Insert Log Detail'
                insert into dbo.{ApplicationLogDetailTable.TableName}
                    (
                        {ApplicationLogTable.ApplicationLogID}, {ApplicationTable.ApplicationID},
                        {ApplicationFieldTable.FieldName}, {ApplicationFieldTable.FieldValue}
                    )
                values {GetAutoNum()}


                commit transaction;
                select @{ApplicationTable.ApplicationID}, '' as Message;
            end try
            begin catch
                rollback transaction
                select 0, @Step + char(10) + char(13) + error_message() as Message
            end catch";

        #endregion


        public DataTable InsertApplication(int userID, ICollection<FieldData> listApplication,
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
            Connector.ExecuteSql(script, out DataTable result);
            return result;
        }

        public DataTable UpdateApplication(int userID, string applicationID,
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
            Connector.ExecuteSql(script, out DataTable result);
            return result;
        }

        public DataSet LoadApplication(string applicationID, int userID)
        {
            Connector.AddParameter(ApplicationTable.ApplicationID, SqlDbType.BigInt, applicationID);
            Connector.AddParameter(BaseTable.UserID, SqlDbType.Int, userID);
            Connector.ExecuteProcedure("dbo.APP_SP_LoadApplication", out DataSet result);
            return result;
        }

        public DataTable SearchApplication(Dictionary<string, string> conditionDictionary)
        {
            foreach (KeyValuePair<string, string> pair in conditionDictionary)
            {
                Connector.AddParameter(pair.Key, SqlDbType.NVarChar, pair.Value);
            }
            Connector.ExecuteProcedure("dbo.APP_SP_SearchApplication", out DataTable result);
            return result;
        }


        private static string GetAutoNum(bool isReset = false)
        {
            if (isReset)
            {
                AutoNum = 0;
            }
            return "{" + AutoNum++ + "}";
        }

        private static string BuildScript(ICollection<FieldData> listField)
        {
            if (listField.Count <= 0)
            {
                return "(@ApplicationID, NULL, NULL)";
            }

            List<string> listData = new List<string>();
            foreach (FieldData fieldInfo in listField)
            {
                listData.Add($"(@ApplicationID, '{fieldInfo.FieldName}', {fieldInfo.FieldValue})");
            }
            return string.Join(CharacterEnum.Comma, listData);
        }

        private static string BuildLogScript(ICollection<FieldData> listField)
        {
            if (listField.Count <= 0)
            {
                return "(@ApplicationLogID, @ApplicationID, NULL, NULL)";
            }

            List<string> listData = new List<string>();
            foreach (FieldData fieldInfo in listField)
            {
                listData.Add($"(@ApplicationLogID, @ApplicationID, '{fieldInfo.FieldName}', {fieldInfo.FieldValue})");
            }
            return string.Join(CharacterEnum.Comma, listData);
        }
    }
}