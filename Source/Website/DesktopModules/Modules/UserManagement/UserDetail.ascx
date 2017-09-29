<%@ Control Language="C#" AutoEventWireup="false" CodeFile="UserDetail.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.UserDetail" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.UserManagement.Database" %>
<%@ Import Namespace="Website.Library.Database" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register Src="~/controls/Label.ascx" TagName="Label" TagPrefix="control" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
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
                            <a href="#TabHistoryInfo">Lịch Sử Thay Đổi</a>
                        </li>
                    </ul>
                </div>

                <!-- Tab General Information -->
                <div class="dnnClear"
                    id="TabGeneralInfo">
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label ID="lblUserName"
                                IsRequire="True"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox CssClass="form-control c-theme"
                                ID="txtUserName"
                                runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <control:Label ID="lblUserID"
                                runat="server" />
                        </div>
                        <div class="col-sm-4 control-value">
                            <asp:Label ID="txtUserID"
                                runat="server" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label ID="lblDisplayName"
                                IsRequire="True"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox CssClass="form-control c-theme"
                                ID="txtDisplayName"
                                runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <control:Label ID="lblGender"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <control:AutoComplete
                                ID="ddlGender"
                                runat="server">
                                <Items>
                                    <control:ComboBoxItem Value="M" Text="Nam" />
                                    <control:ComboBoxItem Value="F" Text="Nữ" />
                                </Items>
                            </control:AutoComplete>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label ID="lblMobile"
                                IsRequire="True"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox CssClass="form-control c-theme"
                                ID="txtMobile"
                                runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <control:Label ID="lblPhoneExtension"
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
                            <control:Label ID="lblStaffID"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox CssClass="form-control c-theme"
                                ID="txtStaffID"
                                runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <control:Label ID="lblTitle"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <control:AutoComplete
                                ID="ddlTitle"
                                runat="server">
                                <Items>
                                    <control:ComboBoxItem Value="CTV" Text="Cộng Tác Viên" />
                                    <control:ComboBoxItem Value="NV" Text="Nhân Viên" />
                                    <control:ComboBoxItem Value="CV" Text="Chuyên Viên" />
                                    <control:ComboBoxItem Value="TBP" Text="Trưởng Bộ Phận" />
                                    <control:ComboBoxItem Value="PGD" Text="Phó Giám Đốc" />
                                    <control:ComboBoxItem Value="GD" Text="Giám Đốc" />
                                </Items>
                            </control:AutoComplete>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label ID="lblBranch"
                                IsRequire="True"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <control:AutoComplete
                                AutoPostBack="True"
                                EmptyMessage="Chi Nhánh"
                                ID="ddlBranch"
                                OnSelectedIndexChanged="ProcessOnBranchChanged"
                                runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <control:Label ID="lblLineManager"
                                runat="server" />
                        </div>
                        <div class="col-sm-4 control-value">
                            <asp:Label ID="txtLineManager"
                                runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label ID="lblAuthorised"
                                runat="server" />
                        </div>
                        <div class="col-sm-4 control-value">
                            <asp:Label ID="txtAuthorised"
                                runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <control:Label ID="lblLastLoginDate"
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
                            <control:Label ID="lblRemark"
                                IsRequire="True"
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
                            <asp:Button CssClass="btn btn-success"
                                ID="btnEnable"
                                OnClick="UpdateAccountStatus"
                                runat="server"
                                CommandArgument="1"
                                CommandName="Kích Hoạt"
                                Text="Kích Hoạt" />
                            <asp:Button CssClass="btn btn-danger"
                                ID="btnDisable"
                                OnClick="UpdateAccountStatus"
                                runat="server"
                                CommandArgument="0"
                                CommandName="Vô Hiệu"
                                Text="Vô Hiệu" />
                            <asp:Button CssClass="btn btn-success"
                                ID="btnUnlock"
                                OnClick="UpdateAccountStatus"
                                runat="server"
                                CommandArgument="2"
                                CommandName="Mở Khóa"
                                Text="Mở Khóa" />
                        </div>
                    </div>
                </div>


                <!-- Tab Roles Information -->
                <div class="dnnClear"
                    id="TabRoleInfo">
                    <div id="DivTabRoleContent"
                        runat="server">
                        <div id="DivRoleTemplate"
                            runat="server">
                            <div class="form-group">
                                <div class="col-sm-2"></div>
                                <div class="col-sm-3 control-label">
                                    <control:Label HelpText="Chọn Quyền mẫu đã thiết lập theo Chức danh"
                                        ID="lblTemplate"
                                        runat="server"
                                        Text="Chọn Quyền theo Chức danh" />
                                </div>
                                <div class="col-sm-4">
                                    <control:AutoComplete
                                        autocomplete="off"
                                        ID="ddlTemplate"
                                        EmptyMessage="Quyền theo Chức danh"
                                        runat="server" />
                                </div>
                                <div class="col-sm-3"></div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-2"></div>
                                <div class="c-center col-sm-7">
                                    <a class="btn btn-primary"
                                        href="javascript:;"
                                        id="btnApply">
                                        <i class="fa fa-gear"></i>Apply
                                    </a>
                                    <a class="btn btn-success"
                                        href="javascript:;"
                                        id="btnRestore">
                                        <i class="fa fa-recycle"></i>Restore
                                    </a>
                                    <a class="btn btn-default"
                                        href="javascript:;"
                                        id="btnCollapse"
                                        onclick="collapseAllPanels()">
                                        <i class="fa fa-compress"></i>Collapse
                                    </a>
                                </div>
                                <div class="col-sm-3"></div>
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
                                <control:Label ID="lblRoleRemark"
                                    IsRequire="True"
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
                                <asp:LinkButton class="btn btn-success"
                                    ID="btnRequest"
                                    OnClick="CreateNewRequest"
                                    runat="server">
                                    Yêu cầu thêm quyền
                                </asp:LinkButton>
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
                                <div class="form-group"
                                    id="DivOldPassword"
                                    runat="server">
                                    <div class="col-sm-4 control-label">
                                        <control:Label ID="lblOldPassword"
                                            IsRequire="True"
                                            runat="server" />
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox autocomplete="off"
                                            CssClass="form-control c-theme"
                                            ID="txtOldPassword"
                                            runat="server"
                                            TextMode="Password" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-4 control-label">
                                        <control:Label ID="lblNewPassword"
                                            IsRequire="True"
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
                                        <control:Label ID="lblConfirmPassword"
                                            IsRequire="True"
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
                                        OnClientClick="return processUpdatePassword();"
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
                        <control:Grid
                            AutoGenerateColumns="False"
                            CssClass="dnnGrid"
                            EnableViewState="true"
                            ID="gridData"
                            OnNeedDataSource="ProcessOnNeedDataSource"
                            runat="server">
                            <MasterTableView>
                                <Columns>
                                    <dnn:DnnGridTemplateColumn HeaderText="ViewDetail">
                                        <ItemTemplate>
                                            <%#RenderLogDetail(Eval(UserRequestTable.UserRequestID).ToString()) %>
                                        </ItemTemplate>
                                    </dnn:DnnGridTemplateColumn>
                                    <dnn:DnnGridBoundColumn DataField="LogAction"
                                        HeaderText="LogAction">
                                        <HeaderStyle Width="20%" />
                                    </dnn:DnnGridBoundColumn>
                                    <dnn:DnnGridBoundColumn DataField="Remark"
                                        HeaderText="Remark">
                                        <HeaderStyle Width="40%" />
                                    </dnn:DnnGridBoundColumn>
                                    <telerik:GridTemplateColumn DataField="UserIDModify" SortExpression="UserIDModify"
                                        HeaderText="Người Cập Nhật">
                                        <HeaderStyle Width="25%" />
                                        <ItemTemplate>
                                            <%#FunctionBase.FormatUserID(Eval(BaseTable.UserIDModify).ToString()) %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="DateTimeModify" SortExpression="DateTimeModify"
                                        HeaderText="Ngày Cập Nhật">
                                        <HeaderStyle Width="15%" />
                                        <ItemTemplate>
                                            <%#FunctionBase.FormatDate(Eval(BaseTable.DateTimeModify).ToString()) %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
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
        <asp:HiddenField ID="hidAuthorise"
            runat="server"
            Value="1"
            Visible="False" />

        <asp:HiddenField ID="hidListUserRoles"
            runat="server"
            Value="" />
    </ContentTemplate>
