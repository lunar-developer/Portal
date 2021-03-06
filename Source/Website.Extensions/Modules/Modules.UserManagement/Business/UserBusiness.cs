﻿using System;
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
using DotNetNuke.Security.Roles;
using Modules.UserManagement.DataAccess;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Enum;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Extension;
using Website.Library.Global;
using ConfigEnum = Website.Library.Enum.ConfigEnum;
using FolderEnum = Modules.UserManagement.Enum.FolderEnum;
using RoleEnum = Modules.UserManagement.Enum.RoleEnum;

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

        public static DataTable SearchUser(Dictionary<string, SQLParameterData> parameterdiDictionary)
        {
            return new UserProvider().SearchUser(parameterdiDictionary);
        }

        public static DataSet LoadUser(string userID, string viewUserID)
        {
            return new UserProvider().LoadUser(userID, viewUserID);
        }

        public static int CreateUser(Dictionary<string, SQLParameterData> dictionary, out string message)
        {
            UserInfo userInfo = new UserInfo
            {
                Username = dictionary[UserTable.UserName].ParameterValue.ToString(),
                Email = dictionary[UserTable.UserName].ParameterValue.ToString(),
                DisplayName = dictionary[UserTable.DisplayName].ParameterValue.ToString(),
                PortalID = 0,                
                Membership =
                {
                    Password = UserController.GeneratePassword(),                    
                    UpdatePassword = true,
                    Approved = true
                }
            };
            UserCreateStatus status = UserController.CreateUser(ref userInfo);
            if (status != UserCreateStatus.Success)
            {
                message = UserController.GetUserCreateStatus(status);
                return 0;
            }

            SendEmailNotification(TemplateEnum.AccountRegister, userInfo);
            return UpdateProfile(dictionary, out message);
        }

        public static int UpdateProfile(Dictionary<string, SQLParameterData> dictionary, out string message)
        {
            int userID = new UserProvider().UpdateProfile(dictionary);
            message = userID == -1 ? "Không thể lưu thông tin Profile của User." : string.Empty;

            if (userID > 0)
            {
                // Reload cache
                CacheBase.Reload<UserData>(userID.ToString());
            }
            return userID;
        }

        public static bool UpdateRole(Dictionary<string, SQLParameterData> dictionary, string userName)
        {
            bool result = new UserProvider().UpdateRole(dictionary);
            if (result)
            {
                DataCache.ClearUserCache(0, userName);
            }
            return result;
        }

        public static bool UpdatePassword(UserInfo userInfo, string oldPassword, string newPassword, int actionUser,
            out PasswordUpdateStatus updateStatus)
        {
            // Check New Password is Valid
            if (UserController.ValidatePassword(newPassword) == false)
            {
                updateStatus = PasswordUpdateStatus.PasswordInvalid;
                return false;
            }

            // Check New Password is not same as username or banned
            MembershipPasswordController membershipPasswordController = new MembershipPasswordController();
            MembershipPasswordSettings settings = new MembershipPasswordSettings(userInfo.PortalID);
            if (settings.EnableBannedList)
            {
                if (membershipPasswordController.FoundBannedPassword(newPassword) || userInfo.Username == newPassword)
                {
                    updateStatus = PasswordUpdateStatus.BannedPasswordUsed;
                    return false;
                }
            }

            // Check new password is not in history
            if (membershipPasswordController.IsPasswordInHistory(userInfo.UserID, userInfo.PortalID, newPassword, false))
            {
                updateStatus = PasswordUpdateStatus.PasswordResetFailed;
                return false;
            }

            // Check user permission (don't required old password if current user is administrator)
            if (string.IsNullOrWhiteSpace(oldPassword) && FunctionBase.IsInRole(RoleEnum.Administrator))
            {
                oldPassword = UserController.ResetPassword(userInfo, string.Empty);
                userInfo.Membership.UpdatePassword = true;
                userInfo.PasswordResetToken = Guid.NewGuid();
                userInfo.PasswordResetExpiration = DateTime.Now.AddDays(7);
            }
            else
            {
                userInfo.Membership.UpdatePassword = false;
            }

            bool result = UserController.ChangePassword(userInfo, oldPassword, newPassword);
            if (result)
            {
                Dictionary<string, SQLParameterData> dictionary = new Dictionary<string, SQLParameterData>
                {
                    { UserTable.UserID, new SQLParameterData(userInfo.UserID, SqlDbType.Int) },
                    { UserTable.LogAction, new SQLParameterData("CẬP NHẬT MẬT KHẨU", SqlDbType.NVarChar) },
                    { UserTable.LogDetail, new SQLParameterData(string.Empty, SqlDbType.NVarChar) },
                    { UserTable.Remark, new SQLParameterData(string.Empty, SqlDbType.NVarChar) },
                    { BaseTable.UserIDModify, new SQLParameterData(actionUser, SqlDbType.Int) },
                    {
                        BaseTable.DateTimeModify,
                        new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                    }
                };
                InsertUserLog(dictionary);
                SendEmailNotification(TemplateEnum.PasswordUpdate, userInfo);
            }

            updateStatus = result ? PasswordUpdateStatus.Success : PasswordUpdateStatus.PasswordResetFailed;
            return result;
        }

        public static bool UpdateAccountStatus(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            bool result = new UserProvider().UpdateAccountStatus(parameterDictionary);
            string authoriseFlag = parameterDictionary[UserTable.Authorised].ParameterValue.ToString();
            string userID = parameterDictionary[BaseTable.UserID].ParameterValue.ToString();
            string userName = parameterDictionary[BaseTable.UserName].ParameterValue.ToString();
            if (result)
            {
                switch (authoriseFlag)
                {
                    case UserAuthoriseEnum.Locked:
                        int.TryParse(userID, out int id);
                        UserInfo userInfo = UserController.GetUserById(0, id);
                        SendEmailNotification(TemplateEnum.AccountUnlock, userInfo);
                        break;

                    case UserAuthoriseEnum.Disabled:
                        DataCache.ClearUserCache(0, userName);
                        CacheBase.Remove<UserData>(userID);
                        break;

                    default:
                        DataCache.ClearUserCache(0, userName);
                        CacheBase.Reload<UserData>(userID);
                        break;
                }
            }
            return true;
        }


        public static bool ConfirmBranch(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            bool result = new UserProvider().ConfirmBranch(parameterDictionary);
            if (result)
            {
                // Reload cache
                CacheBase.Reload<UserData>(parameterDictionary[UserTable.UserID].ParameterValue.ToString());
            }
            return result;
        }

        public static bool InsertUserLog(Dictionary<string, SQLParameterData> dictionary)
        {
            return new UserProvider().InsertUserLog(dictionary);
        }

        public static void SendEmailNotification(
            string template,
            UserInfo userInfo,
            InsensitiveDictionary<string> parameters = null)
        {
            if (userInfo == null
                || DictTemplate.ContainsKey(template) == false)
            {
                return;
            }

            string subject = GetEmailSubject(template);
            string body = GetEmailBody(template, userInfo, parameters);
            MailBase.SendEmail(userInfo.Email, subject, body);
        }

        private static string GetEmailSubject(string template)
        {
            switch (template)
            {
                case TemplateEnum.AccountUnlock:
                    return "MỞ KHÓA TÀI KHOẢN";

                case TemplateEnum.UserRequest:
                    return "THÔNG TIN YÊU CẦU";

                case TemplateEnum.PasswordUpdate:
                    return "THÔNG TIN MẬT KHẨU";

                default:
                    return "THÔNG TIN TÀI KHOẢN";
            }
        }

        private static string GetEmailBody(
            string template,
            UserInfo userInfo,
            InsensitiveDictionary<string> parameters = null)
        {
            string body = DictTemplate[template];
            Dictionary<string, string> dictionary;
            switch (template)
            {
                case TemplateEnum.AccountUnlock:
                    dictionary = new Dictionary<string, string>
                    {
                        { "@DisplayName", userInfo.DisplayName },
                        { "@Link", FunctionBase.GetConfiguration(ConfigEnum.SiteUrl) }
                    };
                    break;

                case TemplateEnum.UserRequest:
                    string link = parameters?.GetValue("Link") ?? "javascript:;";
                    dictionary = new Dictionary<string, string>
                    {
                        { "@DisplayName", userInfo.DisplayName },
                        { "@Link", link }
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

        public static List<UserData> GetUsersInBranch(int branchID)
        {
            BranchData branch = CacheBase.Receive<BranchData>(branchID.ToString());
            return CacheBase.Receive<UserData>().Where(item => item.BranchID == branch.BranchID).ToList();
        }

        public static List<UserData> GetUsersInBranch(string branchCode)
        {
            BranchData branch = CacheBase.Find<BranchData>(BranchTable.BranchCode, branchCode);
            return CacheBase.Receive<UserData>().Where(item => item.BranchID == branch.BranchID).ToList();
        }

        public static List<UserData> GetUsersHaveRoles(params string[] roles)
        {
            List<int> listRoleID = roles.Select(roleName =>
                RoleController.Instance.GetRoleByName(0, roleName).RoleID).ToList();

            return new UserProvider().GetUsersHaveRoles(string.Join(",", listRoleID));
        }

        public static string GetCurrentBranchCode(int userID)
        {
            UserData userData = CacheBase.Receive<UserData>(userID.ToString());
            return CacheBase.Receive<BranchData>(userData.BranchID).BranchCode;
        }
    }
}