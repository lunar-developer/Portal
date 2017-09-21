using System.Collections.Generic;
using System.Data;
using Modules.Application.DataAccess;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.Database;
using Website.Library.Global;

namespace Modules.Application.Business
{
    public static class ApplicationLogBusiness
    {
        public static readonly string Yes = "YES";
        public static readonly string No = "NO";
        public static Dictionary<string, string> GetVersion(string applicationID, string baseVersion = null)
        {
            Dictionary<string, string> versions = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(applicationID))
            {
                return versions;
            }

            DataTable dataTable =  new ApplicationLogProvider().GetVersion(applicationID, baseVersion);
            if (null == dataTable)
            {
                return versions;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                string time = row[BaseTable.DateTimeModify].ToString();
                versions.Add(row[ApplicationLogTable.ApplicationLogID].ToString(), FunctionBase.FormatDate(time));
            }
            return versions;
        }

        public static Dictionary<string, ApplicationLogDetailDiffData> GetChangeOfTwoVersions(string appId, string lhs, string rhs, string fieldName = null)
        {
            DataTable dataTable = new ApplicationLogProvider().GetChangeOfTwoVersions(appId, lhs, rhs, fieldName);

            Dictionary<string, ApplicationLogDetailDiffData> dictionary = new Dictionary<string, ApplicationLogDetailDiffData>();
            if (null == dataTable)
            {
                return dictionary;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                string id = row[ApplicationLogTable.ApplicationLogID].ToString().Trim();
                string field = row[ApplicationFieldTable.FieldName].ToString();
                string value = row[ApplicationFieldTable.FieldValue].ToString();

                if (!dictionary.ContainsKey(field))
                {
                    string valueLhs = string.Empty;
                    string valueRhs = string.Empty;
                    if (id.Equals(lhs))
                    {
                        valueLhs = value;
                        if (string.IsNullOrEmpty(rhs))
                        {
                            valueRhs = "Empty";
                        }
                    }
                    if (id.Equals(rhs))
                    {
                        valueRhs = value;
                    }
                    ApplicationLogDetailDiffData item = new ApplicationLogDetailDiffData
                    {
                        ApplicationLogID = id,
                        FieldName = field,
                        FieldValueLhs = valueLhs,
                        FieldValueRhs = valueRhs,
                        Diff = Yes
                    };
                    dictionary.Add(field, item);
                }
                else
                {
                    ApplicationLogDetailDiffData item = dictionary[field];
                    if (id.Equals(lhs))
                    {
                        item.FieldValueLhs = value;
                        if (value.Equals(item.FieldValueRhs))
                        {
                            item.Diff = No;
                        }
                    }

                    if (!id.Equals(rhs)) continue;

                    item.FieldValueRhs = value;
                    if (value.Equals(item.FieldValueLhs))
                    {
                        item.Diff = No;
                    }
                }
            }

            return dictionary;
        }
    }
}
