using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Extension;
using Website.Library.Global;

namespace Website.UnitTest
{
    public class Program
    {
        private static readonly ConnectionFactory LocalConnectionFactory = new ConnectionFactory
        {
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            HostName = "localhost"
        };


        private static void Main()
        {
            try
            {
                var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);


                string s = "TodayILiveInTheUSAWithSimon";
                Console.WriteLine(r.Replace(s, " "));

                return;

                //using (IConnection connection = LocalConnectionFactory.CreateConnection("Portal Client"))
                //{
                //    using (IModel channel = connection.CreateModel())
                //    {
                //        IBasicProperties props = channel.CreateBasicProperties();
                //        props.ContentType = ContentEnum.Json;
                //        props.Persistent = true;

                //        Dictionary<string, string> dictionary = new Dictionary<string, string>
                //        {
                //            { "ApplicID", "2017080400000001" }
                //        };

                //        RequestData request = new RequestData();
                //        request.Function = "CreateApplication";
                //        request.Data = FunctionBase.Serialize(dictionary);
                //        request.RequestID = "1";
                //        request.RequestDateTime = DateTime.Now.ToString();

                //        byte[] body = Encoding.UTF8.GetBytes(FunctionBase.Serialize(request));
                //        channel.BasicPublish(string.Empty, "Portal.inq", props, body);
                //    }
                //}



                //using (IConnection connection = LocalConnectionFactory.CreateConnection("Portal Client"))
                //{
                //    using (IModel channel = connection.CreateModel())
                //    {
                //        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                //        consumer.Received += AutoReceive;
                //        channel.BasicConsume("mq:RequestData.inq", true, consumer);
                //        System.Threading.Thread.Sleep(10000000);
                //    }
                //}
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Console.ReadLine();
        }

        private static void AutoReceive(object model, BasicDeliverEventArgs e)
        {
            byte[] body = e.Body;
            string message = Encoding.UTF8.GetString(body);
            RequestData request = FunctionBase.Deserialize<RequestData>(message);

            string data;
            switch (request.Function)
            {
                case "QueryCustomer":
                    data = QueryCustomer(request.Data);
                    break;

                case "QueryAccount":
                    data = QueryAccount(request.Data);
                    break;

                case "QueryCollateral":
                    data = QueryCollateral(request.Data);
                    break;

                default:
                    data = string.Empty;
                    break;
            }

            ResponseData response = new ResponseData
            {
                RequestID = request.RequestID,
                ResponseCode = "200",
                Data = data
            };
            message = FunctionBase.Serialize(response);
            using (IConnection connection = LocalConnectionFactory.CreateConnection("Temp"))
            {
                using (IModel channel = connection.CreateModel())
                {
                    IBasicProperties props = channel.CreateBasicProperties();
                    props.ContentType = "application/json";
                    props.Persistent = true;
                    body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(string.Empty,
                        "mq:ResponseData.inq",
                        props,
                        body);
                }
            }
        }

