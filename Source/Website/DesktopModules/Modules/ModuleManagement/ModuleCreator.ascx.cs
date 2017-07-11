using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Definitions;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.UI.Skins.Controls;
using Telerik.Web.UI;
using Website.Library.Global;

namespace DesktopModules.Modules.ModuleManagement
{
    public partial class ModuleCreator : DesktopModuleBase
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
            gridData.LocalResourceFile = LocalResourceFile;
            gridData.Visible = btnInsert.Visible = false;

            // Tab Info
            foreach (TabInfo tab in TabController.Instance.GetTabsByPortal(PortalId).Values)
            {
                if (tab.IsSuperTab && tab.DisableLink)
                {
                    continue;
                }

                string value = tab.TabID.ToString();
                string text = $"{value} - {tab.TabName}";
                ddlTab.Items.Add(new RadComboBoxItem(text, value));
            }

            // Module Info
            foreach (DesktopModuleInfo module in DesktopModuleController.GetDesktopModules(PortalId).Values)
            {
                if (module.ModuleName.StartsWith("Modules.") == false)
                {
                    continue;
                }

                string value = module.DesktopModuleID.ToString();
                string text = $"{value} - {module.ModuleName}";
                ddlModule.Items.Add(new RadComboBoxItem(text, value));
            }
        }

        protected void LoadTabModule(object sender = null, EventArgs e = null)
        {
            if (string.IsNullOrWhiteSpace(ddlTab.SelectedValue))
            {
                gridData.Visible = btnInsert.Visible = false;
                return;
            }

            int tabID = int.Parse(ddlTab.SelectedValue);
            TabInfo tab = TabController.Instance.GetTab(tabID, PortalId);
            gridData.DataSource = tab.Modules;
            gridData.DataBind();
            gridData.Visible = btnInsert.Visible = true;
        }

        protected void LoadModuleDefinition(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlModule.SelectedValue))
            {
                return;
            }

            ddlDefinition.Items.Clear();
            ddlDefinition.ClearSelection();
            int desktopModuleID = int.Parse(ddlModule.SelectedValue);
            foreach (ModuleDefinitionInfo definition
                in ModuleDefinitionController.GetModuleDefinitionsByDesktopModuleID(desktopModuleID).Values)
            {
                string value = definition.ModuleDefID.ToString();
                string text = $"{value} - {definition.DefinitionName}";
                RadComboBoxItem item = new RadComboBoxItem(text, value);
                item.Attributes.Add("DefinitionName", definition.DefinitionName);
                ddlDefinition.Items.Add(item);
            }
        }

        protected void Insert(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlDefinition.SelectedValue))
            {
                ShowAlertDialog("Please select a Definition");
                return;
            }

            int tabID = int.Parse(ddlTab.SelectedValue);
            ModuleInfo module = new ModuleInfo
            {
                TabID = tabID,
                PortalID = PortalId,
                ModuleDefID = int.Parse(ddlDefinition.SelectedValue),
                ModuleTitle = ddlDefinition.SelectedItem.Attributes["DefinitionName"],
                CacheMethod = string.Empty,
                DisplayPrint = false,
                InheritViewPermissions = true,
                IsShareable = true,
                IsShareableViewOnly = true,
                PaneName = "ContentPane"
            };
            int result = ModuleController.Instance.AddModule(module);
            if (result > 0)
            {
                TabController.Instance.ClearCache(PortalId);
                LoadTabModule();
                ShowMessage("Add module defintion success.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage("Add module definition fail.", ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }
}