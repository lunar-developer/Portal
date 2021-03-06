﻿using System.Collections.Generic;
using System.Data;
using Modules.EmployeeManagement.DataAccess;
using Modules.EmployeeManagement.DataTransfer;
using Modules.EmployeeManagement.Enum;
using Website.Library.DataTransfer;

namespace Modules.EmployeeManagement.Business
{
    public static class EmployeeBusiness
    {
        public static bool InsertEmployee(List<EmployeeData> listEmployeeData, Dictionary<string, string> dictionary,
            out string message)
        {
            bool result = new EmployeeProvider().InsertEmployee(listEmployeeData, dictionary);
            message = result ? MessageDefinitionEnum.FileImportSuccess : MessageDefinitionEnum.FileImportFail;
            return result;
        }

        public static DataSet SearchEmployee(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            return new EmployeeProvider().SearchEmployee(parameterDictionary);
        }

        public static DataTable GetAllEmployeeEmail()
        {
            return new EmployeeProvider().GetAllEmployeeEmail();
        }

        public static DataTable LoadEmployeeInfo(string email)
        {
            return new EmployeeProvider().LoadEmployeeInfo(email);
        }

        public static bool UpdateEmployeeInfo(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            return new EmployeeProvider().UpdateEmployeeInfo(parameterDictionary);
        }

        public static bool UpdateEmployeeImage(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            return new EmployeeProvider().UpdateEmployeeImage(parameterDictionary);
        }

        public static bool UpdateEmployeeQRCode(Dictionary<string, string> parameterDictionary)
        {
            return new EmployeeProvider().UpdateEmployeeContactQRCode(parameterDictionary);
        }

        public static DataTable GetEmployeeEmail(string email)
        {
            return new EmployeeProvider().GetEmployeeEmail(email);
        }

        public static List<EmployeeBranchData> LoadEmployeeBranch()
        {
            return new EmployeeProvider().LoadEmployeeBranch();
        }
    }
}