using System;
using Website.Library.DataTransfer;
using Website.Library.Enum;

namespace Website.Library.Global
{
    public abstract class MessageQueueBase
    {
        public abstract bool SendToQueue(string queueName, string message, string contentType = ContentEnum.Json);

        public abstract void ReceiveFromQueue(string queueName, Action<string, string> callback, string clientName = "Portal");

        public abstract MessageQueueBase Clone();


        public static string BuildMessage(
            string functionName, 
            string data,
            string contentType = ContentEnum.Json,
            string requestID = null)
        {
            RequestData requestData = new RequestData
            {
                RequestID = string.IsNullOrWhiteSpace(requestID)
                    ? Guid.NewGuid().ToString(PatternEnum.GuidDigits)
                    : requestID,
                RequestDateTime = DateTime.UtcNow.ToString(PatternEnum.DateTimeUniversal),
                Function = functionName,
                Data = data
            };

            return FunctionBase.Serialize(requestData, contentType);
        }
    }
}