using System.Collections.Generic;
using System.Data;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using Website.Library.Global;

namespace Modules.Application.Business
{
    public static class ApplicationBusiness
    {
        public static long InsertApplication(int userID, Dictionary<string, string> fieldDictionary)
        {
            List<FieldData> listApplication = new List<FieldData>();
            List<FieldData> listInteger = new List<FieldData>();
            List<FieldData> listBigInteger = new List<FieldData>();
            List<FieldData> listString = new List<FieldData>();
            List<FieldData> listUnicodeString = new List<FieldData>();
            List<FieldData> listLog = new List<FieldData>();
            BindData(fieldDictionary, listApplication, listInteger, listBigInteger,
                listString, listUnicodeString, listLog);
            if (listApplication.Count == 0)
            {
                return 0;
            }

            DataTable dtResult = new ApplicationProvider().InsertApplication(userID,
                listApplication, listInteger, listBigInteger, listString, listUnicodeString, listLog);
            long result = long.Parse(dtResult.Rows[0][0].ToString());
            if (result > 0)
            {
                return result;
            }

            string message = dtResult.Rows[0][1].ToString();
            FunctionBase.LogError(message);
            return result;
        }

        public static long UpdateApplication(int userID, string applicationID,
            Dictionary<string, string> fieldDictionary)
        {
            List<FieldData> listApplication = new List<FieldData>();
            List<FieldData> listInteger = new List<FieldData>();
            List<FieldData> listBigInteger = new List<FieldData>();
            List<FieldData> listString = new List<FieldData>();
            List<FieldData> listUnicodeString = new List<FieldData>();
            List<FieldData> listLog = new List<FieldData>();
            BindData(fieldDictionary, listApplication, listInteger, listBigInteger,
                listString, listUnicodeString, listLog);
            if (listApplication.Count == 0)
            {
                return 0;
            }

            DataTable dtResult = new ApplicationProvider().UpdateApplication(userID, applicationID,
                listApplication, listInteger, listBigInteger, listString, listUnicodeString, listLog);
            long result = long.Parse(dtResult.Rows[0][0].ToString());
            if (result > 0)
            {
                return result;
            }

            string message = dtResult.Rows[0][1].ToString();
            FunctionBase.LogError(message);
            return result;
        }

        public static DataSet LoadApplication(string applicationID, int userID)
        {
            return new ApplicationProvider().LoadApplication(applicationID, userID);
        }

        public static DataTable SearchApplication(Dictionary<string, string> conditionDictionary)
        {
            return new ApplicationProvider().SearchApplication(conditionDictionary);
        }

        private static void BindData(IReadOnlyDictionary<string, string> fieldDictionary,
            ICollection<FieldData> listApplication,
            ICollection<FieldData> listInteger,
            ICollection<FieldData> listBigInteger,
            ICollection<FieldData> listString,
            ICollection<FieldData> listUnicodeString,
            ICollection<FieldData> listLog)
        {
            List<ApplicationFieldData> listField = CacheBase.Receive<ApplicationFieldData>();
            foreach (ApplicationFieldData fieldInfo in listField)
            {
                if (fieldDictionary.ContainsKey(fieldInfo.FieldName) == false)
                {
                    continue;
                }

                FieldData fieldData = new FieldData
                {
                    FieldName = fieldInfo.FieldName,
                    FieldValue = PrebuildData(fieldDictionary[fieldInfo.FieldName], fieldInfo.DataType)
                };
                switch (fieldInfo.TableName)
                {
                    case "APP_ApplicationFieldInteger":
                        listInteger.Add(fieldData);
                        break;

                    case "APP_ApplicationFieldBigInteger":
                        listBigInteger.Add(fieldData);
                        break;

                    case "APP_ApplicationFieldString":
                        listString.Add(fieldData);
                        break;

                    case "APP_ApplicationFieldUnicodeString":
                        listUnicodeString.Add(fieldData);
                        break;

                    default:
                        listApplication.Add(fieldData);
                        break;
                }

                FieldData logData = new FieldData()
                {
                    FieldName = fieldInfo.FieldName,
                    FieldValue = PrebuildData(fieldDictionary[fieldInfo.FieldName], "nvarchar")
                };
                listLog.Add(logData);
            }
        }

        private static string PrebuildData(string value, string dataType)
        {
            switch (dataType.ToLower())
            {
                case "varchar":
                    return $"'{value}'";

                case "nvarchar":
                    return $"N'{value}'";

                default:
                    return value;
            }
        }
    }
}