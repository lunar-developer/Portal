<%@ Control Language="C#" AutoEventWireup="false" CodeFile="UserDetail.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.UserDetail" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.UserManagement.Database" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<asp:UpdatePanel ID="updatePanel"
                 runat="server">
<ContentTemplate>
<div class="c-content-title-1 clearfix c-margin-b-20 c-title-md">
    <h3 class="c-font-bold c-font-uppercase">THÔNG TIN TÀI KHOẢN</h3>
    <div class="c-bg-blue c-line-left"></div>
</div>
<div class="form-horizontal">
<div class="form-group">
    <asp:PlaceHolder ID="phMessage"
                     runat="server" />
</div>
<div class="dnnTabs"
     id="DivUserInformation"
     runat="server">
<div class="form-group">
    <ul class="dnnAdminTabNav dnnClear">
        <li>
            <a href="#TabGeneralInfo">Thông Tin Cá Nhân</a>
        </li>
        <li id="DivTabRole"
            runat="server">
            <a href="#TabRoleInfo">Thông Tin Phân Quyền</a>
        </li>
        <li id="DivTabPassword"
            runat="server">
            <a href="#TabPasswordInfo">Quản Lý Mật Khẩu</a>
        </li>
        <li id="DivTabHistory"
            runat="server">
            <a href="#TabHistoryInfo">Lịch Sử</a>
        </li>
    </ul>
</div>


<!-- Tab General Information -->
<div class="dnnClear"
     id="TabGeneralInfo">

    <div class="form-group">
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblUserName"
                       runat="server" />
        </div>
        <div class="col-sm-4">
            <div class="c-square input-group">
                <asp:TextBox CssClass="form-control c-theme"
                             ID="txtUserName"
                             runat="server" />
                <span class="input-group-addon">
                    <i class="fa fa-check"></i>
                </span>
            </div>
        </div>
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblUserID"
                       runat="server" />
        </div>
        <div class="col-sm-4 control-value">
            <asp:Label ID="txtUserID"
                       runat="server" />
        </div>
    </div>

    <div class="form-group">
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblDisplayName"
                       runat="server" />
        </div>
        <div class="col-sm-4">
            <div class="c-square input-group">
                <asp:TextBox CssClass="form-control c-theme"
                             ID="txtDisplayName"
                             runat="server" />
                <span class="input-group-addon">
                    <i class="fa fa-check"></i>
                </span>
            </div>
        </div>
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblGender"
                       runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:DropDownList CssClass="form-control c-theme"
                              ID="ddlGender"
                              runat="server">
                <asp:ListItem disabled="disabled"
                              Selected="True"
                              Text="Chưa Chọn"
                              Value="" />
                <asp:ListItem Text="Nam"
                              Value="M" />
                <asp:ListItem Text="Nữ"
                              Value="F" />
            </asp:DropDownList>
        </div>
    </div>

    <div class="form-group">
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblMobile"
                       runat="server" />
        </div>
        <div class="col-sm-4">
            <div class="c-square input-group">
                <asp:TextBox CssClass="form-control c-theme"
                             ID="txtMobile"
                             runat="server" />
                <span class="input-group-addon">
                    <i class="fa fa-check"></i>
                </span>
            </div>
        </div>
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblPhoneExtension"
                       runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="txtPhoneExtension"
                         runat="server" />
        </div>
    </div>

    <div class="form-group">
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblStaffID"
                       runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="txtStaffID"
                         runat="server" />
        </div>
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblTitle1"
                       runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="txtTitle"
                         runat="server" ReadOnly="True" />
        </div>
    </div>

    <div class="form-group">
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblBranch"
                       runat="server" />
        </div>
        <div class="col-sm-4">
            <control:Combobox CssClass="form-control c-theme"
                              OnSelectedIndexChanged="ChangeBranchValue"
                              AutoPostBack="True"
                              ID="ddlBranch"
                              runat="server" />
        </div>
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblLineManager"
                       runat="server" />
        </div>
        <div class="col-sm-4 control-value">
            <asp:Label ID="txtLineManager"
                       runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblStatus"
                       runat="server" />
        </div>
        <div class="col-sm-4 control-value">
            <asp:Label ID="txtAuthorised"
                       runat="server" />
        </div>
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblLastLoginDate"
                       runat="server" />
        </div>
        <div class="col-sm-4 control-value">
            <asp:Label ID="txtLastLoginDate"
                       runat="server" />
        </div>
    </div>
    <hr class="c-margin-t-40 separator" />
    <div class="form-group"
         id="DivProfileRemark"
         runat="server">
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblRemark"
                       runat="server" />
        </div>
        <div class="col-sm-10">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="txtRemark"
                         Rows="2"
                         runat="server"
                         TextMode="MultiLine" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-2"></div>
        <div class="col-sm-10">
            <asp:Button CssClass="btn btn-primary"
                        ID="btnSaveProfile"
                        OnClick="SaveProfile"
                        OnClientClick="return processUpdateProfile();"
                        runat="server"
                        Text="Cập Nhật" />
        </div>
    </div>
</div>


