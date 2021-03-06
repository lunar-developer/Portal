﻿using System.Collections.Generic;
using System.Data;
using Modules.MarketingCampaign.DataAccess;
using Modules.MarketingCampaign.DataTransfer;

namespace Modules.MarketingCampaign.Business
{
    public static class ResultBusiness
    {
        public static DataTable LoadResult(int topRanking, string reportType)
        {
            return new ResultProvider().LoadResult(topRanking, reportType);
        }

        public static List<ResultData> SearchResult(string staffID)
        {
            return new ResultProvider().SearchResult(staffID);
        }

        public static bool InsertResult(List<ResultData> listResult, out string message)
        {
            return new ResultProvider().InsertResult(listResult, out message);
        }
    }
}