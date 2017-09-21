using Website.Library.DataTransfer;

namespace Modules.Forex.DataTransfer
{
    public class CurrencyRateData:CacheData
    {
        public string CurrencyCode;
        /// <summary>
        /// Rate (VND)
        /// </summary>
        public string Rate;

        public string MasterRate;
        public string PreviousRate;
        /// <summary>
        /// Margin: Limit min Profit for Branch
        /// </summary>
        public string MarginMinProfit;
        /// <summary>
        /// Limit NHNN
        /// </summary>
        public string MarginLimit;

        public string IsDisable;
    }
}
