using System.Collections.Generic;
using System.Data;
using Modules.VSaleKit.DataAccess;
using Modules.VSaleKit.DataTransfer;

namespace Modules.VSaleKit.Business
{
    public static class BatchDataBusiness
    {
        public static DataTable Insert(List<BatchData> listData, Dictionary<string, string> dictionary)
        {
            return new BatchDataProvider().Insert(listData, dictionary);
        }
    }
}