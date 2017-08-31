<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserAssignmentEditor.ascx.cs" Inherits="DesktopModules.Modules.Application.UserAssignmentEditor" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                    runat="server" />
            </div>
            <div class="form-group">
                <div class="col-md-2 control-label">
                    <label>Giai Đoạn</label>
                </div>
                <div class="col-md-4 control-value">
                    <asp:Label runat="server" ID="lblPhase" CssClass="c-font-bold" />
                </div>
                <div class="col-md-1 control-label">
                    <label>User xử lý</label>
                </div>
                <div class="col-md-4">
                    <control:AutoComplete runat="server" ID="ddlUser" EmptyMessage="User xử lý"/>
                </div>
                <div class="col-sm-1"></div>
            </div>
            <div class="form-group">
                <div class="col-md-2 control-label">
                    <label>KPI</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="txtKPI" Width="100px" CssClass="form-control c-theme"></asp:TextBox>
                </div>
                <div class="col-md-1 control-label">
                    <label>Disable</label>
                </div>
                <div class="col-md-4">
                    <control:AutoComplete runat="server" ID="ddlIsDisable">
                        <Items>
                            <control:ComboBoxItem Value="0" Text="No" />
                            <control:ComboBoxItem Value="1" Text="Yes" />
                        </Items>
                    </control:AutoComplete>
                </div>
                <div class="col-sm-1"></div>
            </div>
            <div class="c-margin-t-40 form-group">
                <div class="col-sm-6">
                    <label>Danh sách Chính sách</label>
                </div>
                <div class="col-sm-6">
                    <label>Đang xử lý (Empty = Tất cả Chính sách)</label>
                </div>
                <div class="col-sm-1"></div>
            </div>
            <div class="form-group">
                <div class="col-sm-6">
                    <dnn:DnnListBox 
                        AllowTransfer="true"
                        ButtonSettings-AreaWidth="10%"
                        Height="200px"
                        ID="ListSource"
                        RenderMode="Lightweight"
                        runat="server"
                        SelectionMode="Multiple"
                        TransferToID="ListDestination"
                        EnableDragAndDrop="True"
                        Width="100%">
                    </dnn:DnnListBox>
                </div>
                <div class="col-sm-6">
                    <dnn:DnnListBox 
                        AllowDelete="False"
                        AllowReorder="False"
                        ButtonSettings-AreaWidth="35px"
                        Height="200px"
                        ID="ListDestination"
                        RenderMode="Lightweight"
                        EnableDragAndDrop="True"
                        runat="server"
                        Width="100%">
                    </dnn:DnnListBox>
                </div>
                <div class="col-sm-1"></div>
            </div>
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-10">
                    <asp:Button
                        CssClass="btn btn-primary"
                        ID="btnSave"
                        OnClick="Save"
                        OnClientClick="return onBeforeSave();"
                        runat="server"
                        Text="Cập Nhật" />
                </div>
            </div>
        </div>

        <asp:HiddenField runat="server" ID="hidUserID" Visible="False" />
        <asp:HiddenField runat="server" ID="hidPhaseID" Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function onBeforeSave()
    {
        return validateNumber(getControl("txtKPI"), 10, 100) && validateRadOption("ddlUser");
    }
</script>
