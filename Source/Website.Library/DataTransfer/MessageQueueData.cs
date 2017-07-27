using System;
using System.Threading;
using Website.Library.Business;
using Website.Library.Enum;

namespace Website.Library.DataTransfer
{
    public class MessageQueueData
    {
        public AutoResetEvent AutoEvent = new AutoResetEvent(false);
        public readonly string RequestID;
        public string RequestData;
        public string ResponseData;
        public string ResponseCode;
        public long ExpireTime;


        public MessageQueueData(
            string functionName,
            string data,
            int timeout,
            string contentType = ContentEnum.Json,
            string requestID = null)
        {
            if (timeout <= 0)
            {
                timeout = MessageQueueBusiness.DefaultInterval;
            }

            RequestID = string.IsNullOrWhiteSpace(requestID)
                ? Guid.NewGuid().ToString(PatternEnum.GuidDigits)
                : requestID;
            RequestData = MessageQueueBusiness.BuildMessage(functionName, data, contentType, RequestID);
            ExpireTime = long.Parse(DateTime.Now.AddSeconds(timeout).ToString(PatternEnum.DateTime));
        }
    }
}