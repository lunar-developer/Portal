<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Schedule.ascx.cs" Inherits="DesktopModules.Modules.Application.Schedule" %>
<%@ Import Namespace="Modules.Application.Database" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="Telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <label>Tác vụ</label>
                </div>
                <div class="col-sm-4">
                    <control:AutoComplete
                        AutoPostBack="True"
                        ID="ddlSchedule"
                        OnSelectedIndexChanged="OnSelectedIndexChanged"
                        runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:Button runat="server" ID="btnRefresh" CssClass="btn btn-primary c-margin-t-0" Text="Refresh" OnClick="Refresh" />
                    <asp:Button runat="server" ID="btnClear" CssClass="btn btn-default c-margin-t-0" Text="Clear Log" OnClick="ClearLog" />
                </div>
            </div>
            <div runat="server" id="DivInfo" visible="False">
                <div class="form-group">
                    <div class="col-sm-1"></div>
                    <div runat="server" id="DivSheduleInfo" class="col-sm-10"></div>
                    <div class="col-sm-1"></div>
                </div>
                <div class="form-group">
                    <div class="col-sm-12">
                        <control:Grid ID="GridData" runat="server" AutoGenerateColumns="False" AllowFilteringByColumn="False"
                            OnNeedDataSource="OnNeedDataSource"
                            Width="100%">
                            <MasterTableView>
                                <Columns>
                                    <Telerik:GridTemplateColumn DataField='<%=ScheduleLogTable.CreateDateTime%>'
                                        HeaderText="Ngày xử lý">
                                        <HeaderStyle Width="25%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        <ItemTemplate>
                                            <%#FunctionBase.FormatDate(Eval("CreateDateTime").ToString())%>
                                        </ItemTemplate>
                                    </Telerik:GridTemplateColumn>
                                    <Telerik:GridTemplateColumn DataField='<%=ScheduleLogTable.IsSuccess%>'
                                                                HeaderText="Kết Quả">
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        <ItemTemplate>
                                            <%#FormatState(Eval(ScheduleLogTable.IsSuccess).ToString())%>
                                        </ItemTemplate>
                                    </Telerik:GridTemplateColumn>
                                    <Telerik:GridBoundColumn DataField="LogMessage"
                                        HeaderText="Nội Dung">
                                        <HeaderStyle Width="65%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                    </Telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </control:Grid>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<style type="text/css">
    table.ScheduleLogTable {
        border: none !important;
        margin-bottom: 5px !important;
        width: 100% !important;
    }

        table.ScheduleLogTable tbody tr th {
            color: #69727c;
            border-top: 0.5px dashed #F5F6F7 !important;
            border-bottom: 0.5px dashed #F5F6F7 !important;
            border-left: none !important;
            border-right: none !important;
            padding-left: 5px;
            padding-right: 5px;
            background-color: #F5F6F7;
        }
</style>
