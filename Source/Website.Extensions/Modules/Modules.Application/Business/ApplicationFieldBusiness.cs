using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class ApplicationFieldBusiness
    {
        public static List<ApplicationFieldData> GetAllFields()
        {
            return new ApplicationFieldProvider().GetAllFields();
        }

        public static ApplicationFieldData GetField(string fieldName)
        {
            return new ApplicationFieldProvider().GetField(fieldName);
        }
    }
}