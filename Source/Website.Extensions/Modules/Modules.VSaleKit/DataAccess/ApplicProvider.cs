using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Website.Library.Enum;
using Modules.VSaleKit.Enum;
using Modules.VSaleKit.DataTransfer;
using Modules.VSaleKit.Global;

namespace Modules.VSaleKit.DataAccess
{
    public class ApplicProvider : DatabaseCtrl
    {
        public ApplicProvider() : base(ConnectionEnum.VSaleKit)
        {
            ;            
        }
        public string GetApplicId(string username, string brcd)
        {
            string applicId = "";
            try
            {
                SqlCommand cmd = new SqlCommand("GetApplicID", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, username));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, brcd));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameterWithSize("@ApplicId", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Output, 255));
                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                int r = cmd.ExecuteNonQuery();
                applicId = (string)cmd.Parameters["@ApplicId"].Value;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return applicId;
        }

        public int UpLoadFile(string UserName, string UserType, string ApplicId, string PolicyId, string DocId, int FileNumber, string FileName, string FileData,
            string FileExt, int FileSize, string Status, string Description, string FilePath)
        {
            int r;
            try
            {
                SqlCommand cmd = new SqlCommand("[UploadFile]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@DocID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, DocId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@PolicyID", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Input, int.Parse(PolicyId)));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@FileNumber", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Input, FileNumber));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@FileName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, FileName));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@FileData", System.Data.SqlDbType.NText, System.Data.ParameterDirection.Input, FileData));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@FilePath", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, FilePath));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@FileExt", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, FileExt));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@FileSize", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Input, FileSize));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Status", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Status));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Description", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, Description));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ErrorCode", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output));

                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                r = cmd.ExecuteNonQuery();
                r = (int)cmd.Parameters["@ErrorCode"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

        public int CreateApplic(string UserName, string Brcd, string UserType, string ApplicId, string ProcessId, string Priority, string Status,
            string CustName, string LegalId, string LegalType, string Description, decimal CreditLimit, string PolicyId, string phoneNo)
        {
            int r;
            string notifyUser = "";
            string token = "";
            try
            {
                SqlCommand cmd = new SqlCommand("[CreateApplic]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ProcessID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ProcessId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Priority", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Priority));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Status", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Status));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@CustName", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, Utilities.RemoveUnicodeAndSpace(CustName).ToUpper()));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@SearchCustName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Utilities.RemoveUnicodeAndSpace(CustName)));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@LegalID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, LegalId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@LegalType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, LegalType));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Brithday", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, null));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Phone", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, phoneNo));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Address", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, null));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Description", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, Description));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@CreditLimit", System.Data.SqlDbType.Money, System.Data.ParameterDirection.Input, CreditLimit));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@PolicyID", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Input, int.Parse(PolicyId)));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameterWithSize("@UserReceiveNotification", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Output, 30));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameterWithSize("@Token", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Output, 255));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ErrorCode", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output));


                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                r = cmd.ExecuteNonQuery();
                r = (int)cmd.Parameters["@ErrorCode"].Value;
                if (r == 0 && Status == "1")
                {
                    notifyUser = cmd.Parameters["@UserReceiveNotification"].Value == DBNull.Value ? "" : (string)cmd.Parameters["@UserReceiveNotification"].Value;
                    token = cmd.Parameters["@Token"].Value == DBNull.Value ? "" : (string)cmd.Parameters["@Token"].Value;
                    if (token != "")
                    {
                        InsertApplicNotification(UserName, notifyUser, Utilities.CreateNotificationTitle("1"), Utilities.CreateNotificationMsg("1"), ApplicId);                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

        public int GetDocumentByPolicyId(string PolicyId, ref List<DocumentInfo> docLst)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            docLst = new List<DocumentInfo>();
            try
            {
                string sql = @"select d.DocumentCode as DocID, d.Name as DocName, pd.OrderNo
	                            from PortalModule.dbo.APP_Policy p, PortalModule.dbo.APP_PolicyDocument pd, PortalModule.dbo.APP_DocumentType d
	                            where p.PolicyID=@PolicyId 
                                and p.PolicyID = pd.PolicyID 
	                            and pd.DocumentTypeID = d.DocumentTypeID	                            	                            
	                            order by pd.OrderNo";
                SqlCommand cmd = new SqlCommand(sql, dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@PolicyId", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, PolicyId));                
                                
                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        DocumentInfo doc = new DocumentInfo();
                        doc.DocId = r["DocID"] == DBNull.Value ? "" : r["DocID"].ToString();
                        doc.DocName = r["DocName"] == DBNull.Value ? "" : r["DocName"].ToString();
                        doc.OrderNo = r["OrderNo"] == DBNull.Value ? 0 : int.Parse(r["OrderNo"].ToString());
                        docLst.Add(doc);
                    }                   
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetListFileByApplicId(string applicID, ref List<FileData> fileLst)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            fileLst = new List<FileData>();
            try
            {
                string sql = @"select fd.DocID, fd.FileExt, fd.FileName, fd.FileNumber, fd.FileSize, d.Name as DocName
	                            from FileDetail fd, PortalModule.dbo.APP_DocumentType d
	                            where fd.ApplicID = @ApplicID
	                            and fd.Status = '1'
	                            and fd.DocID = d.DocumentCode	 
	                            order by fd.DocID, fd.FileNumber";
                SqlCommand cmd = new SqlCommand(sql, dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, applicID));

                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        FileData file = new FileData();
                        //file.DocId = r["DocID"] == DBNull.Value ? "" : r["DocID"].ToString().Trim();
                        //file.DocName = r["DocName"] == DBNull.Value ? "" : r["DocName"].ToString().Trim();
                        //file.FileExt = r["FileExt"] == DBNull.Value ? "" : r["FileExt"].ToString().Trim();
                        //file.FileName = r["FileName"] == DBNull.Value ? "" : r["FileName"].ToString().Trim();
                        //file.FileNumber = r["FileNumber"] == DBNull.Value ? 0 : int.Parse(r["FileNumber"].ToString());
                        //file.FileSize = r["FileSize"] == DBNull.Value ? 0 : int.Parse(r["FileSize"].ToString());
                        fileLst.Add(file);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int GetOrderDocByPolicy(string docId, List<DocumentInfo> docLst)
        {
            foreach(DocumentInfo doc in docLst)
            {
                if (doc.DocId == docId)
                    return doc.OrderNo;
            }
            return 0;
        }

        public int GetApplicInfo(string ApplicId, string UserName, string UserType, string Brcd, ref ApplicInfo Applic, ref List<FileData> FileList)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            List<DocumentInfo> docLst = new List<DocumentInfo>();
            try
            {
                
                SqlCommand cmd = new SqlCommand("[PortalModule].[dbo].[VSK_SP_GetApplication]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchCode", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    FileList = new List<FileData>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Applic = new ApplicInfo();
                        Applic.ApplicId = ApplicId;
                        Applic.Brcd = r["BranchID"] == DBNull.Value ? "" : r["BranchID"].ToString();
                        Applic.CreditLimit = r["CreditLimit"] == DBNull.Value ? 0 : decimal.Parse(r["CreditLimit"].ToString());
                        Applic.CustId = r["CustID"] == DBNull.Value ? "" : r["CustID"].ToString();
                        Applic.CustName = r["CustName"] == DBNull.Value ? "" : r["CustName"].ToString();
                        Applic.DateCreate = r["DateCreate"] == DBNull.Value ? "" : r["DateCreate"].ToString();
                        Applic.DateModif = r["DateModif"] == DBNull.Value ? "" : r["DateModif"].ToString();
                        Applic.Description = r["Description"] == DBNull.Value ? "" : r["Description"].ToString();
                        Applic.GroupFileId = r["GroupFileID"] == DBNull.Value ? -1 : int.Parse(r["GroupFileID"].ToString());
                        Applic.LegalId = r["LegalId"] == DBNull.Value ? "" : r["LegalId"].ToString();
                        Applic.LegalType = r["LegalType"] == DBNull.Value ? "" : r["LegalType"].ToString();
                        Applic.NextUserProcess = r["NextUsrProcess"] == DBNull.Value ? "" : r["NextUsrProcess"].ToString();
                        Applic.NextUserTypeProcess = r["NextUsrTypeProcess"] == DBNull.Value ? "" : r["NextUsrTypeProcess"].ToString();
                        Applic.Priority = r["Priority"] == DBNull.Value ? "" : r["Priority"].ToString();
                        Applic.ProcessId = r["ProcessId"] == DBNull.Value ? "" : r["ProcessId"].ToString();
                        Applic.Status = r["StatusID"] == DBNull.Value ? "" : r["StatusID"].ToString();
                        Applic.UserCreate = r["UserCreate"] == DBNull.Value ? "" : r["UserCreate"].ToString();
                        Applic.UserModif = r["UserModif"] == DBNull.Value ? "" : r["UserModif"].ToString();
                        Applic.UserProcess = r["UsrProcess"] == DBNull.Value ? "" : r["UsrProcess"].ToString();
                        Applic.UserTypeProcess = r["UsrTypeProcess"] == DBNull.Value ? "" : r["UsrTypeProcess"].ToString();
                        Applic.PolicyId = r["PolicyID"] == DBNull.Value ? "" : r["PolicyID"].ToString();

                        Applic.LegalName = r["LegalName"] == DBNull.Value ? "" : r["LegalName"].ToString().Trim();
                        Applic.ProcessName = r["ProcessName"] == DBNull.Value ? "" : r["ProcessName"].ToString().Trim();
                        Applic.PolicyName = r["PolicyName"] == DBNull.Value ? "" : r["PolicyName"].ToString().Trim();
                        Applic.PreApprFlag = r["PreApprFlag"] == DBNull.Value ? "" : r["PreApprFlag"].ToString().Trim();
                        Applic.UserCreateType = r["UserCreateType"] == DBNull.Value ? "" : r["UserCreateType"].ToString().Trim();
                        Applic.DisplayName = r["DisplayName"] == DBNull.Value ? "" : r["DisplayName"].ToString().Trim();
                        Applic.OriUserProcess = r["OriUsrProcess"] == DBNull.Value ? "" : r["OriUsrProcess"].ToString().Trim();
                        Applic.OriUserTypeProcess = r["OriUsrTypeProcess"] == DBNull.Value ? "" : r["OriUsrTypeProcess"].ToString().Trim();
                        Applic.OriDisplayName = r["OriDisplayName"] == DBNull.Value ? "" : r["OriDisplayName"].ToString().Trim();
                        Applic.ApplicationTypeID = r["ApplicationTypeID"] == DBNull.Value ? "" : r["ApplicationTypeID"].ToString().Trim();
                        Applic.PhoneNo = r["Phone"] == DBNull.Value ? "" : r["Phone"].ToString().Trim();
                        break;
                    }
                    GetDocumentByPolicyId(Applic.PolicyId, ref docLst);
                    foreach (DataRow r in ds.Tables[1].Rows)
                    {
                        FileData file = new FileData();
                        //file.DocId = r["DocId"] == DBNull.Value ? "" : r["DocId"].ToString().Trim();
                        //file.FileExt = r["FileExt"] == DBNull.Value ? "" : r["FileExt"].ToString().Trim();
                        //file.FileName = r["FileName"] == DBNull.Value ? "" : r["FileName"].ToString().Trim();
                        //file.FileNumber = r["FileNumber"] == DBNull.Value ? -1 : int.Parse(r["FileNumber"].ToString());
                        //file.FileSize = r["FileSize"] == DBNull.Value ? -1 : int.Parse(r["FileSize"].ToString());
                        //file.DocName = r["DocName"] == DBNull.Value ? "" : r["DocName"].ToString().Trim();
                        //file.GroupOrder = GetOrderDocByPolicy(file.DocId, docLst);
                        FileList.Add(file);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApplicInfo> GetListApplicByUser(string UserName, string FromDate, string ToDate)
        {
            List<ApplicInfo> appList = new List<ApplicInfo>();
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("[GetListApplicByUser]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@FromDate", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, FromDate + "000000"));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ToDate", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ToDate + "235959"));
                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ApplicInfo Applic = new ApplicInfo();
                        Applic.ApplicId = r["ApplicID"] == DBNull.Value ? "" : r["ApplicID"].ToString().Trim();
                        Applic.Brcd = r["BranchID"] == DBNull.Value ? "" : r["BranchID"].ToString().Trim();
                        Applic.CreditLimit = r["CreditLimit"] == DBNull.Value ? 0 : decimal.Parse(r["CreditLimit"].ToString());
                        Applic.CustId = r["CustID"] == DBNull.Value ? "" : r["CustID"].ToString();
                        Applic.CustName = r["CustName"] == DBNull.Value ? "" : r["CustName"].ToString().ToString();
                        Applic.DateCreate = r["DateCreate"] == DBNull.Value ? "" : Utilities.FormatDate2String(r["DateCreate"].ToString(), "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss");
                        Applic.DateModif = r["DateModif"] == DBNull.Value ? "" : Utilities.FormatDate2String(r["DateModif"].ToString(), "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss");
                        Applic.Description = r["Description"] == DBNull.Value ? "" : r["Description"].ToString().Trim();
                        Applic.GroupFileId = r["GroupFileID"] == DBNull.Value ? -1 : int.Parse(r["GroupFileID"].ToString());
                        Applic.LegalId = r["LegalId"] == DBNull.Value ? "" : r["LegalId"].ToString().Trim();
                        Applic.LegalType = r["LegalType"] == DBNull.Value ? "" : r["LegalType"].ToString().Trim();
                        Applic.NextUserProcess = r["NextUsrProcess"] == DBNull.Value ? "" : r["NextUsrProcess"].ToString().Trim();
                        Applic.NextUserTypeProcess = r["NextUsrTypeProcess"] == DBNull.Value ? "" : r["NextUsrTypeProcess"].ToString().Trim();
                        Applic.Priority = r["Priority"] == DBNull.Value ? "" : r["Priority"].ToString().Trim();
                        Applic.ProcessId = r["ProcessId"] == DBNull.Value ? "" : r["ProcessId"].ToString().Trim();
                        Applic.Status = r["StatusID"] == DBNull.Value ? "" : r["StatusID"].ToString().Trim();
                        Applic.UserCreate = r["UserCreate"] == DBNull.Value ? "" : r["UserCreate"].ToString().Trim();
                        Applic.UserModif = r["UserModif"] == DBNull.Value ? "" : r["UserModif"].ToString().Trim();
                        Applic.UserProcess = r["UsrProcess"] == DBNull.Value ? "" : r["UsrProcess"].ToString().Trim();
                        Applic.UserTypeProcess = r["UsrTypeProcess"] == DBNull.Value ? "" : r["UsrTypeProcess"].ToString().Trim();
                        Applic.PolicyId = r["PolicyID"] == DBNull.Value ? "" : r["PolicyID"].ToString().Trim();

                        Applic.LegalName = r["LegalName"] == DBNull.Value ? "" : r["LegalName"].ToString().Trim();
                        Applic.ProcessName = r["ProcessName"] == DBNull.Value ? "" : r["ProcessName"].ToString().Trim();
                        Applic.PolicyName = r["PolicyName"] == DBNull.Value ? "" : r["PolicyName"].ToString().Trim();

                        Applic.PreApprFlag = r["PreApprFlag"] == DBNull.Value ? "" : r["PreApprFlag"].ToString().Trim();
                        Applic.UserCreateType = r["UserCreateType"] == DBNull.Value ? "" : r["UserCreateType"].ToString().Trim();
                        Applic.DisplayName = r["DisplayName"] == DBNull.Value ? "" : r["DisplayName"].ToString().Trim();

                        Applic.OriUserProcess = r["OriUsrProcess"] == DBNull.Value ? "" : r["OriUsrProcess"].ToString().Trim();
                        Applic.OriUserTypeProcess = r["OriUsrTypeProcess"] == DBNull.Value ? "" : r["OriUsrTypeProcess"].ToString().Trim();
                        Applic.OriDisplayName = r["OriDisplayName"] == DBNull.Value ? "" : r["OriDisplayName"].ToString().Trim();
                        appList.Add(Applic);                        
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return appList;
        }

        public List<ApplicInfo> GetListApplicProcessedByUser(string UserName, string FromDate, string ToDate)
        {
            List<ApplicInfo> appList = new List<ApplicInfo>();
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("[GetListApplicProcessedByUser]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@FromDate", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, FromDate + "000000"));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ToDate", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ToDate + "235959"));
                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ApplicInfo Applic = new ApplicInfo();
                        Applic.ApplicId = r["ApplicID"] == DBNull.Value ? "" : r["ApplicID"].ToString().Trim();
                        Applic.Brcd = r["BranchID"] == DBNull.Value ? "" : r["BranchID"].ToString().Trim();
                        Applic.CreditLimit = r["CreditLimit"] == DBNull.Value ? 0 : decimal.Parse(r["CreditLimit"].ToString());
                        Applic.CustId = r["CustID"] == DBNull.Value ? "" : r["CustID"].ToString();
                        Applic.CustName = r["CustName"] == DBNull.Value ? "" : r["CustName"].ToString().ToString();
                        Applic.DateCreate = r["DateCreate"] == DBNull.Value ? "" : Utilities.FormatDate2String(r["DateCreate"].ToString(), "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss");
                        Applic.DateModif = r["DateModif"] == DBNull.Value ? "" : Utilities.FormatDate2String(r["DateModif"].ToString(), "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss");
                        Applic.Description = r["Description"] == DBNull.Value ? "" : r["Description"].ToString().Trim();
                        Applic.GroupFileId = r["GroupFileID"] == DBNull.Value ? -1 : int.Parse(r["GroupFileID"].ToString());
                        Applic.LegalId = r["LegalId"] == DBNull.Value ? "" : r["LegalId"].ToString().Trim();
                        Applic.LegalType = r["LegalType"] == DBNull.Value ? "" : r["LegalType"].ToString().Trim();
                        Applic.NextUserProcess = r["NextUsrProcess"] == DBNull.Value ? "" : r["NextUsrProcess"].ToString().Trim();
                        Applic.NextUserTypeProcess = r["NextUsrTypeProcess"] == DBNull.Value ? "" : r["NextUsrTypeProcess"].ToString().Trim();
                        Applic.Priority = r["Priority"] == DBNull.Value ? "" : r["Priority"].ToString().Trim();
                        Applic.ProcessId = r["ProcessId"] == DBNull.Value ? "" : r["ProcessId"].ToString().Trim();
                        Applic.Status = r["StatusID"] == DBNull.Value ? "" : r["StatusID"].ToString().Trim();
                        Applic.UserCreate = r["UserCreate"] == DBNull.Value ? "" : r["UserCreate"].ToString().Trim();
                        Applic.UserModif = r["UserModif"] == DBNull.Value ? "" : r["UserModif"].ToString().Trim();
                        Applic.UserProcess = r["UsrProcess"] == DBNull.Value ? "" : r["UsrProcess"].ToString().Trim();
                        Applic.UserTypeProcess = r["UsrTypeProcess"] == DBNull.Value ? "" : r["UsrTypeProcess"].ToString().Trim();
                        Applic.PolicyId = r["PolicyID"] == DBNull.Value ? "" : r["PolicyID"].ToString().Trim();

                        Applic.LegalName = r["LegalName"] == DBNull.Value ? "" : r["LegalName"].ToString().Trim();
                        Applic.ProcessName = r["ProcessName"] == DBNull.Value ? "" : r["ProcessName"].ToString().Trim();
                        Applic.PolicyName = r["PolicyName"] == DBNull.Value ? "" : r["PolicyName"].ToString().Trim();

                        Applic.PreApprFlag = r["PreApprFlag"] == DBNull.Value ? "" : r["PreApprFlag"].ToString().Trim();
                        Applic.UserCreateType = r["UserCreateType"] == DBNull.Value ? "" : r["UserCreateType"].ToString().Trim();
                        Applic.DisplayName = r["DisplayName"] == DBNull.Value ? "" : r["DisplayName"].ToString().Trim();

                        Applic.OriUserProcess = r["OriUsrProcess"] == DBNull.Value ? "" : r["OriUsrProcess"].ToString().Trim();
                        Applic.OriUserTypeProcess = r["OriUsrTypeProcess"] == DBNull.Value ? "" : r["OriUsrTypeProcess"].ToString().Trim();
                        Applic.OriDisplayName = r["OriDisplayName"] == DBNull.Value ? "" : r["OriDisplayName"].ToString().Trim();
                        appList.Add(Applic);                        
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return appList;
        }

        public List<ApplicInfo> SearchApplic(string userID,string fromDate, string toDate,string customerInfo, string brcd)
        {
            List<ApplicInfo> appList = new List<ApplicInfo>();
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("[PortalModule].[dbo].[VSK_SP_SearchApplication]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserID", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Input, int.Parse(userID)));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@FromDate", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, fromDate));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ToDate", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, toDate));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@CustomerInfo", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, customerInfo));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchCode", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, brcd));
                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ApplicInfo Applic = new ApplicInfo();
                        Applic.ApplicId = r["ApplicID"] == DBNull.Value ? "" : r["ApplicID"].ToString().Trim();
                        Applic.Brcd = r["BranchID"] == DBNull.Value ? "" : r["BranchID"].ToString().Trim();
                        Applic.CreditLimit = r["CreditLimit"] == DBNull.Value ? 0 : decimal.Parse(r["CreditLimit"].ToString());
                        Applic.CustId = r["CustID"] == DBNull.Value ? "" : r["CustID"].ToString();
                        Applic.CustName = r["CustName"] == DBNull.Value ? "" : r["CustName"].ToString().ToString();
                        Applic.DateCreate = r["DateCreate"] == DBNull.Value ? "" : Utilities.FormatDate2String(r["DateCreate"].ToString(), "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss");
                        Applic.DateModif = r["DateModif"] == DBNull.Value ? "" : Utilities.FormatDate2String(r["DateModif"].ToString(), "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss");
                        Applic.Description = r["Description"] == DBNull.Value ? "" : r["Description"].ToString().Trim();
                        Applic.GroupFileId = r["GroupFileID"] == DBNull.Value ? -1 : int.Parse(r["GroupFileID"].ToString());
                        Applic.LegalId = r["LegalId"] == DBNull.Value ? "" : r["LegalId"].ToString().Trim();
                        Applic.LegalType = r["LegalType"] == DBNull.Value ? "" : r["LegalType"].ToString().Trim();
                        Applic.NextUserProcess = r["NextUsrProcess"] == DBNull.Value ? "" : r["NextUsrProcess"].ToString().Trim();
                        Applic.NextUserTypeProcess = r["NextUsrTypeProcess"] == DBNull.Value ? "" : r["NextUsrTypeProcess"].ToString().Trim();
                        Applic.Priority = r["Priority"] == DBNull.Value ? "" : r["Priority"].ToString().Trim();
                        Applic.ProcessId = r["ProcessId"] == DBNull.Value ? "" : r["ProcessId"].ToString().Trim();
                        Applic.Status = r["StatusID"] == DBNull.Value ? "" : r["StatusID"].ToString().Trim();
                        Applic.UserCreate = r["UserCreate"] == DBNull.Value ? "" : r["UserCreate"].ToString().Trim();
                        Applic.UserModif = r["UserModif"] == DBNull.Value ? "" : r["UserModif"].ToString().Trim();
                        Applic.UserProcess = r["UsrProcess"] == DBNull.Value ? "" : r["UsrProcess"].ToString().Trim();
                        Applic.UserTypeProcess = r["UsrTypeProcess"] == DBNull.Value ? "" : r["UsrTypeProcess"].ToString().Trim();
                        Applic.PolicyId = r["PolicyID"] == DBNull.Value ? "" : r["PolicyID"].ToString().Trim();

                        Applic.LegalName = r["LegalName"] == DBNull.Value ? "" : r["LegalName"].ToString().Trim();
                        Applic.ProcessName = r["ProcessName"] == DBNull.Value ? "" : r["ProcessName"].ToString().Trim();
                        Applic.PolicyName = r["PolicyName"] == DBNull.Value ? "" : r["PolicyName"].ToString().Trim();

                        Applic.PreApprFlag = r["PreApprFlag"] == DBNull.Value ? "" : r["PreApprFlag"].ToString().Trim();
                        Applic.UserCreateType = r["UserCreateType"] == DBNull.Value ? "" : r["UserCreateType"].ToString().Trim();
                        Applic.DisplayName = r["DisplayName"] == DBNull.Value ? "" : r["DisplayName"].ToString().Trim();

                        Applic.OriUserProcess = r["OriUsrProcess"] == DBNull.Value ? "" : r["OriUsrProcess"].ToString().Trim();
                        Applic.OriUserTypeProcess = r["OriUsrTypeProcess"] == DBNull.Value ? "" : r["OriUsrTypeProcess"].ToString().Trim();
                        Applic.OriDisplayName = r["OriDisplayName"] == DBNull.Value ? "" : r["OriDisplayName"].ToString().Trim();
                        appList.Add(Applic);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return appList;
        }

        public List<ApplicInfo> GetListApplicWaitApproval(string UserName, string UserType, string Brcd)
        {
            List<ApplicInfo> appList = new List<ApplicInfo>();
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("[GetListApplicWaitApproval]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ApplicInfo Applic = new ApplicInfo();
                        Applic.ApplicId = r["ApplicID"] == DBNull.Value ? "" : r["ApplicID"].ToString().Trim();
                        Applic.Brcd = r["BranchID"] == DBNull.Value ? "" : r["BranchID"].ToString().Trim();
                        Applic.CreditLimit = r["CreditLimit"] == DBNull.Value ? 0 : decimal.Parse(r["CreditLimit"].ToString());
                        Applic.CustId = r["CustID"] == DBNull.Value ? "" : r["CustID"].ToString();
                        Applic.CustName = r["CustName"] == DBNull.Value ? "" : r["CustName"].ToString().ToString();
                        Applic.DateCreate = r["DateCreate"] == DBNull.Value ? "" : Utilities.FormatDate2String(r["DateCreate"].ToString(), "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss");
                        Applic.DateModif = r["DateModif"] == DBNull.Value ? "" : Utilities.FormatDate2String(r["DateModif"].ToString(), "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss");
                        Applic.Description = r["Description"] == DBNull.Value ? "" : r["Description"].ToString().Trim();
                        Applic.GroupFileId = r["GroupFileID"] == DBNull.Value ? -1 : int.Parse(r["GroupFileID"].ToString());
                        Applic.LegalId = r["LegalId"] == DBNull.Value ? "" : r["LegalId"].ToString().Trim();
                        Applic.LegalType = r["LegalType"] == DBNull.Value ? "" : r["LegalType"].ToString().Trim();
                        Applic.NextUserProcess = r["NextUsrProcess"] == DBNull.Value ? "" : r["NextUsrProcess"].ToString().Trim();
                        Applic.NextUserTypeProcess = r["NextUsrTypeProcess"] == DBNull.Value ? "" : r["NextUsrTypeProcess"].ToString().Trim();
                        Applic.Priority = r["Priority"] == DBNull.Value ? "" : r["Priority"].ToString().Trim();
                        Applic.ProcessId = r["ProcessId"] == DBNull.Value ? "" : r["ProcessId"].ToString().Trim();
                        Applic.Status = r["StatusID"] == DBNull.Value ? "" : r["StatusID"].ToString().Trim();
                        Applic.UserCreate = r["UserCreate"] == DBNull.Value ? "" : r["UserCreate"].ToString().Trim();
                        Applic.UserModif = r["UserModif"] == DBNull.Value ? "" : r["UserModif"].ToString().Trim();
                        Applic.UserProcess = r["UsrProcess"] == DBNull.Value ? "" : r["UsrProcess"].ToString().Trim();
                        Applic.UserTypeProcess = r["UsrTypeProcess"] == DBNull.Value ? "" : r["UsrTypeProcess"].ToString().Trim();
                        Applic.PolicyId = r["PolicyID"] == DBNull.Value ? "" : r["PolicyID"].ToString().Trim();

                        Applic.LegalName = r["LegalName"] == DBNull.Value ? "" : r["LegalName"].ToString().Trim();
                        Applic.ProcessName = r["ProcessName"] == DBNull.Value ? "" : r["ProcessName"].ToString().Trim();
                        Applic.PolicyName = r["PolicyName"] == DBNull.Value ? "" : r["PolicyName"].ToString().Trim();

                        Applic.PreApprFlag = r["PreApprFlag"] == DBNull.Value ? "" : r["PreApprFlag"].ToString().Trim();
                        Applic.UserCreateType = r["UserCreateType"] == DBNull.Value ? "" : r["UserCreateType"].ToString().Trim();
                        Applic.DisplayName = r["DisplayName"] == DBNull.Value ? "" : r["DisplayName"].ToString().Trim();

                        Applic.OriUserProcess = r["OriUsrProcess"] == DBNull.Value ? "" : r["OriUsrProcess"].ToString().Trim();
                        Applic.OriUserTypeProcess = r["OriUsrTypeProcess"] == DBNull.Value ? "" : r["OriUsrTypeProcess"].ToString().Trim();
                        Applic.OriDisplayName = r["OriDisplayName"] == DBNull.Value ? "" : r["OriDisplayName"].ToString().Trim();                        
                        appList.Add(Applic);                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return appList;
        }

        public int Approval(string ApplicId, string UserName, string UserType, string Brcd, string Description)
        {
            int r = -1;
            string notifyUser = "";
            string token = "";
            string statusID = "";

            try
            {
                SqlCommand cmd = new SqlCommand("[Approval]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Description", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, Description));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameterWithSize("@UserReceiveNotification", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Output, 30));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameterWithSize("@Token", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Output, 255));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameterWithSize("@StatusID", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Output, 10));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ErrorCode", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output));

                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                r = cmd.ExecuteNonQuery();
                r = (int)cmd.Parameters["@ErrorCode"].Value;
                if (r == 0)
                {
                    notifyUser = cmd.Parameters["@UserReceiveNotification"].Value == DBNull.Value ? "" : (string)cmd.Parameters["@UserReceiveNotification"].Value;
                    token = cmd.Parameters["@Token"].Value == DBNull.Value ? "" : (string)cmd.Parameters["@Token"].Value;
                    statusID = (string)cmd.Parameters["@StatusID"].Value;
                    if (token != "")
                    {
                        InsertApplicNotification(UserName, notifyUser, Utilities.CreateNotificationTitle(statusID), Utilities.CreateNotificationMsg(statusID),ApplicId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

        public int InsertApplicNotification(string userCreate, string usrReceive, string title, string msg, string applicId)
        {
            int r = 0;
            string sql = @"insert into [Notification](UserCreate, DateCreate, UsrReceive, NotificationType, Title, NotificationMsg, Status,Para01,Send)
                            values(@UserCreate, format(getdate(), 'yyyyMMddHHmmssfff'), @UsrReceive, '1', @Title, @NotificationMsg, '1',@ApplicID,'0')";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserCreate", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, userCreate));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UsrReceive", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, usrReceive));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Title", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, title));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@NotificationMsg", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, msg));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, applicId));
                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                r = cmd.ExecuteNonQuery();                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }


        public int ReturnApplic(string ApplicId, string UserName, string UserType, string Brcd, string Description)
        {
            int r = -1;
            string notifyUser = "";
            string token = "";
            try
            {
                SqlCommand cmd = new SqlCommand("[Return]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Description", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, Description));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameterWithSize("@UserReceiveNotification", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Output, 30));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameterWithSize("@Token", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Output, 255));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ErrorCode", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output));

                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                r = cmd.ExecuteNonQuery();
                r = (int)cmd.Parameters["@ErrorCode"].Value;
                if (r == 0)
                {
                    notifyUser = cmd.Parameters["@UserReceiveNotification"].Value == DBNull.Value ? "" : (string)cmd.Parameters["@UserReceiveNotification"].Value;
                    token = cmd.Parameters["@Token"].Value == DBNull.Value ? "" : (string)cmd.Parameters["@Token"].Value;
                    if (token != "")
                    {
                        InsertApplicNotification(UserName, notifyUser, Utilities.CreateNotificationTitle("3"), Utilities.CreateNotificationMsg("3"), ApplicId);                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

        public int RecallApplic(string ApplicId, string UserName, string UserType, string Brcd, string Description)
        {
            int r = -1;
            try
            {
                SqlCommand cmd = new SqlCommand("[ReCall]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Description", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, Description));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ErrorCode", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output));

                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                r = cmd.ExecuteNonQuery();
                r = (int)cmd.Parameters["@ErrorCode"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

        public int CloseApplic(string ApplicId, string UserName, string UserType, string Brcd, string Description)
        {
            int r = -1;
            try
            {
                SqlCommand cmd = new SqlCommand("[CloseApplic]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Description", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, Description));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ErrorCode", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output));

                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                r = cmd.ExecuteNonQuery();
                r = (int)cmd.Parameters["@ErrorCode"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

        public List<ApplicProcessHistoryInfo> GetApplicProcessHistory(string ApplicId, string UserName)
        {
            List<ApplicProcessHistoryInfo> appList = new List<ApplicProcessHistoryInfo>();
            SqlDataAdapter da;
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("[PortalModule].[dbo].[VSK_SP_GetApplicProcessHistory]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));                
                
                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {                   
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ApplicProcessHistoryInfo Applic = new ApplicProcessHistoryInfo();
                        Applic.CreateDate = r["DateCreate"] == DBNull.Value ? "" : r["DateCreate"].ToString().Trim();
                        Applic.Description = r["Description"] == DBNull.Value ? "" : r["Description"].ToString().Trim();
                        Applic.DisplayName = r["DisplayName"] == DBNull.Value ? "" : r["DisplayName"].ToString().Trim();
                        Applic.PreStatus = r["PreStatusID"] == DBNull.Value ? "" : r["PreStatusID"].ToString().Trim();
                        Applic.PreUserProcess = r["PreUsrProcess"] == DBNull.Value ? "" : r["PreUsrProcess"].ToString().Trim();
                        Applic.ProcessId = r["ProcessID"] == DBNull.Value ? "" : r["ProcessID"].ToString().Trim();
                        Applic.Status = r["StatusID"] == DBNull.Value ? "" : r["StatusID"].ToString().Trim();
                        Applic.UserCreate = r["UserCreate"] == DBNull.Value ? "" : r["UserCreate"].ToString().Trim();
                        Applic.ActionDesc = CreateActionDescription(Applic.DisplayName, Applic.Status, Applic.PreStatus);
                        appList.Insert(appList.Count, Applic);                        
                    }

                    foreach (DataRow r in ds.Tables[1].Rows)
                    {
                        string status = r["StatusID"] == DBNull.Value ? "" : r["StatusID"].ToString().Trim();
                        string DisplayNameOfNextUsrProcess = r["DisplayNameOfNextUsrProcess"] == DBNull.Value ? "" : r["DisplayNameOfNextUsrProcess"].ToString().Trim();
                        if ((status == "0A" || status == "0" || status == "1" || status == "2" || status == "3" || status == "4") && DisplayNameOfNextUsrProcess != "")
                        {
                            ApplicProcessHistoryInfo Applic = new ApplicProcessHistoryInfo();
                            Applic.CreateDate = ((DateTime)r["Sysdate"]).ToString("yyyyMMddHHmmss");
                            Applic.Description = "";
                            Applic.DisplayName = "";
                            //Applic.PreStatus = r["PreStatusID"] == DBNull.Value ? "" : r["PreStatusID"].ToString().Trim();
                            //Applic.PreUserProcess = r["PreUsrProcess"] == DBNull.Value ? "" : r["PreUsrProcess"].ToString().Trim();
                            //Applic.ProcessId = r["ProcessID"] == DBNull.Value ? "" : r["ProcessID"].ToString().Trim();
                            //Applic.Status = r["StatusID"] == DBNull.Value ? "" : r["StatusID"].ToString().Trim();
                            //Applic.UserCreate = r["UserCreate"] == DBNull.Value ? "" : r["UserCreate"].ToString().Trim();
                            Applic.ActionDesc = string.Format("Chờ {0} xử lý hồ sơ", r["DisplayNameOfNextUsrProcess"].ToString().Trim());
                            appList.Insert(0, Applic);
                        }
                        break;
                    }                    
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return appList;
        }

        public int DeleteFile(string ApplicId, string UserName, string UserType, string Brcd, string FileName)
        {
            int r = -1;
            try
            {
                SqlCommand cmd = new SqlCommand("[DeleteFile]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@FileName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, FileName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ErrorCode", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output));

                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                r = cmd.ExecuteNonQuery();
                r = (int)cmd.Parameters["@ErrorCode"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

        public int ChangeApplicInfo(string ApplicId, string UserName, string UserType, string Brcd, string Priority, int CustId, string CustName, string LegalId,
            string LegalType, string Description, decimal CreditLimit,string PhoneNo)
        {
            int r = -1;
            try
            {
                SqlCommand cmd = new SqlCommand("[ChangeApplicInfo]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Priority", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Priority));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@CustId", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Input, CustId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@CustName", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, Utilities.RemoveUnicodeAndSpace(CustName).ToUpper()));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@SearchCustName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Utilities.RemoveUnicodeAndSpace(CustName)));

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@LegalID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, LegalId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@LegalType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, LegalType));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@Description", System.Data.SqlDbType.NVarChar, System.Data.ParameterDirection.Input, Description));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@CreditLimit", System.Data.SqlDbType.Money, System.Data.ParameterDirection.Input, CreditLimit));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@PhoneNo", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, PhoneNo));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ErrorCode", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output));

                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                r = cmd.ExecuteNonQuery();
                r = (int)cmd.Parameters["@ErrorCode"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }
        public int DownloadFile(string ApplicId, string UserName, string FileName, ref string FileData)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("[PortalModule].[dbo].[VSK_SP_DownloadFile]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@FileName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, FileName));
                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                //r = cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                foreach(DataRow r in ds.Tables[0].Rows)
                {
                    FileData = r["FileData"] == DBNull.Value ? null : r["FileData"].ToString();
                    break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }
        private string CreateActionDescription(string DisplayName, string Status, string PreStatus)
        {
            string msg = "";
            try
            {
                switch (Status)
                {
                    //case "0":
                    //    if (PreStatus == "")
                    //        msg = string.Format("{0} tạo hồ sơ.", DisplayName);
                    //    else
                    //        msg = string.Format("{0} cập nhật hồ sơ.", DisplayName);
                    //    break;
                    case "0":
                        msg = string.Format("{0} tạo hồ sơ.", DisplayName);
                        break;
                    case "0A":
                        msg = string.Format("{0} cập nhật hồ sơ.", DisplayName);
                        break;
                    case "1":
                        msg = string.Format("{0} trình hồ sơ.", DisplayName);
                        break;
                    case "3":
                        msg = string.Format("{0} trả lại hồ sơ.", DisplayName);
                        break;
                    case "4":
                        msg = string.Format("{0} rút hồ sơ.", DisplayName);
                        break;
                    case "5":
                        msg = string.Format("{0} đóng hồ sơ.", DisplayName);
                        break;
                    case "6":
                        msg = string.Format("{0} từ chối hồ sơ.", DisplayName);
                        break;
                    case "7":
                        msg = string.Format("{0} duyệt hồ sơ.", DisplayName);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ;
            }
            return msg;
        }

        //public List<ApplicInfo> GetListApplicHasChangedByUser(string UserName, string UserType, string Brcd)
        //{
        //    List<ApplicInfo> appList = new List<ApplicInfo>();
        //    SqlDataAdapter da;
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("[GetListApplicHasChangedByUser]", dbVS.Connection);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.CommandTimeout = AppSettings.DatabaseTimeout;
        //        cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
        //        cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
        //        cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
        //        if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
        //        {
        //            dbVS.Connection.Open();
        //        }
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        if (ds.Tables.Count > 0)
        //        {
        //            foreach (DataRow r in ds.Tables[0].Rows)
        //            {
        //                ApplicInfo Applic = new ApplicInfo();
        //                Applic.ApplicId = r["ApplicID"] == DBNull.Value ? "" : r["ApplicID"].ToString().Trim();
        //                Applic.Brcd = r["BranchID"] == DBNull.Value ? "" : r["BranchID"].ToString().Trim();
        //                Applic.CreditLimit = r["CreditLimit"] == DBNull.Value ? 0 : decimal.Parse(r["CreditLimit"].ToString());
        //                Applic.CustId = r["CustID"] == DBNull.Value ? "" : r["CustID"].ToString();
        //                Applic.CustName = r["CustName"] == DBNull.Value ? "" : r["CustName"].ToString().ToString();
        //                Applic.DateCreate = r["DateCreate"] == DBNull.Value ? "" : Utilities.FormatDate2String(r["DateCreate"].ToString(), "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss");
        //                Applic.DateModif = r["DateModif"] == DBNull.Value ? "" : Utilities.FormatDate2String(r["DateModif"].ToString(), "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss");
        //                Applic.Description = r["Description"] == DBNull.Value ? "" : r["Description"].ToString().Trim();
        //                Applic.GroupFileId = r["GroupFileID"] == DBNull.Value ? -1 : int.Parse(r["GroupFileID"].ToString());
        //                Applic.LegalId = r["LegalId"] == DBNull.Value ? "" : r["LegalId"].ToString().Trim();
        //                Applic.LegalType = r["LegalType"] == DBNull.Value ? "" : r["LegalType"].ToString().Trim();
        //                Applic.NextUserProcess = r["NextUsrProcess"] == DBNull.Value ? "" : r["NextUsrProcess"].ToString().Trim();
        //                Applic.NextUserTypeProcess = r["NextUsrTypeProcess"] == DBNull.Value ? "" : r["NextUsrTypeProcess"].ToString().Trim();
        //                Applic.Priority = r["Priority"] == DBNull.Value ? "" : r["Priority"].ToString().Trim();
        //                Applic.ProcessId = r["ProcessId"] == DBNull.Value ? "" : r["ProcessId"].ToString().Trim();
        //                Applic.Status = r["StatusID"] == DBNull.Value ? "" : r["StatusID"].ToString().Trim();
        //                Applic.UserCreate = r["UserCreate"] == DBNull.Value ? "" : r["UserCreate"].ToString().Trim();
        //                Applic.UserModif = r["UserModif"] == DBNull.Value ? "" : r["UserModif"].ToString().Trim();
        //                Applic.UserProcess = r["UsrProcess"] == DBNull.Value ? "" : r["UsrProcess"].ToString().Trim();
        //                Applic.UserTypeProcess = r["UsrTypeProcess"] == DBNull.Value ? "" : r["UsrTypeProcess"].ToString().Trim();
        //                Applic.PolicyId = r["PolicyID"] == DBNull.Value ? "" : r["PolicyID"].ToString().Trim();

        //                Applic.LegalName = r["LegalName"] == DBNull.Value ? "" : r["LegalName"].ToString().Trim();
        //                Applic.ProcessName = r["ProcessName"] == DBNull.Value ? "" : r["ProcessName"].ToString().Trim();
        //                Applic.PolicyName = r["PolicyName"] == DBNull.Value ? "" : r["PolicyName"].ToString().Trim();

        //                Applic.PreApprFlag = r["PreApprFlag"] == DBNull.Value ? "" : r["PreApprFlag"].ToString().Trim();
        //                Applic.UserCreateType = r["UserCreateType"] == DBNull.Value ? "" : r["UserCreateType"].ToString().Trim();
        //                Applic.DisplayName = r["DisplayName"] == DBNull.Value ? "" : r["DisplayName"].ToString().Trim();

        //                Applic.OriUserProcess = r["OriUsrProcess"] == DBNull.Value ? "" : r["OriUsrProcess"].ToString().Trim();
        //                Applic.OriUserTypeProcess = r["OriUsrTypeProcess"] == DBNull.Value ? "" : r["OriUsrTypeProcess"].ToString().Trim();
        //                Applic.OriDisplayName = r["OriDisplayName"] == DBNull.Value ? "" : r["OriDisplayName"].ToString().Trim();

        //                if (Applic.ProcessId == Constants.HSCN || Applic.ProcessId == Constants.PRE)
        //                {
        //                    appList.Add(Applic);
        //                }
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return appList;
        //}

        //public int RemoveHasChanged(string UserName, string UserType, string Brcd, string ApplicId)
        //{
        //    int r = -1;
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("[RemoveHasChanged]", dbVS.Connection);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.CommandTimeout = AppSettings.DatabaseTimeout;

        //        cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
        //        cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
        //        cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
        //        cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
        //        cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ErrorCode", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output));

        //        if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
        //        {
        //            dbVS.Connection.Open();
        //        }
        //        r = cmd.ExecuteNonQuery();
        //        r = (int)cmd.Parameters["@ErrorCode"].Value;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return r;
        //}

        public int IsCustomerHasApplicWaitProcess(string UserName, string UserType, string Brcd, string LegalId)
        {
            int r = -1;
            try
            {
                SqlCommand cmd = new SqlCommand("[IsCustomerHasApplicWaitProcess]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;

                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@LegalID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, LegalId));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameterWithSize("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Output, 50));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameterWithSize("@Exists", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Output, 1));

                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                r = cmd.ExecuteNonQuery();
                r = int.Parse((string)cmd.Parameters["@Exists"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

        //public int RemoveHasChanged(string UserName, string UserType, string Brcd, List<string> ApplicList)
        //{
        //    int r;
        //    int total = 0;
        //    try
        //    {
        //        foreach(string applicId in ApplicList)
        //        {
        //            r = RemoveHasChanged(UserName, UserType, Brcd, applicId);
        //            if (r == 0)
        //            {
        //                total++;
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        LogManager.LogError(ex);
        //    }
        //    return total;
        //}
        public int AttachDocCommit(string ApplicId, string UserName, string UserType, string Brcd)
        {
            int r = -1;
            try
            {
                SqlCommand cmd = new SqlCommand("[AttachDocCommit]", dbVS.Connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = DBEnum.CommandTimeout;
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserName", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserName));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@UserType", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, UserType));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@BranchID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, Brcd));
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ApplicID", System.Data.SqlDbType.VarChar, System.Data.ParameterDirection.Input, ApplicId));
                
                cmd.Parameters.Add(new SQLParameterCtr().CreateParameter("@ErrorCode", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output));

                if (dbVS.Connection.State == System.Data.ConnectionState.Closed)
                {
                    dbVS.Connection.Open();
                }
                r = cmd.ExecuteNonQuery();
                r = (int)cmd.Parameters["@ErrorCode"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }
    }
}
