using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using RabbitMQ.Client;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace Website.Library.Business
{
    public static class MessageQueueBusiness
    {
        public const int DefaultInterval = 5;
        internal static Timer ScheduleTimer;

        private static readonly ConcurrentDictionary<string, MessageQueueData> MessageDictionary =
            new ConcurrentDictionary<string, MessageQueueData>();
        private static readonly Dictionary<string, string> ConsumerDictionary = new Dictionary<string, string>();
        private static readonly Dictionary<string, MessageQueueBase> HandlerDictionary;


        static MessageQueueBusiness()
        {
            // Inject Hanlders
            HandlerDictionary = new Dictionary<string, MessageQueueBase>
            {
                {
                    MessageQueueEnum.Rabbit, new RabbitMessageQueueBase(new ConnectionFactory
                    {
                        HostName = FunctionBase.GetConfiguration(ConfigEnum.QueueRabbitServiceHostName),
                        Port = FunctionBase.ConvertToInteger(
                            FunctionBase.GetConfiguration(ConfigEnum.QueueRabbitServicePort)),
                        VirtualHost = FunctionBase.GetConfiguration(ConfigEnum.QueueRabbitServiceVirtualHost),
                        UserName = FunctionBase.GetConfiguration(ConfigEnum.QueueRabbitServiceUserName),
                        Password = FunctionBase.GetConfiguration(ConfigEnum.QueueRabbitServicePassword),
                        RequestedHeartbeat = ushort.Parse(FunctionBase.GetConfiguration(ConfigEnum.QueueRabbitServiceHeartbeat, "30"))
                    })
                }
            };

            // Inject Consumer
            InjectConsumer(ConfigurationBase.QueuePortalServiceOut, ProcessOnQueueReceive, "PortalService.Out");

            // Define Schedule Timer
            int interval = FunctionBase.ConvertToInteger(
                FunctionBase.GetConfiguration(ConfigEnum.MessageQueueSchedule), DefaultInterval);
            ScheduleTimer = new Timer
            {
                Interval = interval * UnitEnum.Second,
                AutoReset = true
            };
            ScheduleTimer.Elapsed += ProcessOnTimerElapsed;
        }


        private static void ProcessOnQueueReceive(string data, string contentType)
        {
            ResponseData responseData = FunctionBase.Deserialize<ResponseData>(data, contentType);
            if (responseData == null
                || MessageDictionary.TryRemove(responseData.RequestID, out MessageQueueData messageData) == false)
            {
                return;
            }

            messageData.ResponseCode = responseData.ResponseCode;
            messageData.ResponseData = responseData.ResponseCode == ResponseEnum.Success
                ? responseData.Data
                : responseData.Description;
            messageData.AutoEvent.Set();
        }

        private static void ProcessOnTimerElapsed(object sender, ElapsedEventArgs args)
        {
            if (MessageDictionary.Count == 0)
            {
                ScheduleTimer.Stop();
            }

            // Process all expired items
            long currentTime = long.Parse(DateTime.Now.ToString(PatternEnum.DateTime));
            List<string> listExpireItems = MessageDictionary
                .Where(item => item.Value.ExpireTime <= currentTime)
                .Select(item => item.Key).ToList();
            foreach (string key in listExpireItems)
            {
                if (MessageDictionary.TryRemove(key, out MessageQueueData item) == false)
                {
                    continue;
                }

                item.ResponseData = ResponseEnum.GetDescription(ResponseEnum.Timeout);
                item.ResponseCode = ResponseEnum.Timeout;
                item.AutoEvent.Set();
            }
        }


        public static void Initialize() {}

        public static bool InjectConsumer(
            string queueName,
            Action<string, string> callback,
            string clientName = "Portal",
            string queueHandler = MessageQueueEnum.Rabbit)
        {
            try
            {
                MessageQueueBase messageQueue = GetQueueHandler(queueHandler);
                if (messageQueue == null || ConsumerDictionary.ContainsKey(queueName))
                {
                    return false;
                }

                MessageQueueBase instance = messageQueue.Clone();
                ConsumerDictionary.Add(queueName, callback.GetType().FullName);
                instance.ReceiveFromQueue(queueName, callback, clientName);
                return true;
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
                return false;
            }
        }

        public static string BuildMessage(
            string functionName,
            string data,
            string contentType = ContentEnum.Json,
            string requestID = null)
        {
            return MessageQueueBase.BuildMessage(functionName, data, contentType, requestID);
        }

        public static bool SendToQueue(
            string queueName,
            string functionName,
            string data,
            string contentType,
            string queueHandler)
        {
            string message = BuildMessage(functionName, data, contentType);
            return SendToQueue(queueName, message, contentType, queueHandler);
        }

        public static bool SendToQueue(
            string queueName,
            string message,
            string contentType = ContentEnum.Json,
            string queueHandler = MessageQueueEnum.Rabbit)
        {
            MessageQueueBase messageQueue = GetQueueHandler(queueHandler);
            return messageQueue != null && messageQueue.SendToQueue(queueName, message, contentType);
        }

        public static bool SendToQueue(
            string queueName,
            ref MessageQueueData messageData,
            string contentType = ContentEnum.Json,
            string queueHandler = MessageQueueEnum.Rabbit)
        {
            MessageQueueBase messageQueue = GetQueueHandler(queueHandler);
            if (messageQueue == null)
            {
                return false;
            }
            return InsertToCache(messageData) &&
                messageQueue.SendToQueue(queueName, messageData.RequestData, contentType);
        }


        private static MessageQueueBase GetQueueHandler(string queueHandler)
        {
            return HandlerDictionary.ContainsKey(queueHandler) ? HandlerDictionary[queueHandler] : null;
        }

        private static bool InsertToCache(MessageQueueData messageData)
        {
            if (MessageDictionary.ContainsKey(messageData.RequestID))
            {
                return false;
            }

            bool result = MessageDictionary.TryAdd(messageData.RequestID, messageData);
            if (result)
            {
                ScheduleTimer.Start();
            }
            return result;
        }
    }
}