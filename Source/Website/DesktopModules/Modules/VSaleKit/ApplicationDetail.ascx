<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ApplicationDetail.ascx.cs"
Inherits="DesktopModules.Modules.VSaleKit.ApplicationDetail" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude FilePath="DesktopModules/Modules/VSaleKit/Assets/css/lightbox.css"
                   ForceBundle="True"
                   ForceVersion="True"
                   runat="server" />
<dnn:DnnJsInclude FilePath="DesktopModules/Modules/VSaleKit/Assets/js/lightbox.js"
                  ForceBundle="True"
                  ForceVersion="True"
                  runat="server" />


<asp:UpdatePanel ID="updatePanel"
                 runat="server">
<ContentTemplate>
<div class="form-horizontal">
<div id="PanelApplication">
    <div class="form-group">
        <ul class="dnnAdminTabNav dnnClear">
            <li>
                <a href="#tabGeneralInfo">Thông tin hồ sơ</a>
            </li>
            <li>
                <a href="#<%= tabDocumentInfo.ClientID %>">Chứng từ</a>
            </li>
            <li>
                <a href="#tabHistoryInfo">Lịch sử</a>
            </li>
        </ul>
    </div>

    <!-- Tab General Information -->
    <div class="dnnClear"
         id="tabGeneralInfo">
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <dnn:Label ID="lblCustomerName"
                           runat="server" />
            </div>
            <div class="col-sm-4">
                <asp:TextBox CssClass="form-control c-theme"
                             ID="txtCustomerName"
                             runat="server" />
            </div>
            <div class="col-sm-2 control-label">
                <dnn:Label ID="lblMobileNumber"
                           runat="server" />
            </div>
            <div class="col-sm-4">
                <asp:TextBox CssClass="form-control c-theme"
                             ID="txtMobileNumber"
                             runat="server" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <dnn:Label ID="lblIdentityType"
                           runat="server" />
            </div>
            <div class="col-sm-4">
                <asp:DropDownList CssClass="form-control c-theme"
                                  ID="ddlIdentityType"
                                  runat="server" />
            </div>
            <div class="col-sm-2 control-label">
                <dnn:Label ID="lblCustomerID"
                           runat="server" />
            </div>
            <div class="col-sm-4">
                <asp:TextBox CssClass="form-control c-theme"
                             ID="txtCustomerID"
                             runat="server" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <dnn:Label ID="lblPolicy"
                           runat="server" />
            </div>
            <div class="col-sm-4">
                <control:Combobox CssClass="form-control c-theme"
                                  ID="ddlPolicy"
                                  runat="server" />
            </div>
            <div class="col-sm-2 control-label">
                <dnn:Label ID="lblApplicationType"
                           runat="server" />
            </div>
            <div class="col-sm-4">
                <asp:DropDownList CssClass="form-control c-theme"
                                  ID="ddlApplicationType"
                                  runat="server" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <dnn:Label ID="lblProposalLimit"
                           runat="server" />
            </div>
            <div class="col-sm-4">
                <asp:TextBox CssClass="form-control c-theme"
                             ID="txtProposalLimit"
                             runat="server" />
            </div>
            <div class="col-sm-2 control-label">
                <dnn:Label ControlName="chkIsHighPriority"
                           ID="lblIsHighPriority"
                           runat="server" />
            </div>
            <div class="col-sm-4">
                <div class="c-checkbox has-info">
                    <asp:CheckBox Checked="True"
                                  ID="chkIsHighPriority"
                                  runat="server" />
                    <label for="<%= chkIsHighPriority.ClientID %>">
                    <span class="inc"></span>
                    <span class="check"></span>
                    <span class="box"></span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <dnn:Label ID="lblStatusLabel"
                           runat="server" />
            </div>
            <div class="col-sm-4 control-value">
                <asp:Label ID="lblStatus"
                           runat="server" />
            </div>
            <div class="col-sm-2 control-label"></div>
            <div class="col-sm-4"></div>
        </div>
    </div>

    <!-- Tab Document -->
    <div class="dnnClear"
         id="tabDocumentInfo"
         runat="server">
    </div>

    <!-- Tab History -->
    <div class="dnnClear"
         id="tabHistoryInfo">
        <div class="form-group">
            <div class="col-sm-12">
                <control:Grid AllowPaging="true"
                              AutoGenerateColumns="False"
                              CssClass="dnnGrid"
                              EnableViewState="False"
                              ID="gridLogData"
                              PageSize="10"
                              runat="server">
                    <MasterTableView>
                        <Columns>
                            <dnn:DnnGridTemplateColumn HeaderText="ModifyUserID">
                                <HeaderStyle Width="20%" />
                                <ItemTemplate>
                                    <%# FunctionBase.FormatUserID(Eval("ModifyUserID").ToString()) %>
                                </ItemTemplate>
                            </dnn:DnnGridTemplateColumn>
                            <dnn:DnnGridTemplateColumn DataField="ActionCode"
                                                       HeaderText="ActionCode">
                                <HeaderStyle Width="20%" />
                                <ItemTemplate>
                                    <%# GetResource(Eval("ActionCode").ToString()) %>
                                </ItemTemplate>
                            </dnn:DnnGridTemplateColumn>
                            <dnn:DnnGridBoundColumn DataField="Remark"
                                                    HeaderText="Remark" />
                            <dnn:DnnGridTemplateColumn HeaderText="ModifyDateTime">
                                <HeaderStyle Width="16%" />
                                <ItemTemplate>
                                    <%# FunctionBase.FormatDate(Eval("ModifyDateTime").ToString()) %>
                                </ItemTemplate>
                            </dnn:DnnGridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </control:Grid>
            </div>
        </div>
    </div>
