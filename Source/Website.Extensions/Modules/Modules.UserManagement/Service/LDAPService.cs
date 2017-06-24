using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using Modules.UserManagement.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;
using ConfigEnum = Modules.UserManagement.Enum.ConfigEnum;

namespace Modules.UserManagement.Service
{
    public class LDAPService : ServiceBase
    {
        private static readonly string LDAPServiceUrl = FunctionBase.GetConfiguration(ConfigEnum.LDAPServiceUrl);
        private static readonly string LDAPServiceKey = FunctionBase.GetConfiguration(ConfigEnum.LDAPServiceKey);


        public LDAPService() : base(LDAPServiceUrl)
        {
        }

        public UserInfo Authenticate(string userName, string password, out UserLoginStatus status)
        {
            status = UserLoginStatus.LOGIN_FAILURE;
            string responseCode = Authenticate(userName, password).Result;
            switch (responseCode)
            {
                case ResponseEnum.Success:
                    UserInfo userInfo = UserController.GetUserByName(0, userName);
                    if (userInfo != null)
                    {
                        if (CacheBase.Receive<UserData>(userInfo.UserID.ToString()) == null)
                        {
                            CacheBase.Reload<UserData>(userInfo.UserID.ToString());
                        }

                        status = UserLoginStatus.LOGIN_SUCCESS;
                    }
                    return userInfo;

                default:
                    return null;
            }
        }

        public async Task<string> Authenticate(string userName, string password)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { "UserName", userName },
                { "Password", SecurityBase.Encrypt(LDAPServiceKey, password) },
                { "LoginProvider", "LDAP" }
            };
            string json = FunctionBase.Serialize(dictionary);
            await Task.FromResult(Post("Authenticate", json));
            return GetResponseCode();
        }
    }
}