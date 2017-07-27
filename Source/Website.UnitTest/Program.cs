using System;
using System.Collections.Generic;
using Website.Library.Global;

namespace Website.UnitTest
{
    public class Program
    {
        private static void Main()
        {
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    { "ApplicId", "1" }
                };
                string data = FunctionBase.Serialize(dictionary);
                data = MessageQueueBase.BuildMessage("CreateApplication", data);
                new RabbitMessageQueueBase().SendToQueue("Portal.inq", data);
                Console.WriteLine(data);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Console.ReadLine();
        }
    }
}