</div>

<div id="PanelNavigator"
     runat="server">
    <div class="form-group">
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblProcess"
                       runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:DropDownList CssClass="form-control c-theme"
                              ID="ddlAction"
                              runat="server"/>
        </div>
        <div class="col-sm-2 control-label"></div>
        <div class="col-sm-4"></div>
    </div>
    <div class="form-group">
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblRemark"
                       runat="server" />
        </div>
        <div class="col-sm-10">
            <asp:TextBox CssClass="form-control c-theme"
                         Height="100"
                         ID="txtRemark"
                         runat="server"
                         TextMode="MultiLine" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-2"></div>
        <div class="col-sm-10">
            <asp:Button CssClass="btn btn-primary"
                        ID="btnProcess"
                        OnClick="btnProcess_Click"
                        OnClientClick="return onProcess(arguments[1])"
                        runat="server"
                        Text="Đồng ý" />
        </div>
    </div>
</div>

<asp:HiddenField ID="hidApplicationID"
                 runat="server"
                 Visible="False" />
<asp:HiddenField ID="hidProcessID"
                 runat="server"
                 Visible="False" />
<asp:HiddenField ID="hidPhaseID"
                 runat="server"
                 Visible="False" />
</div>
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function()
    {
        $("#PanelApplication").dnnTabs();
    }, true);

    function onProcess(isConfirmed)
    {
        if (isConfirmed === true)
        {
            return true;
        }

        var element = getControl("ddlAction");
        var text = element.options[element.selectedIndex].text;
        var message = "Bạn có chắc muốn " + text.toLowerCase() + "?";
        var msg = {
            text: message,
            title: "Nhắc nhở",
            yesText: "Đồng ý",
            noText: "Hủy",
            buttonYesClass: "button button-primary",
            buttonNoClass: "button button-secondary",
            draggable: true,
            isButton: true,
            callbackTrue: callBackTrue
        };
        $.dnnConfirm(msg);
        return false;
    }

    function callBackTrue()
    {
        getJQueryControl("btnProcess").trigger("click", [true]);
    }
</script>