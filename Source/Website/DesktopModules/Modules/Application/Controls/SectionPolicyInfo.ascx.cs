using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Global;
using Modules.Controls;
using Telerik.Web.UI;
using Website.Library.Global;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionPolicyInfo : ApplicationFormModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            BindData();
        }

        private void BindData()
        {
            BindPolicyData(ctrlPolicyCode, GetEmptyItem());
            BindCountryData(ctrlVisaAllocationCountry);
            BindCountryData(ctrlStudyingAbroadCountry);

            // Bind City of HO CHI MINH (79) & HA NOI (01)
            List<CityData> listCity = CacheBase.Receive<CityData>();
            foreach (CityData city in listCity)
            {
                if (city.StateCode == "79" || city.StateCode == "01")
                {
                    ctrlSchoolInDistrict.Items.Add(new RadComboBoxItem(city.CityName));
                }
            }
        }

        public void ProcessOnPolicyCodeChange(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            List<PolicyFieldData> oldResult = CacheBase.Filter<PolicyFieldData>(PolicyFieldTable.PolicyCode, e.OldValue);
            if (oldResult.Count > 0)
            {
                foreach (PolicyFieldData policyField in oldResult)
                {
                    System.Web.UI.Control control = FindControl(policyField.FieldName);
                    if (control == null)
                        continue;
                    else
                        control.Visible = false;
                }
            }

            string policyCode = ctrlPolicyCode.SelectedValue;
            List<PolicyFieldData> result = CacheBase.Filter<PolicyFieldData>(PolicyFieldTable.PolicyCode, policyCode);

            foreach (PolicyFieldData policyField in result)
            {
                string fieldName = policyField.FieldName;
                System.Web.UI.Control control = FindControl(fieldName);
                DoubleLabel label = FindControl<DoubleLabel>(control);


                if (control == null)
                    continue;

                control.Visible = true;
                if (label != null)
                    label.IsRequire = FunctionBase.ConvertToBool(policyField.IsRequire);
            }
        }


        #region PUBLIC PROPERTY

        public RadComboBox ControlPolicyCode => ctrlPolicyCode;
        public TextBox ControlNumOfDocument => ctrlNumOfDocument;
        public TextBox ControlMembershipID => ctrlMembershipID;
        public TextBox ControlGuarantorName => ctrlGuarantorName;


        #endregion
    }
}