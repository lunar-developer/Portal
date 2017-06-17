using System.Collections.Generic;
using System.Data;
using Modules.EmployeeManagement.DataAccess;
using Modules.EmployeeManagement.DataTransfer;
using Modules.EmployeeManagement.Enum;
using Website.Library.DataTransfer;

namespace Modules.EmployeeManagement.Business
{
    public static class EmployeeBusiness
    {
        public static bool InsertEmployee(List<EmployeeData> listEmployeeData, Dictionary<string,string> dictionary,
            out string message)
        {
            bool result = new EmployeeProvider().InsertEmployee(listEmployeeData, dictionary);
            message = result ? MessageDefinitionEnum.FileImportSuccess : MessageDefinitionEnum.FileImportFail;
            return result;
        }

        public static DataTable SearchEmployee(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            return new EmployeeProvider().SearchEmployee(parameterDictionary);
        }
    }
}