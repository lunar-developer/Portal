<%@ Control Language="C#" AutoEventWireup="False" CodeFile="DataInquiry.ascx.cs" Inherits="DesktopModules.Modules.MasterData.DataInquiry" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls"%>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

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
                    <control:Combobox CssClass="form-control c-theme"
                                      ID="ddlDataTable"
                                      placeholder="Cấu hình"
                                      runat="server"
                                      onchange="processOnChange()"
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
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnAdd"
                                OnClick="Create"
                                runat="server"
                                Text="Thêm" />
                    <asp:Button CssClass="btn btn-primary"
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
            <div class="c-margin-t-50 form-group">
                <div style="width: 100%; overflow: auto;">
                    <control:Grid AllowPaging="True"
                                  AutoGenerateColumns="true"
                                  CssClass="dnnGrid"
                                  EnableViewState="true"
                                  ID="gridData"
                                  OnItemCommand="GridOnItemCommand"
                                  OnItemDataBound="OnGridDataBound"
                                  OnPageIndexChanged="OnPageIndexChanging"
                                  OnPageSizeChanged="OnPageSizeChanging"
                                  runat="server"
                                  Visible="False">
                        <MasterTableView>
                            <Columns>
                                <dnn:DnnGridImageCommandColumn CommandName="Edit"
                                                               IconKey="Edit"
                                                               UniqueName="EditButton" />
                                <dnn:DnnGridImageCommandColumn CommandName="Delete"
                                                               IconKey="Delete"
                                                               UniqueName="DeleteButton">
                                    <ItemStyle CssClass="btnDelete"></ItemStyle>
                                </dnn:DnnGridImageCommandColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
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

        <asp:HiddenField ID="hidCurrentPage"
                         runat="server"
                         Value="0"
                         Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function processOnLoad()
    {
        return validateOption(getControl("ddlDataTable"));
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

    addPageLoaded(function()
    {
        confirmMessage("td.btnDelete input", "Bạn có chắc muốn <b>XÓA</b> thông tin này?");
    }, true);
</script>