        private static string QueryCustomer(string requestData)
        {
            InsensitiveDictionary<string> dataDictionary =
                FunctionBase.Deserialize<InsensitiveDictionary<string>>(requestData);
            string customerID = dataDictionary.GetValue("CustomerID");
            switch (customerID)
            {
                case "1234":
                {
                    InsensitiveDictionary<string> responseDictionary = new InsensitiveDictionary<string>
                    {
                        { "IdentityTypeCode", "CM" },
                        { "CIFNo", "1234" },
                        { "FullName", "HUYNH HUU LOC" },
                        { "OldCustomerID", "023832218" },
                        { "Gender", "M" },
                        { "Nationality", "VN" },
                        { "BirthDate", "19851123" },
                        { "Mobile01", "0907" },
                        { "Email01", "lochh@gmail.com" },
                        { "CustomerClass", "P" },

                        { "HomeAddress01", "82" },
                        { "HomeAddress02", "TRINH HOAI DUC" },
                        { "HomeAddress03", "P.13" },
                        { "HomeCountry", "VN" },
                        { "HomeState", "79" },
                        { "HomeCity", "774" },
                        { "HomePhone01", "0907" }
                    };
                    return FunctionBase.Serialize(responseDictionary);
                }

                case "5678":
                {
                    InsensitiveDictionary<string> responseDictionary = new InsensitiveDictionary<string>
                    {
                        { "IdentityTypeCode", "HC" },
                        { "CIFNo", "A1234" },
                        { "FullName", "HUYEN TON NU TRAN THI KHANH TRAN" },
                        { "OldCustomerID", "" },
                        { "Gender", "F" },
                        { "Nationality", "VN" },
                        { "BirthDate", "19901020" },
                        { "Mobile01", "0907" },
                        { "Email01", "tran@gmail.com" },
                        { "CustomerClass", "P" },

                        { "HomeAddress01", "10" },
                        { "HomeAddress02", "NGUYEN TRAI" },
                        { "HomeAddress03", "P.1" },
                        { "HomeCountry", "VN" },
                        { "HomeState", "79" },
                        { "HomeCity", "760" },
                        { "HomePhone01", "0907" }
                    };
                    return FunctionBase.Serialize(responseDictionary);
                }

                default:
                    return string.Empty;
            }
        }

        private static string QueryCollateral(string requestData)
        {
            InsensitiveDictionary<string> dataDictionary =
                FunctionBase.Deserialize<InsensitiveDictionary<string>>(requestData);
            string collateralID = dataDictionary.GetValue("CollateralID");
            switch (collateralID)
            {
                case "1234":
                    {
                        InsensitiveDictionary<string> responseDictionary = new InsensitiveDictionary<string>
                        {
                            { "CollateralValue", "10000000" },
                            { "CollateralCreditLimit", "1000000" },
                            { "CollateralPurpose", "MUA IPHONE 7" },
                            { "CollateralDescription", "CAM CO XE GAN MAY" }
                        };
                        return FunctionBase.Serialize(responseDictionary);
                    }

                case "5678":
                    { 
                        InsensitiveDictionary<string> responseDictionary = new InsensitiveDictionary<string>
                        {
                            { "CollateralValue", "20000000" },
                            { "CollateralCreditLimit", "2000000" },
                            { "CollateralPurpose", "MUA XE TOYOTA 7 CHO" },
                            { "CollateralDescription", "CAM CO NHA CUA" }
                        };
                        return FunctionBase.Serialize(responseDictionary);
                    }

                default:
                    return string.Empty;
            }
        }

        private static string QueryAccount(string requestData)
        {
            InsensitiveDictionary<string> dataDictionary =
                FunctionBase.Deserialize<InsensitiveDictionary<string>>(requestData);
            string cifNo = dataDictionary.GetValue("CIFNo");
            switch (cifNo)
            {
                case "1234":
                    {
                        InsensitiveDictionary<string> responseDictionary = new InsensitiveDictionary<string>();
                        List<Dictionary<string, string>> list = new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "AccountNo", "0001" },
                            { "BranchName", "CN QUAN 1" }
                        },
                        new Dictionary<string, string>
                        {
                            { "AccountNo", "0002" },
                            { "BranchName", "CN QUAN 5" }
                        }
                    };
                        responseDictionary.Add("CustomerName", "Huynh Huu Loc");
                        responseDictionary.Add("Accounts", FunctionBase.Serialize(list));
                        return FunctionBase.Serialize(responseDictionary);
                    }

                case "5678":
                    {
                        InsensitiveDictionary<string> responseDictionary =
                            new InsensitiveDictionary<string>
                            {
                                { "CustomerName", "Nguyen Van Ba" },
                                { "Accounts", string.Empty }
                            };
                        return FunctionBase.Serialize(responseDictionary);
                    }

                default:
                    return string.Empty;
            }
        }
    }
}