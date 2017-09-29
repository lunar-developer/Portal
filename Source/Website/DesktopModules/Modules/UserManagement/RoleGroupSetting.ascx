<%@ Control Language="C#" AutoEventWireup="false" CodeFile="RoleGroupSetting.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.RoleGroupSetting" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2 col-sm-0"></div>
                <div class="col-md-2 col-sm-6 control-label">
                    <label>Nhóm Quyền</label>
                </div>
                <div class="col-md-5 col-sm-6">
                    <control:AutoComplete
                        autocomplete="off"
                        AutoPostBack="True"
                        EmptyMessage="Vui lòng chọn Nhóm Quyền"
                        ID="ddlRoleGroup"
                        OnSelectedIndexChanged="LoadSettings"
                        runat="server" />
                </div>
                <div class="col-md-3 col-sm-0"></div>
            </div>
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                    runat="server" />
            </div>
            <div class="c-margin-t-40 form-group">
                <div class="col-sm-1"></div>
                <div class="col-sm-5">
                    <label>Danh sách Chi Nhánh</label>
                </div>
                <div class="col-sm-5">
                    <label>Chi Nhánh đang áp dụng</label>
                </div>
                <div class="col-sm-1"></div>
            </div>
            <div class="form-group">
                <div class="col-sm-1"></div>
                <div class="col-sm-5">
                    <dnn:DnnListBox AllowTransfer="true"
                        AllowTransferOnDoubleClick="True"
                        ButtonSettings-AreaWidth="10%"
                        Height="200px"
                        ID="ListSource"
                        RenderMode="Lightweight"
                        runat="server"
                        SelectionMode="Multiple"
                        TransferToID="ListDestination"
                        Width="100%">
                    </dnn:DnnListBox>
                </div>
                <div class="col-sm-5">
                    <dnn:DnnListBox AllowDelete="False"
                        AllowTransferOnDoubleClick="True"
                        AllowReorder="False"
                        ButtonSettings-AreaWidth="35px"
                        Height="200px"
                        ID="ListDestination"
                        RenderMode="Lightweight"
                        runat="server"
                        Width="100%">
                    </dnn:DnnListBox>
                </div>
                <div class="col-sm-1"></div>
            </div>

            <div class="form-group">
                <div class="col-sm-1"></div>
                <div class="col-sm-11">
                    <asp:Button CssClass="btn btn-primary"
                        ID="btnUpdate"
                        OnClick="Save"
                        runat="server"
                        Text="Cập Nhật" />
                    <asp:Button CssClass="btn btn-success"
                        ID="btnRefresh"
                        OnClick="Refresh"
                        runat="server"
                        Text="Refresh" />
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
