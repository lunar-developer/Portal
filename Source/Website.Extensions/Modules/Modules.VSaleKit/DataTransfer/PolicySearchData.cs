using System;

namespace Modules.VSaleKit.DataTransfer
{
    [Serializable]
    public class PolicySearchData
    {
        public string ID { get; set; }
        public string PolicyID { get; set; }
        public string Keyword { get; set; }
    }
}