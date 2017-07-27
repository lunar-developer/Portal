using System.Collections.Generic;

namespace Website.Library.Enum
{
    public static class ResponseEnum
    {
        public const string Success = "200";
        public const string BadRequest = "400";
        public const string Unauthorized = "401";
        public const string Forbidden = "403";
        public const string Conflict = "409";
        public const string Error = "500";
        public const string NotImplemented = "501";
        public const string ServiceUnavailable = "503";
        public const string Timeout = "504";


        private static readonly Dictionary<string, string> DictDescription = new Dictionary<string, string>
        {
            { Success, "Success" },
            { BadRequest, "Bad Request" },
            { Unauthorized, "Unauthorized" },
            { Forbidden, "Method Not Allowed" },
            { Conflict, "Duplicate Request" },
            { Error, "Internal Server Error" },
            { NotImplemented, "Service Not Implemented"},
            { ServiceUnavailable, "Service Unavailable" },
            { Timeout, "Request Timeout" }
        };


        public static string GetDescription(string responseCode)
        {
            return DictDescription.ContainsKey(responseCode) ? DictDescription[responseCode] : string.Empty;
        }
    }
}