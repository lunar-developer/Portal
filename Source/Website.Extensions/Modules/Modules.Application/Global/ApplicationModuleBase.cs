using System.Collections.Generic;
using System.Web.UI.WebControls;
using Modules.Application.Business;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Enum;
using Modules.UserManagement.Global;
using Website.Library.Global;

namespace Modules.Application.Global
{
    public partial class ApplicationModuleBase : DesktopModuleBase
    {
        private static void BindItems(ListControl dropDownList, ListItem[] additionalItems)
        {
            dropDownList.Items.Clear();
            if (additionalItems != null)
            {
                dropDownList.Items.AddRange(additionalItems);
            }
        }

        private static ListItem CreateItem(string text, string value, string isDisable)
        {
            ListItem item = new ListItem(text, value);
            if (FunctionBase.ConvertToBool(isDisable))
            {
                item.Attributes.Add("disabled", "disabled");
            }
            return item;
        }


        protected static void BindApplicationTypeData(DropDownList dropDownList, params ListItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (ApplicationTypeData cacheData in CacheBase.Receive<ApplicationTypeData>())
            {
                ListItem item = CreateItem(cacheData.Name, cacheData.ApplicationTypeID, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindIdentityTypeData(DropDownList dropDownList, params ListItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (IdentityTypeData cacheData in CacheBase.Receive<IdentityTypeData>())
            {
                ListItem item = CreateItem(cacheData.Name, cacheData.IdentityTypeCode, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindLanguageData(DropDownList dropDownList, params ListItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (LanguageData cacheData in CacheBase.Receive<LanguageData>())
            {
                ListItem item = CreateItem(cacheData.Name, cacheData.LanguageID, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindCountryData(DropDownList dropDownList, params ListItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (CountryData cacheData in CacheBase.Receive<CountryData>())
            {
                ListItem item = CreateItem(cacheData.Name, cacheData.CountryCode, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindCustomerClassData(DropDownList dropDownList, params ListItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (CustomerClassData cacheData in CacheBase.Receive<CustomerClassData>())
            {
                ListItem item = CreateItem(cacheData.Name, cacheData.CustomerClassCode, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected void BindBranchData(DropDownList dropDownList, bool isHasOptionAll = false, params ListItem[] additionalItems)
        {
            UserManagementModuleBase.BindBranchData(dropDownList, UserInfo.UserID.ToString(), true, isHasOptionAll, additionalItems);
        }

        protected static void BindPolicyData(DropDownList dropDownList, params ListItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (PolicyData cacheData in CacheBase.Receive<PolicyData>())
            {
                ListItem item = CreateItem(cacheData.Name, cacheData.PolicyID, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindStateData(DropDownList dropDownList, params ListItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (StateData cacheData in CacheBase.Receive<StateData>())
            {
                ListItem item = CreateItem(cacheData.StateName, cacheData.StateCode, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindCityData(DropDownList dropDownList, string filterID, params ListItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (CityData cacheData in CacheBase.Filter<CityData>(CityTable.StateCode, filterID))
            {
                ListItem item = CreateItem(cacheData.CityName, cacheData.CityCode, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }


        protected bool IsRoleInput()
        {
            return IsInRole(RoleEnum.Input);
        }

        protected bool IsRoleAssessment()
        {
            return IsInRole(RoleEnum.Assessment);
        }

        protected bool IsRoleConfiguration()
        {
            return IsInRole(RoleEnum.Configuration);
        }




        private static List<PhaseMappingData> GetListPhaseMappingByApplicationCode(string applicationTypeCode)
        {
            List<PhaseMappingListData> phaseMappingCache = CacheBase.Receive<PhaseMappingListData>();
            if (phaseMappingCache != null && phaseMappingCache.Count > 0)
            {
                foreach (PhaseMappingListData item in phaseMappingCache)
                {
                    if (item.ApplicationTypeCode == applicationTypeCode)
                    {
                        return item.PhaseListMapping;

                    }
                }
            }
            return null;
        }
        protected static void BindPhaseData(DropDownList dropDownList, string applicationTypeCode, string selectedValue = null)
        {
            List<PhaseMappingData> phaseList = GetListPhaseMappingByApplicationCode(applicationTypeCode);
            if (phaseList != null && phaseList.Count > 0)
            {
                foreach (PhaseMappingData phaseData in phaseList)
                {
                    ListItem item = new ListItem(phaseData.Name, phaseData.PhaseCode)
                    {
                        Selected = !string.IsNullOrWhiteSpace(selectedValue) &&
                            phaseData.PhaseCode.Equals(selectedValue)
                    };
                    dropDownList.Items.Add(item);
                }
            }
        }

        private static string[] SlipStringArray(string str)
        {
            try
            {
                char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                return str.Split(delimiterChars);

            }
            catch
            {
                return null;
            }
            
        }

        private static List<ListItem> ListBoxPolicySelected(string policyCode)
        {
            if (string.IsNullOrWhiteSpace(policyCode)) return null;

            string[] policyArray = SlipStringArray(policyCode);
            if (policyArray == null || policyArray.Length == 0) return null;

            List<PolicyData> policyData = CacheBase.Receive<PolicyData>();
            if(policyData == null || policyData.Count == 0) return null;

            List<ListItem> listItems = new List<ListItem>();
            foreach (PolicyData policy in policyData)
            {
                foreach (string code in policyArray)
                {
                    if (policy.PolicyCode.Equals(code))
                    {
                        listItems.Add(new ListItem(policy.Name, policy.PolicyCode));
                    }
                }
            }
            return listItems;
        }
        private static void BindPolicyListItemSelected(ListBox listbox,string policyCode)
        {
            List<ListItem> listItems = ListBoxPolicySelected(policyCode);
            if (listItems != null && listItems.Count > 0)
            {
                foreach (ListItem item in listItems)
                {
                    listbox.Items.Add(item);
                }
            }
        }

        private static void BindPolicyListItemNonSelected(ListBox listbox, string policyCode)
        {
            List<PolicyData> policyData = CacheBase.Receive<PolicyData>();
            //check: policyData
            if (policyData != null && policyData.Count > 0)
            {
                foreach (PolicyData policy in policyData)
                {
                    listbox.Items.Add(new ListItem(policy.Name, policy.PolicyCode));
                }
                
                List<ListItem> listItems = ListBoxPolicySelected(policyCode);
                if (listItems != null && listItems.Count > 0)
                {
                    foreach (ListItem item in listItems)
                    {
                        listbox.Items.Remove(item);
                    }
                }
            }//end check: policyData
        }
        protected static void BindPolicyData(ListBox listbox, string policyCode, bool isAssign )
        {
            listbox.Items.Clear();
            if (isAssign)
            {
                BindPolicyListItemSelected(listbox, policyCode);
            }
            else
            {
                BindPolicyListItemNonSelected(listbox, policyCode);

            }//end isAssign
            
        }

        private static List<ListItem> GetListItemUserApplic(string phaseCode)
        {
            List<ListItem> listItem = new List<ListItem>();
            List<UserApplicationData> userList = UserApplicationBusiness.GetUserApplic();
            if (userList == null || userList.Count == 0) return listItem;
            foreach (UserApplicationData user in userList)
            {
                if (user.PhaseCode.Equals(phaseCode))
                {
                    ListItem item = new ListItem($"{user.FullName}<{user.Username}>", user.UserID);
                    if (listItem.Contains(item))
                    {
                        listItem.Remove(item);
                    }
                    listItem.Add(item);
                }

            }
            return listItem;
        }
        protected static void BinUserMappingData(DropDownList dropDownList, string phaseCode)
        {
            List<ListItem> listItem = GetListItemUserApplic (phaseCode);
            if (listItem != null && listItem.Count > 0)
            {
                foreach (ListItem item in listItem)
                {
                    dropDownList.Items.Add(item);
                }
            }
            
        }

        protected static void BindAllUserData(DropDownList dropDownList,string selectedValue)
        {
            List<UserApplicationData> userList = UserApplicationBusiness.GetUserApplic();
            foreach (UserApplicationData user in userList)
            {
                ListItem item = new ListItem(user.FullName, user.UserID)
                {
                    Selected = !string.IsNullOrWhiteSpace(selectedValue) &&
                        user.UserID.Equals(selectedValue)
                };
                dropDownList.Items.Add(item);
            }
        }

        protected string FormatIdentityType(string value)
        {
            return CacheBase.Find<IdentityTypeData>(IdentityTypeTable.IdentityTypeID, value)?.Name;
        }

        protected bool IsConfigurationRole()
        {
            return UserInfo.IsInRole(FunctionBase.GetConfiguration("APP_Admin"));
            //return true;
        }
        protected string DeleteIcon => $"{Request.ApplicationPath}/images/delete.gif".Replace(@"//", "/");
        protected string EditIcon => $"{Request.ApplicationPath}/images/edit.gif".Replace(@"//", "/");
        protected string EditPenIcon => $"{Request.ApplicationPath}/images/edit_pen.gif".Replace(@"//", "/");
        protected string AddIcon => $"{Request.ApplicationPath}/images/add.gif".Replace(@"//", "/");
    }
}