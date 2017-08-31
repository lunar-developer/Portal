using System;
using System.Web.UI.WebControls;
using Modules.Application.Business;
using Modules.Application.DataTransfer;
using Modules.Application.Global;
using Telerik.Web.UI;
using Website.Library.Extension;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionAutoPayInfo : ApplicationFormModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
            }
        }

        protected void QueryAccount(object sender, EventArgs e)
        {
            string cifNo = ctrlPaymentCIFNo.Text.Trim();
            string currentAccountNo = ctrlPaymentAccountNo.SelectedValue;
            ctrlPaymentAccountNo.ClearSelection();
            ctrlPaymentAccountNo.Items.Clear();
            ctrlPaymentAccountName.Text = string.Empty;
            ctrlPaymentBankCode.Text = string.Empty;

            string message;
            CustomerData customer = ApplicationBusiness.QueryAccount(cifNo, out message);
            if (customer == null)
            {
                ShowAlertDialog(message);
            }
            else
            {
                ctrlPaymentAccountName.Text = customer.CustomerName;
                if (customer.Accounts == null)
                {
                    return;
                }

                ctrlPaymentAccountNo.Items.Add(GetEmptyItem());
                foreach (InsensitiveDictionary<string> dataDictionary in customer.Accounts)
                {
                    string accountNo = dataDictionary.GetValue("AccountNo");
                    string branchName = dataDictionary.GetValue("BranchName");
                    RadComboBoxItem item = new RadComboBoxItem(accountNo, accountNo);
                    item.Attributes.Add("BranchName", branchName);
                    ctrlPaymentAccountNo.Items.Add(item);

                    if (currentAccountNo == accountNo)
                    {
                        ctrlPaymentAccountNo.SelectedValue = currentAccountNo;
                    }
                }
                ctrlPaymentBankCode.Text = ctrlPaymentAccountNo.SelectedItem.Attributes["BranchName"];
            }
        }

        protected void ProcessOnSelectAccount(object sender, EventArgs e)
        {
            ctrlPaymentBankCode.Text = ctrlPaymentAccountNo.SelectedItem.Attributes["BranchName"];
        }


        #region PUBLIC PROPERTY

        public Button ControlQueryAccount => ctrlQueryAccount;

        public RadComboBox ControlPaymentMethod => ctrlPaymentMethod;
        public RadComboBox ControlPaymentSource => ctrlPaymentSource;
        public TextBox ControlPaymentCIFNo => ctrlPaymentCIFNo;
        public TextBox ControlPaymentAccountName => ctrlPaymentAccountName;

        public RadComboBox ControlPaymentAccountNo => ctrlPaymentAccountNo;
        public TextBox ControlPaymentBankCode => ctrlPaymentBankCode;
        public RadComboBox ControlAutoPayIndicator => ctrlAutoPayIndicator;

        #endregion
    }
}