using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Users.Membership;
using DotNetNuke.Security.Membership;
using Modules.UserManagement.DataAccess;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Enum;
using Website.Library.Enum;
using Website.Library.Global;
using ConfigEnum = Website.Library.Enum.ConfigEnum;
using FolderEnum = Modules.UserManagement.Enum.FolderEnum;

namespace Modules.UserManagement.Business
{
    public static class UserBusiness
    {
        private static readonly Dictionary<string, string> DictTemplate = new Dictionary<string, string>();


        static UserBusiness()
        {
            try
            {
                string folder = HttpContext.Current.Server.MapPath("~") + FolderEnum.TemplateFolder;
                foreach (string filePath in Directory.GetFiles(folder))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        DictTemplate.Add(
                            Path.GetFileNameWithoutExtension(filePath),
                            FunctionBase.Minimize(reader.ReadToEnd()));
                    }
                }
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
            }
        }


        public static List<UserData> GetUserExtension()
        {
            return new UserProvider().GetUserExtension();
        }

        public static UserData GetUserExtension(string userID)
        {
            return new UserProvider().GetUserExtension(userID);
        }

        public static DataTable GetUserLog(string userID)
        {
            return new UserProvider().GetUserLog(userID);
        }

        public static DataTable SearchUser(Dictionary<string, string> dictionary)
        {
            return new UserProvider().SearchUser(dictionary);
        }

        public static DataSet LoadUser(string userID, string viewUserID)
        {
            return new UserProvider().LoadUser(userID, viewUserID);
        }

        public static int CreateUser(Dictionary<string, string> dictionary, out string message)
        {
            UserInfo userInfo = new UserInfo
            {
                Username = dictionary[UserTable.UserName],
                Email = dictionary[UserTable.UserName],
                DisplayName = dictionary[UserTable.DisplayName],
                PortalID = 0,
                Membership =
                {
                    Password = UserController.GeneratePassword(),
                    Approved = true
                }
            };
            UserCreateStatus status = UserController.CreateUser(ref userInfo);
            if (status != UserCreateStatus.Success)
            {
                message = UserController.GetUserCreateStatus(status);
                return 0;
            }

            SendNotification(TemplateEnum.AccountRegister, userInfo);
            return UpdateProfile(dictionary, out message);
        }

        public static int UpdateProfile(Dictionary<string, string> dictionary, out string message)
        {
            return new UserProvider().UpdateProfile(dictionary, out message);
        }

        public static bool UpdateRole(Dictionary<string, string> dictionary, string userName, out string message)
        {
            bool result = new UserProvider().UpdateRole(dictionary, out message);
            if (result)
            {
                DataCache.ClearUserCache(0, userName);
            }
            return result;
        }

        public static bool UpdatePassword(UserInfo userInfo, string password, int actionUser,
            out PasswordUpdateStatus updateStatus)
        {
            // Check New Password is Valid
            if (UserController.ValidatePassword(password) == false)
            {
                updateStatus = PasswordUpdateStatus.PasswordInvalid;
                return false;
            }

            // Check New Password is not same as username or banned
            MembershipPasswordController membershipPasswordController = new MembershipPasswordController();
            MembershipPasswordSettings settings = new MembershipPasswordSettings(userInfo.PortalID);
            if (settings.EnableBannedList)
            {
                if (membershipPasswordController.FoundBannedPassword(password) || userInfo.Username == password)
                {
                    updateStatus = PasswordUpdateStatus.BannedPasswordUsed;
                    return false;
                }
            }

            // Check new password is not in history
            if (membershipPasswordController.IsPasswordInHistory(userInfo.UserID, userInfo.PortalID, password, false))
            {
                updateStatus = PasswordUpdateStatus.PasswordResetFailed;
                return false;
            }

            bool result = UserController.ResetAndChangePassword(userInfo, password);
            if (result)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    { UserTable.UserID, userInfo.UserID.ToString() },
                    { UserTable.LogAction, "Cập Nhật Mật Khẩu" },
                    { UserTable.LogDetail, string.Empty },
                    { UserTable.Remark, string.Empty },
                    { UserTable.ModifyUserID, actionUser.ToString() },
                    { UserTable.ModifyDateTime, DateTime.Now.ToString(PatternEnum.DateTime) }
                };
                InsertUserLog(dictionary);
                SendNotification(TemplateEnum.PasswordUpdate, userInfo);
            }

            updateStatus = result ? PasswordUpdateStatus.Success : PasswordUpdateStatus.PasswordResetFailed;
            return result;
        }

        public static bool InsertUserLog(Dictionary<string, string> dictionary)
        {
            return new UserProvider().InsertUserLog(dictionary);
        }

        private static void SendNotification(string template, UserInfo userInfo)
        {
            if (DictTemplate.ContainsKey(template) == false)
            {
                return;
            }

            string subject = GetEmailSubject(template);
            string body = GetEmailBody(template, userInfo);
            MailBase.SendEmail(userInfo.Email, subject, body, null);
        }

        private static string GetEmailSubject(string template)
        {
            switch (template)
            {
                case TemplateEnum.PasswordUpdate:
                    return "THÔNG TIN MẬT KHẨU";

                default:
                    return "THÔNG TIN TÀI KHOẢN";
            }
        }

        private static string GetEmailBody(string template, UserInfo userInfo)
        {
            string body = DictTemplate[template];
            Dictionary<string, string> dictionary;
            switch (template)
            {
                case TemplateEnum.PasswordUpdate:
                    dictionary = new Dictionary<string, string>
                    {
                        { "@SiteName", PortalSettings.Current.PortalName },
                        { "@DisplayName", userInfo.DisplayName },
                        { "@UserName", userInfo.Username },
                        { "@Password", userInfo.Membership.Password },
                        { "@Link", FunctionBase.GetConfiguration(ConfigEnum.SiteUrl) }
                    };
                    break;

                default:
                    dictionary = new Dictionary<string, string>
                    {
                        { "@SiteName", PortalSettings.Current.PortalName },
                        { "@DisplayName", userInfo.DisplayName },
                        { "@UserName", userInfo.Username },
                        { "@Password", userInfo.Membership.Password },
                        { "@Link", FunctionBase.GetConfiguration(ConfigEnum.SiteUrl) }
                    };
                    break;
            }

            return dictionary.Aggregate(body, (current, pair) => current.Replace(pair.Key, pair.Value));
        }

        public static List<BranchData> GetUserBranch(string userId)
        {
            UserData userData = CacheBase.Receive<UserData>(userId);
            return userData == null
                ? new List<BranchData>()
                : GetUserBranch(CacheBase.Receive<BranchData>(), userData.BranchID)
                    .OrderBy(iterator => int.Parse(iterator.BranchID))
                    .ToList();
        }

        private static List<BranchData> GetUserBranch(List<BranchData> listSource, string branchID)
        {
            List<BranchData> listResult = new List<BranchData>();
            IEnumerable<BranchData> listBranch = listSource.Where(iterator =>
                iterator.BranchID == branchID || iterator.ParentID == branchID);
            foreach (BranchData branch in listBranch)
            {
                if (branch.BranchID == branchID)
                {
                    listResult.Add(branch);
                }
                else
                {
                    listResult.AddRange(GetUserBranch(listSource, branch.BranchID));
                }
            }
            return listResult;
        }

        public static bool IsUserOfBranch(string userID, int branchID)
        {
            string value = branchID.ToString();
            return GetUserBranch(userID).Any(item => item.BranchID == value);
        }

        public static bool IsUserOfBranch(string userID, string branchCode)
        {
            return GetUserBranch(userID).Any(item => item.BranchCode == branchCode);
        }
    }
}