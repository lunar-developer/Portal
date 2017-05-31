using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class IdentityTypeBusiness
    {
        public static List<IdentityTypeData> GetAllIdentityType()
        {
            return new IdentityTypeProvider().GetAllIdentityType();
        }

        public static IdentityTypeData GetIdentityType(string identityTypeID)
        {
            return new IdentityTypeProvider().GetIdentityType(identityTypeID);
        }
    }
}