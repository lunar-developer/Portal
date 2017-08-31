using System.Collections.Generic;
using System.Data;
using Modules.VSaleKit.DataAccess;
using Modules.VSaleKit.DataTransfer;
using Website.Library.DataTransfer;

namespace Modules.VSaleKit.Business
{
    public static class ApplicationFormBusiness
    {
        public static DataTable SearchApplication(Dictionary<string, SQLParameterData> dictionary)
        {
            return new ApplicationFormProvider().SearchApplication(dictionary);
        }

        public static DataSet LoadApplication(string uniqueID, int userID, string roleName)
        {
            return new ApplicationFormProvider().LoadApplication(uniqueID, userID, roleName);
        }

        public static DataTable LoadAttachFiles(string uniqueID, string documentCode = "")
        {
            return new ApplicationFormProvider().LoadAttachFiles(uniqueID, documentCode);
        }

        public static DataTable LoadLogHistory(string uniqueID)
        {
            return new ApplicationFormProvider().LoadLogHistory(uniqueID);
        }

        public static int InsertApplication(Dictionary<string, SQLParameterData> dataDictionary, out string uniqueID)
        {
            return new ApplicationFormProvider().InsertApplication(dataDictionary, out uniqueID);
        }

        public static int UpdateApplication(Dictionary<string, SQLParameterData> dataDictionary, out string uniqueID)
        {
            return new ApplicationFormProvider().UpdateApplication(dataDictionary, out uniqueID);
        }

        public static int ProcessApplication(Dictionary<string, SQLParameterData> dataDictionary)
        {
            return new ApplicationFormProvider().ProcessApplication(dataDictionary);
        }

        public static int AttachFiles(Dictionary<string, SQLParameterData> dataDictionary, List<FileData> listFiles)
        {
            return new ApplicationFormProvider().AttachFiles(dataDictionary, listFiles);
        }

        public static bool DeleteFile(string userName, string fileID)
        {
            return new ApplicationFormProvider().DeleteFile(userName, fileID);
        }

        public static bool SortFiles(string uniqueID, string documentCode, Dictionary<int, int> dataDictionary)
        {
            return new ApplicationFormProvider().SortFiles(uniqueID, documentCode, dataDictionary);
        }

        public static int GetCurrentFileNumber(Dictionary<string, SQLParameterData> dataDictionary)
        {
            return new ApplicationFormProvider().GetCurrentFileNumber(dataDictionary);
        }
    }
}