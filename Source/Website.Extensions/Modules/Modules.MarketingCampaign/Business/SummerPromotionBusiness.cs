using System.Collections.Generic;
using System.Data;
using Modules.MarketingCampaign.DataAccess;
using Modules.MarketingCampaign.DataTransfer;
using Modules.MarketingCampaign.Enum;

namespace Modules.MarketingCampaign.Business
{
    public class SummerPromotionBusiness
    {
        public static bool InsertResult(List<SummerPromotionData> rsList, out string message)
        {
            return new SummerPromotionProvider().InsertResult(rsList, out message);
        }

        public static DataTable LoadResult(int top = 10, string reportType = ReportTypeEnum.Session)
        {
            return new SummerPromotionProvider().LoadResult(top,reportType);
        }

        public static List<SummerPromotionData> SearchResult(string branchCode)
        {
            return new SummerPromotionProvider().SearchResult(branchCode);
        }
    }
}
