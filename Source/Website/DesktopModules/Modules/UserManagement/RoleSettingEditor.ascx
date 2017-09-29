<%@ Control Language="C#" AutoEventWireup="false" CodeFile="RoleSettingEditor.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.RoleSettingEditor" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="dnnPanels form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                    runat="server" />
            </div>

            <div class="form-group">
                <div class="col-md-2 col-sm-6 control-label">
                    <dnn:Label runat="server"
                        HelpText="Nhóm Quyền"
                        Text="Nhóm Quyền" />
                </div>
                <div class="col-md-4 col-sm-12">
                    <control:AutoComplete
                        AutoPostBack="True"
                        EmptyMessage="Nhóm Quyền"
                        OnSelectedIndexChanged="ProcessOnSelectRoleGroup"
                        ID="ddlRoleGroup"
                        runat="server" />
                </div>
                <div class="col-md-1 col-sm-6 control-label">
                    <dnn:Label runat="server"
                        HelpText="Quyền"
                        Text="Quyền" />
                </div>
                <div class="col-md-4 col-sm-12">
                    <control:AutoComplete
                        AutoPostBack="True"
                        EmptyMessage="Quyền"
                        OnSelectedIndexChanged="ProcessOnSelectRole"
                        ID="ddlRole"
                        runat="server" />
                </div>
                <div class="col-md-1 col-sm-0"></div>
            </div>

            <h2 class="dnnFormSectionHead">
                <a href="#">THÔNG TIN CƠ BẢN</a>
            </h2>
            <fieldset>
                <div class="form-group">
                    <div class="col-sm-2 control-label">
                        <dnn:Label runat="server"
                            ID="lblRoleName"
                            HelpText="Role Name"
                            Text="Tên quyền" />
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox CssClass="form-control c-theme"
                            ID="txtRoleName"
                            Enabled="False"
                            runat="server" />
                    </div>
                    <div class="col-sm-1 control-label">
                        <dnn:Label runat="server"
                            ID="lblDescription"
                            HelpText="Description"
                            Text="Mô tả" />
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox CssClass="form-control c-theme"
                            ID="txtDescription"
                            Enabled="False"
                            runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-2 control-label">
                        <dnn:Label runat="server"
                            ID="lblIsVisibleForBranch"
                            HelpText="Phạm vi áp dụng"
                            Text="Phạm vi áp dụng" />
                    </div>
                    <div class="col-sm-4">
                        <control:AutoComplete ID="ddlRoleScope" runat="server"/>
                    </div>
                    <div class="col-sm-1 control-label">
                        <dnn:Label runat="server"
                            ID="lblIsDisable"
                            HelpText="Disable"
                            Text="Vô hiệu" />
                    </div>
                    <div class="col-sm-4">
                        <div class="c-checkbox has-info">
                            <asp:CheckBox
                                ID="chkIsDisable"
                                runat="server" />
                            <label for="<%= chkIsDisable.ClientID %>">
                            <span class="inc"></span>
                            <span class="check"></span>
                            <span class="box"></span>
                        </div>
                    </div>
                </div>
            </fieldset>
            <h2 class="dnnFormSectionHead">
                <a href="#">THÔNG TIN CÁC QUYỀN KHÔNG TƯƠNG THÍCH</a>
            </h2>
            <fieldset>
                <div class="form-group">
                    <div class="col-sm-1"></div>
                    <div class="col-sm-5">
                        <label>Danh Sách Tất Cả Quyền</label>
                        <dnn:DnnListBox AllowTransfer="true"
                            AllowTransferOnDoubleClick="True"
                            ButtonSettings-AreaWidth="10%"
                            Height="200px"
                            ID="ListAllRole"
                            RenderMode="Lightweight"
                            runat="server"
                            SelectionMode="Multiple"
                            TransferToID="ListExcludeRole"
                            Width="100%">
                        </dnn:DnnListBox>
                    </div>
                    <div class="col-sm-5">
                        <label>Danh Sách Các Quyền Không Tương Thích</label>
                        <dnn:DnnListBox AllowDelete="False"
                            AllowTransferOnDoubleClick="True"
                            AllowReorder="False"
                            ButtonSettings-AreaWidth="35px"
                            Height="200px"
                            ID="ListExcludeRole"
                            RenderMode="Lightweight"
                            SelectionMode="Multiple"
                            TransferToID="ListAllRole"
                            runat="server"
                            Width="100%">
                        </dnn:DnnListBox>
                    </div>
                    <div class="col-sm-1"></div>
                </div>
            </fieldset>
            
            <div class="form-group">
                <div class="col-sm-1"></div>
                <div class="col-sm-11">
                    <asp:Button CssClass="btn btn-primary"
                        ID="btnUpdate"
                        OnClick="Save"
                        runat="server"
                        Visible="False"
                        Text="Cập Nhật" />
                    <asp:Button CssClass="btn btn-danger"
                        ID="btnDelete"
                        OnClick="Delete"
                        runat="server"
                        Visible="False"
                        Text="Xóa" />
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function ()
    {
        $(".dnnPanels").dnnPanels({ defaultState: "open" });

        registerConfirm({
            jquery: "#" + getClientID("btnDelete"),
            isUseRuntimeMessage: true,
            getRunTimeMessage: function ()
            {
                var value = getControl("txtRoleName").value;
                return "Bạn có chắc muốn xóa cấu hình phân Quyền <b>" + value + "</b>?";
            }
        });
    }, true);
</script>
