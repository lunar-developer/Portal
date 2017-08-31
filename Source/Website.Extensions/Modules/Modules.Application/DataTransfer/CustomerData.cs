using System.Collections.Generic;
using Website.Library.Extension;

namespace Modules.Application.DataTransfer
{
    public class CustomerData
    {
        public string CustomerName { get; set; }
        public List<InsensitiveDictionary<string>> Accounts { get; set; }
    }
}