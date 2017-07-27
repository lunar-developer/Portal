using System.Runtime.Serialization;

namespace Website.Library.DataTransfer
{
    [DataContract(Namespace = "")]
    public sealed class ResponseData
    {
        [DataMember]
        public string Data { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string RequestID { get; set; }

        [DataMember]
        public string ResponseCode { get; set; }
    }
}