</asp:UpdatePanel>


<script type="text/javascript">
    addPageLoaded(function()
    {
        $(".dnnTabs").dnnTabs();
        $(".dnnPanels").dnnPanels({
            defaultState: "open"
        });

        confirmMessage("#btnApply", "Bạn muốn Apply Quyền theo Chức danh?", undefined, undefined, undefined,
            applyTemplate);
        confirmMessage("#btnRestore", "Bạn muốn Restore Quyền hiện tại của User?", undefined, undefined, undefined,
            resetToDefault);

        $("#" + getClientID("btnEnable") + ", #" + getClientID("btnDisable") + ", #" + getClientID("btnUnlock"))
            .each(function()
            {
                var element = this;
                registerConfirm({
                    jquery: element,
                    onBeforeOpen: function()
                    {
                        return validateInput(getControl("txtRemark"));
                    },
                    isUseRuntimeMessage: true,
                    getRunTimeMessage: function ()
                    {
                        return "Bạn có chắc muốn <b>" + element.value + "</b> tài khoản này?";
                    }
                });
            });
    }, true);

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

        if (validateRadOption("ddlBranch") === false)
        {
            return false;
        }

        var element = getControl("txtRemark");
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
        // Validate old password
        var element = getControl("txtOldPassword");
        if (element != null && validateInput(element) === false)
        {
            return false;
        }

        // Validate new password
        var array = ["txtNewPassword", "txtConfirmPassword"];
        if (validateInputArray(array) === false)
        {
            return false;
        }

        // Validate confirm password
        element = getControl("txtConfirmPassword");
        var confirmPassword = element.value;
        var newPassword = getControl("txtNewPassword").value;
        if (newPassword !== confirmPassword)
        {
            alertMessage("<b>Mật Khẩu mới</b> không chính xác.", undefined, undefined, function ()
            {
                showError(element, true);
                element.focus();
            });
            return false;
        }

        return true;
    }

    function applyTemplate()
    {
        var element = getControl("ddlTemplate");
        if (validateOption(element) === false)
        {
            return;
        }

        // Clean
        $("input[name='Roles']").attr("checked", false);

        // Apply Template
        var array = element.value.split(',');
        for (var i = 0; i < array.length; i++)
        {
            $("#Role" + array[i]).attr("checked", true);
        }
    }

    function resetToDefault()
    {
        // Clean
        $("input[name='Roles']").attr("checked", false);

        // Reset to history
        var array = getControl("hidListUserRoles").value.split(',');
        for (var i = 0; i < array.length; i++)
        {
            $("#Role" + array[i]).attr("checked", true);
        }
    }
</script>