using System.Runtime.Serialization;

namespace Website.Library.DataTransfer
{
    [DataContract(Namespace = "")]
    public sealed class RequestData
    {
        [DataMember]
        public string Data { get; set; }

        [DataMember]
        public string Function { get; set; }

        [DataMember]
        public string RequestDateTime { get; set; }

        [DataMember]
        public string RequestID { get; set; }
    }
}