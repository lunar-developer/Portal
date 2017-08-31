using System;

namespace Modules.VSaleKit.DataTransfer
{
    [Serializable]
    public class PolicyDocumentData
    {
        public string PolicyID { get; set; }
        public string DocumentTypeID { get; set; }
        public string IsRequire { get; set; }
        public string OrderNo { get; set; }
        public string ModifyUserID { get; set; }
        public string ModifyDateTime { get; set; }
    }
}