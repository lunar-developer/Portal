using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class ApplicationFormProvider: DataProvider
    {
        #region SQL string: Insert Applic Format
        private static readonly string SQL = @"
                BEGIN
                    DECLARE @Step varchar(50) = 'Begin Insert';
                    BEGIN TRY
                        DECLARE @AppID int;
                        BEGIN TRANSACTION
                            SET @Step = 'Step 1: Insert Application';
                            INSERT INTO dbo.APP_Application ({0}) VALUES ({1}); 
                            SET @AppID = @@IDENTITY;
                            
                            SET @Step = 'Step 2: APP_ApplicationFieldInteger';
                            IF({8} > 0)
                                INSERT INTO dbo.APP_ApplicationFieldInteger (ApplicationID,FieldName,FieldValue)  VALUES {2} ;
                            
                            SET @Step = 'Step 3: APP_ApplicationFieldBigInteger';
                            IF({9} > 0)
                                INSERT INTO dbo.APP_ApplicationFieldBigInteger (ApplicationID,FieldName,FieldValue)  VALUES {3} ;
                            
                            SET @Step = 'Step 4: APP_ApplicationFieldString';
                            IF({10} > 0)
                                INSERT INTO dbo.APP_ApplicationFieldString (ApplicationID,FieldName,FieldValue)  VALUES {4} ;
                            
                            SET @Step = 'Step 5: APP_ApplicationFieldUnicodeString';
                            IF({11} > 0)
                                INSERT INTO dbo.APP_ApplicationFieldUnicodeString (ApplicationID,FieldName,FieldValue)  VALUES {5} ;
                            
                            SET @Step = 'Step 6: APP_ApplicationLog';
                            INSERT INTO dbo.APP_ApplicationLog (ApplicationID,{6}) VALUES (@AppID, {7});

                        COMMIT TRANSACTION;
                        SELECT @AppID, N'Cập nhật dữ liệu thành công' AS MESSAGE;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION
                        SELECT 0, @Step + ' | ' + error_message() AS MESSAGE
                    END CATCH
                END";
        #endregion

        #region Utilities SQL: Processing String

        private string GetSqlFormatValue(SqlDbType type, string value)
        {
            if (type == SqlDbType.NVarChar)
            {
                return $"N'{value}'";
            }
            if (type == SqlDbType.VarChar)
            {
                return $"'{value}'";
            }
            return value;
        }

        private string GetFieldTypeValue(string fieldName, string value)
        {
            return $"(@AppID,'{fieldName}',{value})";
        }
        private string GetSqlElement(List<string> list, bool isFieldValue = false)
        {
            if (isFieldValue && (list == null || list.Count == 0))
            {
                return GetFieldTypeValue("'-1'", "'-1'");
            }
            return string.Join(",", list);
        }

        private string GetSqlString(string applicFieldName, string applicValue,
            string integerValue, string bigIntegerValue, string stringValue,
            string unicodeStringValue, string logFieldName, string logValue,
            int countIntgSql, int countBigIntSql, int countStringSql,
            int countUnicodeStringSql)
        {
            return string.Format(SQL, applicFieldName, applicValue, integerValue, bigIntegerValue, stringValue,
                unicodeStringValue, logFieldName, logValue, countIntgSql, countBigIntSql,
                countStringSql, countUnicodeStringSql);
        }

        #endregion

        public DataTable InsertApplicationForm(List<FieldData> listItem)
        {
            List<string> listFieldApplic = new List<string>();
            List<string> listValueApplic = new List<string>();

            List<string> listIntegerValue = new List<string>();
            List<string> listBigIntegerValue = new List<string>();
            List<string> listStringValue = new List<string>();
            List<string> listUnicodeStringValue = new List<string>();

            List<string> listFieldApplicLog = new List<string>();
            List<string> listValueApplicLog = new List<string>();

            #region Filter Sql Value By TableName

            

            #endregion

            string execSql = GetSqlString(GetSqlElement(listFieldApplic), GetSqlElement(listValueApplic),
                GetSqlElement(listIntegerValue,true), GetSqlElement(listBigIntegerValue,true),
                GetSqlElement(listStringValue,true),GetSqlElement(listUnicodeStringValue,true), 
                GetSqlElement(listFieldApplicLog), GetSqlElement(listValueApplicLog),
                listIntegerValue?.Count ?? 0, listBigIntegerValue?.Count ?? 0, 
                listStringValue?.Count ?? 0, listUnicodeStringValue?.Count ?? 0);

            Connector.ExecuteSql(execSql, out DataTable dtResult);

            return dtResult;
        }
    }
}
