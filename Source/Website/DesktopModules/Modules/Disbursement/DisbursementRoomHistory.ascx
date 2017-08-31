<%@ Control Language="C#" AutoEventWireup="false" CodeFile="DisbursementRoomHistory.ascx.cs" Inherits="DesktopModules.Modules.Disbursement.DisbursementRoomHistory" %>
<%@ Import Namespace="Modules.Disbursement.Database" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.UserManagement.Global" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnn" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-group">
            <asp:PlaceHolder ID="phMessage"
                             runat="server" />
        </div>
        <div class="form-horizontal">
           <control:Grid AllowPaging="true"
                                 AutoGenerateColumns="false"
                                 CssClass="dnnGrid"
                                 EnableViewState="true"
                                 ID="gridData"
                                 OnPageIndexChanged="OnPageIndexChanging"
                                 OnPageSizeChanged="OnPageSizeChanging"
                                 PageSize="10"
                                 runat="server"
                                 Visible="false">
                        <MasterTableView>
                            <Columns>
                                <dnn:DnnGridBoundColumn  HeaderText="RateLDR" DataField="Room" >
                                    <HeaderStyle Width="25%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="Room">
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatDecimal(Eval(DisbursementRoomTable.RateLdr).ToString())%>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" />
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridBoundColumn  HeaderText="Rate" DataField="Rate" >
                                    <HeaderStyle Width="15%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn  HeaderText="CreatedBy" DataField="CreatedBy" >
                                    <HeaderStyle Width="25%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="CreatedAt">
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatDate(Eval(DisbursementRoomTable.CreatedAt).ToString()) %>
                                    </ItemTemplate>
                                    <HeaderStyle Width="20%" />
                                </dnn:DnnGridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
        </div>
        <asp:HiddenField runat="server" Visible="True" ID="HidenId"/>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
</script>