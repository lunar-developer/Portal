<%@ Control Language="C#" AutoEventWireup="false" CodeFile="CacheInquiry.ascx.cs" Inherits="DesktopModules.Modules.CacheManagement.CacheInquiry" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-5">
                    <control:AutoComplete 
                        ID="ddlCacheType"
                        AutoPostBack="True"
                        onselectedindexchanged="ProcessOnSelectionChange"
                        EmptyMessage="Please select a cache type"
                        runat="server">
                    </control:AutoComplete>
                </div>
                <div class="col-sm-4">
                    <h4>
                        <div class="col-sm-4">
                            Total Caches: <asp:Label ID="lblTotalCaches" runat="server" Text="0" />
                        </div>
                        <div class="col-sm-8">
                            Total Items of Selected Cache: <asp:Label ID="lblTotalItems" runat="server" Text="0" />
                        </div>
                    </h4>
                </div>
                <div class="col-sm-3">
                    <asp:Button CssClass="btn btn-success c-margin-t-0"
                                ID="btnRefresh"
                                OnClick="Refresh"
                                runat="server"
                                Visible="False"
                                Text="Refresh" />
                    <asp:Button CssClass="btn btn-warning c-margin-t-0"
                                ID="btnReload"
                                OnClick="Reload"
                                runat="server"
                                Visible="False"
                                Text="Reload" />
                </div>
            </div>

            <div class="form-group"
                 id="DivControl"
                Visible="False"
                 runat="server">
                <div class="col-sm-5">
                    <control:AutoComplete
                        ID="ddlField"
                        runat="server"
                        EmptyMessage="Please select a field"/>
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control"
                                 ID="tbKeyword"
                                 placeholder="Keyword"
                                 runat="server" />
                </div>
                <div class="col-sm-3">
                    <asp:Button CssClass="btn btn-primary c-margin-t-0"
                                ID="btnSearch"
                                OnClick="Filter"
                                runat="server"
                                Text="Search" />
                    <asp:Button CssClass="btn btn-default c-margin-t-0"
                                ID="btnClear"
                                OnClick="ClearFilter"
                                runat="server"
                                Text="Clear" />
                </div>
            </div>

            <div class="form-group">
                <div class="col-sm-12 table-responsive">
                    <control:Grid 
                        ID="gridView"
                        OnPageIndexChanged="OnPageIndexChanging"
                        OnPageSizeChanged="OnPageSizeChanging"
                        AllowSorting="False"
                        runat="server"
                        Visible="True" />
                </div>
            </div>
        </div>
        
        <asp:HiddenField runat="server" ID="hidFieldName"/>
        <asp:HiddenField runat="server" ID="hidFieldValue"/>
    </ContentTemplate>
</asp:UpdatePanel>