using System;
using System.Text;
using System.Threading;
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
        private string LocalClientName;
        private string LocalQueueName;
        private Action<string, string> LocalCallback;


        public RabbitMessageQueueBase(
            string hostName = "localhost",
            int port = 5672,
            string virtualHost = "/",
            string userName = "guest",
            string password = "guest",
            ushort heartBeat = 30)
        {
            LocalConnectionFactory = new ConnectionFactory
            {
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost,
                HostName = hostName,
                Port = port,
                RequestedHeartbeat = heartBeat
            };
        }

        public RabbitMessageQueueBase(ConnectionFactory connectionFactory)
        {
            LocalConnectionFactory = connectionFactory;
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

        public override void ReceiveFromQueue(string queueName, Action<string, string> callback, string clientName = "Portal")
        {
            LocalClientName = clientName;
            LocalQueueName = queueName;
            LocalCallback = callback;

            OpenConnection();
            RegisterConsumer();
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

        private void ProcessOnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            // Domain unload => Exit
            if (e.Initiator == ShutdownInitiator.Application)
            {
                return;
            }

            // Try to reconnect
            OpenConnection();
            RegisterConsumer();
        }

        private void OpenConnection()
        {
            while (true)
            {
                try
                {
                    LocalConnection = LocalConnectionFactory.CreateConnection(LocalClientName);
                    LocalConnection.ConnectionShutdown += ProcessOnConnectionShutdown;
                    break;
                }
                catch (Exception exception)
                {
                    FunctionBase.LogError(exception);
                    Thread.Sleep(10000);
                }
            }
        }

        private void RegisterConsumer()
        {
            try
            {
                LocalModel = LocalConnection.CreateModel();
                EventingBasicConsumer consumer = new EventingBasicConsumer(LocalModel);
                consumer.Received += ProcessOnQueueReceive;
                LocalModel.BasicConsume(LocalQueueName, true, consumer);
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);

                LocalModel?.Dispose();
                LocalConnection?.Dispose();
            }
        }
    }
}