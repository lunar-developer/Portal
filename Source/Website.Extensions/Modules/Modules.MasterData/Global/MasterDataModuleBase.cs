using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetNuke.Entities.Users;
using Modules.MasterData.Database;
using Website.Library.Enum;
using Website.Library.Global;

namespace Modules.MasterData.Global
{
    public class MasterDataModuleBase : DesktopModuleBase
    {
        private static readonly Dictionary<string, Func<string>> FunctionDictionary;
        private static readonly Dictionary<string, Func<string, string>> TemplateDictionary;

        static MasterDataModuleBase()
        {
            FunctionDictionary = new Dictionary<string, Func<string>>
            {
                { MasterDataTable.UserIDModify, GetCurrentUserID },
                { MasterDataTable.DateTimeModify, GetCurrentDateTime },
            };

            TemplateDictionary = new Dictionary<string, Func<string, string>>
            {
                { MasterDataTable.UserIDModify, FunctionBase.FormatUserID },
                { MasterDataTable.DateTimeModify, FunctionBase.FormatDate },
                { MasterDataTable.RoleID, FunctionBase.FormatRoleID }
            };
        }

        public bool IsRuntimeField(string fieldName)
        {
            return FunctionDictionary.ContainsKey(fieldName);
        }

        public bool IsTemplateField(string fieldName)
        {
            return TemplateDictionary.ContainsKey(fieldName);
        }

        public string GetRuntimeValue(string fieldName)
        {
            return FunctionDictionary.ContainsKey(fieldName) ? FunctionDictionary[fieldName].Invoke() : string.Empty;
        }

        public string GetTemplateValue(string fieldName, string fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                return fieldValue;
            }

            return TemplateDictionary.ContainsKey(fieldName)
                ? TemplateDictionary[fieldName].Invoke(fieldValue)
                : fieldValue;
        }


        private static string GetCurrentUserID()
        {
            return UserController.Instance.GetCurrentUserInfo().UserID.ToString();
        }

        private static string GetCurrentDateTime()
        {
            return DateTime.Now.ToString(PatternEnum.DateTime);
        }


        public void ReloadCache(string assemblyName, string cacheName, string cacheID,
            Dictionary<string, string> dataDictionary)
        {
            if (FunctionBase.IsNullOrWhiteSpace(assemblyName, cacheName, cacheID))
            {
                return;
            }

            // Validate Assembly
            Type type = Type.GetType($"{cacheName}, {assemblyName}");
            if (type == null)
            {
                return;
            }

            // Convert to cache object
            object dataOject = Activator.CreateInstance(type);
            foreach (KeyValuePair<string, string> field in dataDictionary)
            {
                type.GetField(field.Key)?.SetValue(dataOject, field.Value);
                type.GetProperty(field.Key)?.SetValue(dataOject, field.Value);
            }

            string id = dataDictionary[cacheID];
            MethodInfo method = typeof(CacheBase).GetMethod("Update").MakeGenericMethod(type);
            method.Invoke(null, new[] { id, dataOject });
        }

        public void RemoveCache(string assemblyName, string cacheName, string key)
        {
            if (FunctionBase.IsNullOrWhiteSpace(assemblyName, cacheName, key))
            {
                return;
            }

            // Validate Assembly
            Type type = Type.GetType($"{cacheName}, {assemblyName}");
            if (type == null)
            {
                return;
            }

            MethodInfo method = typeof(CacheBase).GetMethod("Remove", new []{ typeof(string) }).MakeGenericMethod(type);
            method.Invoke(null, new object[] { key });
        }

        public List<object> ReceiveCache(Type type)
        {
            MethodInfo method = typeof(CacheBase).GetMethod("Receive", new Type[0]).MakeGenericMethod(type);
            return ((IEnumerable<object>) method.Invoke(null, new object[0])).ToList();
        }
    }
}