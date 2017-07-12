using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class EducationBusiness
    {
        public static List<EducationData> GetList()
        {
            return new EducationProvider().GetList();
        }

        public static EducationData GetItem(string key)
        {
            return new EducationProvider().GetItem(key);
        }
    }
}