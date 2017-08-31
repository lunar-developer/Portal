using Modules.VSaleKit.DataAccess;
using Modules.VSaleKit.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.VSaleKit.Business
{
    public class ApplicController
    {
        private readonly ApplicProvider iProvider = new ApplicProvider();

        public ApplicController()
        {
            ;
        }

        public string GetApplicId(string username, string brcd)
        {
            try
            {
                return iProvider.GetApplicId(username, brcd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpLoadFile(string UserName, string UserType, string ApplicId, string PolicyId, string DocId, int FileNumber, string FileName, string FileData,
           string FileExt, int FileSize, string Status, string Description, string FilePath)
        {
            try
            {
                return iProvider.UpLoadFile(UserName, UserType, ApplicId, PolicyId, DocId, FileNumber, FileName, FileData, FileExt, FileSize, Status, Description, FilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateApplic(string UserName, string Brcd, string UserType, string ApplicId, string ProcessId, string Priority, string Status,
          string CustName, string LegalId, string LegalType, string Description, decimal CreditLimit, string PolicyId, string phoneNo)
        {
            try
            {
                return iProvider.CreateApplic(UserName, Brcd, UserType, ApplicId, ProcessId, Priority, Status, CustName, LegalId, LegalType, Description, CreditLimit, PolicyId,phoneNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetApplicInfo(string ApplicId, string UserName, string UserType, string Brcd, ref ApplicInfo Applic, ref List<FileData> FileList)
        {
            try
            {
                return iProvider.GetApplicInfo(ApplicId, UserName, UserType, Brcd, ref Applic, ref FileList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApplicInfo> GetListApplicByUser(string UserName, string FromDate, string ToDate)
        {
            try
            {
                return iProvider.GetListApplicByUser(UserName, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApplicInfo> GetListApplicProcessedByUser(string UserName, string FromDate, string ToDate)
        {
            try
            {
                return iProvider.GetListApplicProcessedByUser(UserName, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ApplicInfo> GetListApplicWaitApproval(string UserName, string UserType, string Brcd)
        {
            try
            {
                return iProvider.GetListApplicWaitApproval(UserName, UserType, Brcd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Approval(string ApplicId, string UserName, string UserType, string Brcd, string Description)
        {
            try
            {
                return iProvider.Approval(ApplicId, UserName, UserType, Brcd, Description);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ReturnApplic(string ApplicId, string UserName, string UserType, string Brcd, string Description)
        {
            try
            {
                return iProvider.ReturnApplic(ApplicId, UserName, UserType, Brcd, Description);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RecallApplic(string ApplicId, string UserName, string UserType, string Brcd, string Description)
        {
            try
            {
                return iProvider.RecallApplic(ApplicId, UserName, UserType, Brcd, Description);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int CloseApplic(string ApplicId, string UserName, string UserType, string Brcd, string Description)
        {
            try
            {
                return iProvider.CloseApplic(ApplicId, UserName, UserType, Brcd, Description);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ApplicProcessHistoryInfo> GetApplicProcessHistory(string ApplicId, string UserName)
        {
            try
            {
                return iProvider.GetApplicProcessHistory(ApplicId, UserName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteFile(string ApplicId, string UserName, string UserType, string Brcd, string FileName)
        {
            try
            {
                return iProvider.DeleteFile(ApplicId, UserName, UserType, Brcd, FileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ChangeApplicInfo(string ApplicId, string UserName, string UserType, string Brcd, string Priority, int CustId, string CustName, string LegalId,
           string LegalType, string Description, decimal CreditLimit, string PhoneNo)
        {
            try
            {
                return iProvider.ChangeApplicInfo(ApplicId, UserName, UserType, Brcd, Priority, CustId, CustName, LegalId, LegalType, Description, CreditLimit,PhoneNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DownloadFile(string ApplicId, string UserName,  string FileName, ref string FileData)
        {
            try
            {
                return iProvider.DownloadFile(ApplicId, UserName, FileName, ref FileData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApplicInfo> SearchApplic(string userID, string fromDate, string toDate, string customerInfo, string brcd)
        {
            try
            {
                return iProvider.SearchApplic(userID, fromDate, toDate, customerInfo,brcd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<ApplicInfo> GetListApplicHasChangedByUser(string UserName, string UserType, string Brcd)
        //{
        //    try
        //    {
        //        return iProvider.GetListApplicHasChangedByUser(UserName, UserType, Brcd);
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int RemoveHasChanged(string UserName, string UserType, string Brcd, List<string> ApplicList)
        //{
        //    try
        //    {
        //        return iProvider.RemoveHasChanged(UserName, UserType, Brcd, ApplicList);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public int IsCustomerHasApplicWaitProcess(string UserName, string UserType, string Brcd, string LegalId)
        {
            try
            {
                return iProvider.IsCustomerHasApplicWaitProcess(UserName, UserType, Brcd, LegalId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetListFileByApplicId(string applicID, ref List<FileData> fileLst)
        {
            try
            {
                return iProvider.GetListFileByApplicId(applicID, ref fileLst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AttachDocCommit(string ApplicId, string UserName, string UserType, string Brcd)
        {
            try
            {
                return iProvider.AttachDocCommit(ApplicId, UserName, UserType, Brcd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            if (iProvider != null)
            {
                iProvider.Dispose();
            }
        }
    }
}