<!-- Tab Roles Information -->
<div class="dnnClear"
     id="TabRoleInfo">
    <div id="DivTabRoleContent"
         runat="server">
        <div class="form-group">
            <div class="row">
                <div class="col-sm-2"></div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblTitle"
                           runat="server" />
                </div>
                <div class="col-sm-4">
                    <control:Combobox CssClass="form-control c-theme" 
                                  OnSelectedIndexChanged="LoadRoleDefault" 
                                  AutoPostBack="True"
                                  ID="ddlTitle"
                                  runat="server" />
                </div>
                <div class="col-sm-4"></div>
            </div>
        </div>
        <div class="dnnPanels"
             id="DivRoles"
             runat="server">
        </div>
        <hr class="c-margin-t-40 separator" />
        <div class="form-group"
             id="DivRoleRemark"
             runat="server">
            <div class="col-sm-2 control-label">
                <dnn:Label ID="lblRoleRemark"
                           runat="server" />
            </div>
            <div class="col-sm-10">
                <asp:TextBox CssClass="form-control c-theme"
                             ID="txtRoleRemark"
                             Rows="2"
                             runat="server"
                             TextMode="MultiLine" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-2"></div>
            <div class="col-sm-10">
                <asp:Button CssClass="btn btn-primary"
                            ID="btnUpdateRole"
                            OnClick="UpdateRole"
                            OnClientClick="return processUpdateRole();"
                            runat="server"
                            Text="Cập Nhật" />
            </div>
        </div>
    </div>
</div>


<!-- Tab Password Information -->
<div class="dnnClear"
     id="TabPasswordInfo">
    <div id="DivTabPasswordContent"
         runat="server">
        <div class="form-group">
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <dnn:Label ID="lblNewPassword"
                                   runat="server" />
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox autocomplete="off"
                                     CssClass="form-control c-theme"
                                     ID="txtNewPassword"
                                     runat="server"
                                     TextMode="Password" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <dnn:Label ID="lblConfirmPassword"
                                   runat="server" />
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox autocomplete="off"
                                     CssClass="form-control c-theme"
                                     ID="txtConfirmPassword"
                                     runat="server"
                                     TextMode="Password" />
                    </div>
                </div>
            </div>
            <div class="col-sm-6"></div>
        </div>
        <hr class="c-margin-t-40 separator" />
        <div class="col-sm-6">
            <div class="form-group">
                <div class="col-sm-4"></div>
                <div class="col-sm-8">
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnUpdatePassword"
                                OnClick="UpdatePassword"
                                OnClientClick="return onProcessUpdatePassword();"
                                runat="server"
                                Text="Cập Nhật" />
                </div>
            </div>
        </div>
        <div class="col-sm-6"></div>
    </div>
</div>


<!-- Tab History Information -->
<div class="dnnClear"
     id="TabHistoryInfo">
    <div id="DivTabHistoryContent"
         runat="server">
        <control:Grid AllowPaging="true"
                      AutoGenerateColumns="False"
                      CssClass="dnnGrid"
                      EnableViewState="true"
                      ID="gridData"
                      OnPageIndexChanged="OnPageIndexChanging"
                      OnPageSizeChanged="OnPageSizeChanging"
                      PageSize="10"
                      runat="server">
            <MasterTableView>
                <Columns>
                    <dnn:DnnGridBoundColumn DataField="LogAction"
                                            HeaderText="LogAction">
                        <HeaderStyle Width="20%" />
                    </dnn:DnnGridBoundColumn>
                    <dnn:DnnGridBoundColumn DataField="Remark"
                                            HeaderText="Remark">
                        <HeaderStyle Width="40%" />
                    </dnn:DnnGridBoundColumn>
                    <dnn:DnnGridTemplateColumn DataField="ModifyUserID"
                                               HeaderText="ModifyUserID">
                        <HeaderStyle Width="20%" />
                        <ItemTemplate>
                            <%#FunctionBase.FormatUserID(Eval(UserTable.ModifyUserID).ToString()) %>
                        </ItemTemplate>
                    </dnn:DnnGridTemplateColumn>
                    <dnn:DnnGridTemplateColumn DataField="ModifyDateTime"
                                               HeaderText="ModifyDateTime">
                        <HeaderStyle Width="20%" />
                        <ItemTemplate>
                            <%#FunctionBase.FormatDate(Eval(UserTable.ModifyDateTime).ToString()) %>
                        </ItemTemplate>
                    </dnn:DnnGridTemplateColumn>
                </Columns>
            </MasterTableView>
        </control:Grid>
    </div>
</div>
</div>
</div>

<asp:HiddenField ID="hidUserID"
                 runat="server"
                 Value="0"
                 Visible="False" />
<asp:HiddenField ID="hidIsAccountLDAP"
                 runat="server"
                 Value="0"
                 Visible="False" />

</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function()
        {
            $(".dnnTabs").dnnTabs({
                selected: 0
            });
            $(".dnnPanels").dnnPanels({
                defaultState: "open"
            });
        },
        true);

    function toggleGroup(element, groupId)
    {
        $(element)
            .closest("table")
            .find("input[group='" + groupId + "']")
            .attr("checked", element.checked);
    }

    function processUpdateProfile()
    {
        var array = ["txtUserName", "txtDisplayName", "txtMobile"];
        if (validateInputArray(array) === false
            || validateEmail(getControl("txtUserName")) === false)
        {
            return false;
        }

        var element = getControl("ddlBranch");
        if (isInvalidOption(element.value, "-1"))
        {
            alertMessage("Vui lòng chọn Chi Nhánh", undefined, undefined, function()
            {
                showError(element, true);
                element.focus();
            });
            return false;
        }

        element = getControl("txtRemark");
        if (element != null && validateInput(element) === false)
        {
            return false;
        }

        showError(element, false);
        return true;
    }

    function processUpdateRole()
    {
        return validateInput(getControl("txtRoleRemark"));
    }

    function processUpdatePassword()
    {
        var array = ["txtNewPassword", "txtConfirmPassword"];
        if (validateInputArray(array) === false)
        {
            var element = getControl("txtNewPassword");
            var newPassword = element.value;
            var confirmPassword = getControl("txtConfirmPassword").value;
            if (newPassword !== confirmPassword)
            {
                alertMessage("Mật Khẩu không giống nhau", undefined, undefined, function()
                {
                    showError(element, true);
                    element.focus();
                });
                return false;
            }
        }

        return true;
    }
</script>