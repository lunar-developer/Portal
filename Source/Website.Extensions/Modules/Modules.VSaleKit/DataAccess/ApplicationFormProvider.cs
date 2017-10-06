using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Modules.VSaleKit.Database;
using Modules.VSaleKit.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;

namespace Modules.VSaleKit.DataAccess
{
    internal class ApplicationFormProvider : DataProvider
    {
        public DataTable SearchApplication(Dictionary<string, SQLParameterData> dictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("VSK_SP_SearchApplication", out DataTable dtResult);
            return dtResult;
        }

        public DataSet LoadApplication(string uniqueID, int userID, string roleName)
        {
            Connector.AddParameter(ApplicationFormTable.UniqueID, SqlDbType.VarChar, uniqueID);
            Connector.AddParameter(BaseTable.UserID, SqlDbType.Int, userID);
            Connector.AddParameter("RoleName", SqlDbType.VarChar, roleName);
            Connector.ExecuteProcedure("dbo.VSK_SP_LoadApplication", out DataSet result);
            return result;
        }

        public DataTable LoadAttachFiles(string uniqueID, string documentCode)
        {
            Connector.AddParameter(ApplicationFormTable.UniqueID, SqlDbType.VarChar, uniqueID);
            Connector.AddParameter(ApplicationFormTable.DocumentCode, SqlDbType.VarChar, documentCode);
            Connector.ExecuteProcedure("dbo.VSK_SP_LoadAttachFiles", out DataTable result);
            return result;
        }

        public DataTable LoadLogHistory(string uniqueID)
        {
            Connector.AddParameter(ApplicationFormTable.UniqueID, SqlDbType.VarChar, uniqueID);
            Connector.ExecuteProcedure("dbo.VSK_SP_LoadLogHistory", out DataTable result);
            return result;
        }

        public int InsertApplication(Dictionary<string, SQLParameterData> dataDictionary, out string uniqueID)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in dataDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.VSK_SP_InsertApplication", out DataTable result);
            uniqueID = result.Rows[0][1].ToString();
            return int.Parse(result.Rows[0][0].ToString());
        }

        public int UpdateApplication(Dictionary<string, SQLParameterData> dataDictionary, out string uniqueID)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in dataDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.VSK_SP_UpdateApplication", out DataTable result);
            uniqueID = result.Rows[0][1].ToString();
            return int.Parse(result.Rows[0][0].ToString());
        }

        public int ProcessApplication(Dictionary<string, SQLParameterData> dataDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in dataDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.VSK_SP_ProcessApplication", out string result);
            return int.Parse(result);
        }


        private const string ScriptInsertFiles = @"
            begin transaction
            begin try
                declare @ErrorCode as int = 0
                {0}
                commit transaction
                select 1
            end try
            begin catch
                rollback transaction
                insert into SYS_Exception(ErrorCode, ErrorMessage, StackTrace, DateTimeCreate)
                values(Error_Number(), Error_Message(), 'ApplicationFormProvider.AttachFiles', dbo.SYS_FN_GetCurrentDateTime())
                select -1
            end catch
        ";

        private const string ScriptUploadFile = @"
            execute VSaleKit.dbo.UploadFile
                @UserName, @RoleName, @UniqueID, @DocumentCode, @PolicyID, 
                {0}, '{1}', '{2}', '{3}', '{4}', {5}, 1, '', @ErrorCode output
        ";

        public int AttachFiles(Dictionary<string, SQLParameterData> dataDictionary, List<FileData> listFiles)
        {
            List<string> listScript = listFiles.Select(file => string.Format(ScriptUploadFile,
                file.FileNumber, file.FileName, file.FileContent,
                file.FilePath, file.FileExtension, file.FileSize)).ToList();

            foreach (KeyValuePair<string, SQLParameterData> pair in dataDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteSql(string.Format(ScriptInsertFiles, string.Join("", listScript)), out string result);
            return int.Parse(result);
        }

        public bool DeleteFile(string userName, string fileID)
        {
            const string script = @"
                update VSaleKit.dbo.FileDetail
                set 
                    Status = '0',
                    UserModif = @ModifyUserName,
                    DateModif = @ModfiyDateTime
                where FileID = @FileID
            ";
            Connector.AddParameter("ModifyUserName", SqlDbType.VarChar, userName);
            Connector.AddParameter("ModfiyDateTime", SqlDbType.VarChar, DateTime.Now.ToString(PatternEnum.DateTime));
            Connector.AddParameter("FileID", SqlDbType.Int, fileID);
            Connector.ExecuteSql(script);
            return true;
        }

        private const string ScriptSortFile = @"
            begin transaction
            begin try
                delete from VSaleKit.dbo.FileDetail
                where 
                    ApplicID = @UniqueID
                and DocID = @DocumentCode
                and Status = 0;

                {0}
                {1}
                commit transaction
                select 1
            end try
            begin catch
                rollback transaction
                insert into SYS_Exception(ErrorCode, ErrorMessage, StackTrace, DateTimeCreate)
                values(Error_Number(), Error_Message(), 'ApplicationFormProvider.SortFiles', dbo.SYS_FN_GetCurrentDateTime())
                select -1
            end catch
        ";
        public bool SortFiles(string uniqueID, string documentCode, Dictionary<int, int> dataDictionary)
        {
            const string scriptReset = @"
                update
                    VSaleKit.dbo.FileDetail
                set
                    FileNumber = FileNumber * -1
                where
                    FileID = {0};
            ";
            const string scriptUpdate = @"
                update
                    VSaleKit.dbo.FileDetail
                set
                    FileNumber = {1},
                    FileName = ApplicID + '-' + DocID + '-' + convert(varchar, {1})
                where
                    FileID = {0};
            ";

            StringBuilder sqlBuilder00 = new StringBuilder();
            StringBuilder sqlBuilder01 = new StringBuilder();
            foreach (KeyValuePair<int, int> pair in dataDictionary)
            {
                sqlBuilder00.AppendLine(string.Format(scriptReset, pair.Key));
                sqlBuilder01.AppendLine(string.Format(scriptUpdate, pair.Key, pair.Value));
            }
            Connector.AddParameter(ApplicationFormTable.UniqueID, SqlDbType.VarChar, uniqueID);
            Connector.AddParameter(ApplicationFormTable.DocumentCode, SqlDbType.VarChar, documentCode);
            Connector.ExecuteSql(string.Format(ScriptSortFile, sqlBuilder00, sqlBuilder01), out string result);
            return result == "1";
        }

        public int GetCurrentFileNumber(Dictionary<string, SQLParameterData> dataDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in dataDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.VSK_SP_GetCurrentFileNumber", out string result);
            return int.Parse(result);
        }
    }
}