using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.UserManagement.Business
{
    public static class RoleTemplateBusiness
    {
        public static DataTable GetRoleTemplate(string branchID)
        {
            return new RoleTemplateProvider().GetRoleTemplate(branchID);
        }

        public static DataSet GetRoleTemplateDetail(string templateID)
        {
            return new RoleTemplateProvider().GetRoleTemplateDetail(templateID);
        }

        public static int UpdateRoleTemplate(Dictionary<string, SQLParameterData> parametediDictionary)
        {
            return new RoleTemplateProvider().UpdateRoleTemplate(parametediDictionary);
        }

        public static bool DeleteRoleTemplate(string templateID)
        {
            return new RoleTemplateProvider().DeleteRoleTemplate(templateID);
        }
    }
}