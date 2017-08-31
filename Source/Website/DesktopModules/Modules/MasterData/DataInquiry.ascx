<%@ Control Language="C#" AutoEventWireup="False" CodeFile="DataInquiry.ascx.cs" Inherits="DesktopModules.Modules.MasterData.DataInquiry" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<style type="text/css">
    .btnDelete
    {
        display: block;
    }
</style>

<asp:UpdatePanel ID="UpdatePanel"
    runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                    runat="server" />
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblSetting"
                        runat="server" />
                </div>
                <div class="col-sm-4">
                    <control:AutoComplete
                        ID="ddlDataTable"
                        EmptyMessage="Danh Mục/Cấu Hình"
                        runat="server"
                        OnClientSelectedIndexChanged="processOnChange"
                        Width="300px" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-10">
                    <asp:Button CssClass="btn btn-primary"
                        ID="btnLoad"
                        OnClick="LoadData"
                        OnClientClick="return processOnLoad();"
                        runat="server"
                        Text="Xem" />
                    <asp:Button CssClass="btn btn-success"
                        ID="btnAdd"
                        OnClick="CreateData"
                        runat="server"
                        Text="Thêm" />
                    <asp:Button CssClass="btn btn-default"
                        ID="btnExport"
                        OnClick="Export"
                        runat="server"
                        Text="Export" />

                    <asp:Button CssClass="btn btn-primary invisible"
                        ID="btnRefresh"
                        OnClick="Refresh"
                        runat="server"
                        Text="Refresh" />
                </div>
            </div>
            <div class="c-margin-t-50 form-group table-responsive">
                <control:Grid
                    ID="gridData"
                    AutoGenerateColumns="true"
                    AllowFilteringByColumn="True"
                    OnItemCommand="ProcessGridDataOnItemCommand"
                    OnItemDataBound="ProcessGridDataOnItemDataBound"
                    OnNeedDataSource="ProcessGridDataOnNeedDataSource"
                    OnColumnCreated="ProcessGridDataOnColumnCreated"
                    runat="server"
                    Visible="False">
                    <MasterTableView>
                        <Columns>
                            <dnn:DnnGridTemplateColumn
                                AllowFiltering="False"
                                UniqueName="EditButton">
                                <HeaderStyle Width="30" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="EditRow">
                                        <i class="fa fa-pencil icon-primary"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </dnn:DnnGridTemplateColumn>
                            <dnn:DnnGridTemplateColumn
                                AllowFiltering="False"
                                UniqueName="DeleteButton">
                                <HeaderStyle Width="30" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" class="btnDelete" CommandName="DeleteRow">
                                        <i class="fa fa-trash-o icon-danger"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </dnn:DnnGridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </control:Grid>
                <control:Grid
                    ID="gridDataReserve"
                    AutoGenerateColumns="true"
                    AllowFilteringByColumn="True"
                    OnItemCommand="ProcessGridDataOnItemCommand"
                    OnItemDataBound="ProcessGridDataOnItemDataBound"
                    OnNeedDataSource="ProcessGridDataOnNeedDataSource"
                    OnColumnCreated="ProcessGridDataOnColumnCreated"
                    runat="server"
                    Visible="False">
                    <MasterTableView>
                        <Columns>
                            <dnn:DnnGridTemplateColumn
                                AllowFiltering="False"
                                UniqueName="EditButton">
                                <HeaderStyle Width="30" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="EditRow">
                                        <i class="fa fa-pencil icon-primary"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </dnn:DnnGridTemplateColumn>
                            <dnn:DnnGridTemplateColumn
                                AllowFiltering="False"
                                UniqueName="DeleteButton">
                                <HeaderStyle Width="30" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" class="btnDelete" CommandName="DeleteRow">
                                        <i class="fa fa-trash-o icon-danger"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </dnn:DnnGridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </control:Grid>
            </div>
        </div>

        <asp:HiddenField ID="hidTableID"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidConnectionName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidDatabaseName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidSchemaName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidTableName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidPrimaryKey"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidAssemblyName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidCacheName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidCacheID"
            runat="server"
            Visible="False" />

        <asp:HiddenField runat="server" ID="hidGridID"/>

    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function processOnLoad()
    {
        return validateRadOption("ddlDataTable");
    }

    function processOnChange()
    {
        getJQueryControl("btnAdd").hide();
        getJQueryControl("btnExport").hide();
    }

    function refresh()
    {
        getControl("btnRefresh").click();
    }

    addPageLoaded(function ()
    {
        confirmMessage("a.btnDelete", "Bạn có chắc muốn <b>XÓA</b> thông tin này?");
    }, true);
</script>
