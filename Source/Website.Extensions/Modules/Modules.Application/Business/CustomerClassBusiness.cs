using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class CustomerClassBusiness
    {
        public static List<CustomerClassData> GetAllCustomerClass()
        {
            return new CustomerClassProvider().GetAllCustomerClass();
        }

        public static CustomerClassData GetCustomerClass(string code)
        {
            return new CustomerClassProvider().GetCustomerClass(code);
        }
    }
}