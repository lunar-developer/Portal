using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Enum;
using Modules.UserManagement.Business;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Global;
using Telerik.Web.UI;
using Website.Library.Global;

namespace Modules.Application.Global
{
    public class ApplicationModuleBase : DesktopModuleBase
    {
        protected static readonly string ApplicationEditUrl =
            FunctionBase.GetTabUrl(FunctionBase.GetConfiguration("APP_EditUrl"));


        protected RadComboBoxItem GetEmptyItem()
        {
            RadComboBoxItem item = new RadComboBoxItem("CHƯA CHỌN", string.Empty);
            return item;
        }

        private static void BindItems(RadComboBox dropDownList, RadComboBoxItem[] additionalItems)
        {
            dropDownList.Items.Clear();
            if (additionalItems != null)
            {
                dropDownList.Items.AddRange(additionalItems);
            }
        }

        private static RadComboBoxItem CreateItem(string text, string value, string isDisable)
        {
            RadComboBoxItem item = new RadComboBoxItem(text, value)
            {
                Enabled = FunctionBase.ConvertToBool(isDisable) == false
            };
            return item;
        }

        protected static void BindApplicationTypeData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (ApplicationTypeData cacheData in CacheBase.Receive<ApplicationTypeData>())
            {
                RadComboBoxItem item =
                    CreateItem(cacheData.Name, cacheData.ApplicationTypeID, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindIdentityTypeData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (IdentityTypeData cacheData in CacheBase.Receive<IdentityTypeData>())
            {
                RadComboBoxItem item =
                    CreateItem(cacheData.Name, cacheData.IdentityTypeCode, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindLanguageData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (LanguageData cacheData in CacheBase.Receive<LanguageData>())
            {
                RadComboBoxItem item = CreateItem(cacheData.Name, cacheData.LanguageID, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindCountryData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (CountryData cacheData in CacheBase.Receive<CountryData>())
            {
                RadComboBoxItem item = CreateItem(cacheData.Name, cacheData.CountryCode, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindCustomerClassData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (CustomerClassData cacheData in CacheBase.Receive<CustomerClassData>())
            {
                string text = $"{cacheData.CustomerClassCode} - {cacheData.Name}";
                string value = cacheData.CustomerClassCode;
                RadComboBoxItem item = CreateItem(text, value, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindStateData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            dropDownList.ClearSelection();
            BindItems(dropDownList, additionalItems);
            foreach (StateData cacheData in CacheBase.Receive<StateData>())
            {
                RadComboBoxItem item = CreateItem(cacheData.StateName, cacheData.StateCode, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
            dropDownList.SelectedIndex = 0;
        }

        protected static void BindCityData(RadComboBox dropDownList, string filterID, params RadComboBoxItem[] additionalItems)
        {
            dropDownList.ClearSelection();
            BindItems(dropDownList, additionalItems);
            foreach (CityData cacheData in CacheBase
                .Filter<CityData>(CityTable.StateCode, filterID)
                .OrderBy(item => item.CityName))
            {
                RadComboBoxItem item = CreateItem(cacheData.CityName, cacheData.CityCode, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
            dropDownList.SelectedIndex = 0;
        }

        protected bool IsSensitiveInfo(string status)
        {
            return FunctionBase.IsInArray(status,
                ApplicationStatusEnum.SendToAssessTeam, ApplicationStatusEnum.Assessing,
                ApplicationStatusEnum.SendToApprover, ApplicationStatusEnum.Approved);
        }

        protected void BindBranchData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            dropDownList.Items.AddRange(additionalItems);
            if (IsRoleAssessment())
            {
                BindAllBranchData(dropDownList);
            }
            else
            {
                BindBranchData(dropDownList);
            }
        }

        protected void BindAllBranchData(RadComboBox dropDownList)
        {
            foreach (BranchData branch in BranchBusiness.GetAllBranchInfo())
            {
                if (branch.BranchID == "-1")
                {
                    continue;
                }

                string text = $"{branch.BranchCode} - {branch.BranchName}";
                string value = branch.BranchCode;
                dropDownList.Items.Add(CreateItem(text, value, branch.IsDisable));
            }
        }

        protected void BindBranchData(RadComboBox dropDownList)
        {
            foreach (BranchData branch in UserBusiness.GetUserBranch(UserInfo.UserID.ToString()))
            {
                if (branch.BranchID == "-1")
                {
                    continue;
                }

                string text = $"{branch.BranchCode} - {branch.BranchName}";
                string value = branch.BranchCode;
                dropDownList.Items.Add(CreateItem(text, value, branch.IsDisable));
            }
        }

        protected static void BindContractTypeData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (ContractTypeData cacheData in CacheBase.Receive<ContractTypeData>())
            {
                string text = $"{cacheData.ContractTypeCode} - {cacheData.ContractTypeName}";
                string value = cacheData.ContractTypeCode;
                RadComboBoxItem item = CreateItem(text, value, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindCorporateEntityTypeData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (CorporateEntityTypeData cacheData in CacheBase.Receive<CorporateEntityTypeData>())
            {
                string text = $"{cacheData.CorporateEntityTypeCode} - {cacheData.CorporateEntityTypeName}";
                string value = cacheData.CorporateEntityTypeCode;
                RadComboBoxItem item = CreateItem(text, value, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindCorporateSizeData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (CorporateSizeData cacheData in CacheBase.Receive<CorporateSizeData>())
            {
                string text = cacheData.CorporateSizeName;
                string value = cacheData.CorporateSizeCode;
                RadComboBoxItem item = CreateItem(text, value, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindCorporateStatusData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (CorporateStatusData cacheData in CacheBase.Receive<CorporateStatusData>())
            {
                string text = $"{cacheData.CorporateStatusCode} - {cacheData.CorporateStatusName}";
                string value = cacheData.CorporateStatusCode;
                RadComboBoxItem item = CreateItem(text, value, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindOccupationData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (OccupationData cacheData in CacheBase.Receive<OccupationData>())
            {
                string text = $"{cacheData.OccupationCode} - {cacheData.OccupationName}";
                string value = cacheData.OccupationCode;
                RadComboBoxItem item = CreateItem(text, value, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindSICData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (SICData cacheData in CacheBase.Receive<SICData>())
            {
                string text = $"{cacheData.SICCode} - {cacheData.SICName}";
                string value = cacheData.SICCode;
                RadComboBoxItem item = CreateItem(text, value, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindEducationData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (EducationData cacheData in CacheBase.Receive<EducationData>())
            {
                string text = $"{cacheData.EducationCode} - {cacheData.EducationName}";
                string value = cacheData.EducationCode;
                RadComboBoxItem item = CreateItem(text, value, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindHomeOwnershipData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (HomeOwnershipData cacheData in CacheBase.Receive<HomeOwnershipData>())
            {
                string text = $"{cacheData.HomeOwnershipCode} - {cacheData.HomeOwnershipName}";
                string value = cacheData.HomeOwnershipCode;
                RadComboBoxItem item = CreateItem(text, value, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindMaritalStatusData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (MaritalStatusData cacheData in CacheBase.Receive<MaritalStatusData>())
            {
                string text = $"{cacheData.MaritalStatusCode} - {cacheData.MaritalStatusName}";
                string value = cacheData.MaritalStatusCode;
                RadComboBoxItem item = CreateItem(text, value, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }

        protected static void BindPositionData(RadComboBox dropDownList, params RadComboBoxItem[] additionalItems)
        {
            BindItems(dropDownList, additionalItems);
            foreach (PositionData cacheData in CacheBase.Receive<PositionData>())
            {
                string text = $"{cacheData.PositionCode} - {cacheData.PositionName}";
                string value = cacheData.PositionCode;
                RadComboBoxItem item = CreateItem(text, value, cacheData.IsDisable);
                dropDownList.Items.Add(item);
            }
        }








        protected void BindBranchData(DropDownList dropDownList, bool isHasOptionAll = false, params ListItem[] additionalItems)
        {
            UserManagementModuleBase.BindBranchData(dropDownList, UserInfo.UserID.ToString(), true, isHasOptionAll, new List<string>(), additionalItems);
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




        //private static List<PhaseMappingData> GetListPhaseMappingByApplicationCode(string applicationTypeCode)
        //{
        //    List<PhaseMappingListData> phaseMappingCache = CacheBase.Receive<PhaseMappingListData>();
        //    if (phaseMappingCache != null && phaseMappingCache.Count > 0)
        //    {
        //        foreach (PhaseMappingListData item in phaseMappingCache)
        //        {
        //            if (item.ApplicationTypeCode == applicationTypeCode)
        //            {
        //                return item.PhaseListMapping;

        //            }
        //        }
        //    }
        //    return null;
        //}
        //protected static void BindPhaseData(DropDownList dropDownList, string applicationTypeCode, string selectedValue = null)
        //{
        //    List<PhaseMappingData> phaseList = GetListPhaseMappingByApplicationCode(applicationTypeCode);
        //    if (phaseList != null && phaseList.Count > 0)
        //    {
        //        foreach (PhaseMappingData phaseData in phaseList)
        //        {
        //            ListItem item = new ListItem(phaseData.Name, phaseData.PhaseCode)
        //            {
        //                Selected = !string.IsNullOrWhiteSpace(selectedValue) &&
        //                    phaseData.PhaseCode.Equals(selectedValue)
        //            };
        //            dropDownList.Items.Add(item);
        //        }
        //    }
        //}

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
            if (policyData == null || policyData.Count == 0) return null;

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
        private static void BindPolicyListItemSelected(ListBox listbox, string policyCode)
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
        protected static void BindPolicyData(ListBox listbox, string policyCode, bool isAssign)
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

        //private static List<ListItem> GetListItemUserApplic(string phaseCode)
        //{
        //    List<ListItem> listItem = new List<ListItem>();
        //    List<UserApplicationData> userList = UserApplicationBusiness.GetUserApplic();
        //    if (userList == null || userList.Count == 0) return listItem;
        //    foreach (UserApplicationData user in userList)
        //    {
        //        if (user.PhaseCode.Equals(phaseCode))
        //        {
        //            ListItem item = new ListItem($"{user.FullName}<{user.Username}>", user.UserID);
        //            if (listItem.Contains(item))
        //            {
        //                listItem.Remove(item);
        //            }
        //            listItem.Add(item);
        //        }

        //    }
        //    return listItem;
        //}
        //protected static void BinUserMappingData(DropDownList dropDownList, string phaseCode)
        //{
        //    List<ListItem> listItem = GetListItemUserApplic (phaseCode);
        //    if (listItem != null && listItem.Count > 0)
        //    {
        //        foreach (ListItem item in listItem)
        //        {
        //            dropDownList.Items.Add(item);
        //        }
        //    }

        //}

        //protected static void BindAllUserData(DropDownList dropDownList,string selectedValue)
        //{
        //    List<UserApplicationData> userList = UserApplicationBusiness.GetUserApplic();
        //    foreach (UserApplicationData user in userList)
        //    {
        //        ListItem item = new ListItem(user.FullName, user.UserID)
        //        {
        //            Selected = !string.IsNullOrWhiteSpace(selectedValue) &&
        //                user.UserID.Equals(selectedValue)
        //        };
        //        dropDownList.Items.Add(item);
        //    }
        //}

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


        protected string FormatStatus(string status)
        {
            return ApplicationStatusEnum.GetStatusDescription(status);
        }
    }
}