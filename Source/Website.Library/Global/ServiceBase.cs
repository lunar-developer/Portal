using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Website.Library.Enum;

namespace Website.Library.Global
{
    public class ServiceBase
    {
        private readonly string ServiceUrl;
        private Dictionary<string, string> Dictionary = new Dictionary<string, string>();


        public ServiceBase(string url)
        {
            ServiceUrl = url;
        }

        protected bool Post(string function, string data)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(ServiceUrl);
            httpWebRequest.Accept = ContentEnum.Json;
            httpWebRequest.ContentType = ContentEnum.Json;
            httpWebRequest.Method = HTTPMethodEnum.Post;
            httpWebRequest.Timeout = 60 * UnitEnum.Second;

            // Send Request
            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(BuildMessage(function, data));
                streamWriter.Flush();
            }

            // Receive Response
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream stream = httpWebResponse.GetResponseStream();
            if (stream == null)
            {
                return false;
            }
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string response = streamReader.ReadToEnd();
                Dictionary = FunctionBase.Deserialize<Dictionary<string, string>>(response);
                return IsResponseSuccess();
            }
        }

        protected string BuildMessage(string function, string data)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { "RequestID", Guid.NewGuid().ToString(PatternEnum.GuidDigits) },
                { "RequestDateTime", DateTime.Now.ToString(PatternEnum.DateTimeUniversal) },
                { "Function", function },
                { "Data", data }
            };
            return FunctionBase.Serialize(dictionary);
        }

        protected string GetData(string key)
        {
            return Dictionary.ContainsKey(key) ? Dictionary[key] : string.Empty;
        }

        protected string GetResponseCode()
        {
            return GetData("ResponseCode");
        }

        protected bool IsResponseSuccess()
        {
            return GetResponseCode() == ResponseEnum.Success;
        }
    }
}