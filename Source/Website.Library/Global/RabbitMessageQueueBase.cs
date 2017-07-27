using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Website.Library.Enum;

namespace Website.Library.Global
{
    public class RabbitMessageQueueBase : MessageQueueBase
    {
        private readonly ConnectionFactory LocalConnectionFactory;
        private IConnection LocalConnection;
        private IModel LocalModel;
        private Action<string, string> LocalCallback;


        public RabbitMessageQueueBase(
            string hostName = "localhost",
            int port = 5672,
            string virtualHost = "/",
            string userName = "guest",
            string password = "guest")
        {
            LocalConnectionFactory = new ConnectionFactory
            {
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost,
                HostName = hostName,
                Port = port
            };
        }

        public RabbitMessageQueueBase(ConnectionFactory connectionFactory)
        {
            LocalConnectionFactory = connectionFactory;
        }

        ~RabbitMessageQueueBase()
        {
            LocalConnection?.Dispose();
            LocalModel?.Dispose();
        }


        public override bool SendToQueue(string queueName, string message, string contentType = ContentEnum.Json)
        {
            try
            {
                using (LocalConnection = LocalConnectionFactory.CreateConnection())
                {
                    using (LocalModel = LocalConnection.CreateModel())
                    {
                        IBasicProperties props = LocalModel.CreateBasicProperties();
                        props.ContentType = contentType;
                        props.Persistent = true;

                        byte[] body = Encoding.UTF8.GetBytes(message);
                        LocalModel.BasicPublish(string.Empty, queueName, props, body);
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
                return false;
            }
        }

        public override void ReceiveFromQueue(
            string queueName,
            Action<string, string> callback,
            string clientName = "Portal")
        {
            try
            {
                LocalCallback = callback;
                LocalConnection = LocalConnectionFactory.CreateConnection(clientName);
                LocalModel = LocalConnection.CreateModel();
                EventingBasicConsumer consumer = new EventingBasicConsumer(LocalModel);
                consumer.Received += ProcessOnQueueReceive;
                LocalModel.BasicConsume(queueName, true, consumer);
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);

                LocalConnection?.Dispose();
                LocalModel?.Dispose();
            }
        }

        public override MessageQueueBase Clone()
        {
            return new RabbitMessageQueueBase(LocalConnectionFactory);
        }


        private void ProcessOnQueueReceive(object model, BasicDeliverEventArgs eventArgs)
        {
            byte[] body = eventArgs.Body;
            string contentType = eventArgs.BasicProperties.ContentType;
            string message = Encoding.UTF8.GetString(body);
            LocalCallback(message, contentType);
        }
    }
}