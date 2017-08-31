<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserAssignment.ascx.cs" Inherits="DesktopModules.Modules.Application.UserAssignment" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.Application.Database" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-4 control-label">
                    <label>Giai Đoạn</label>
                </div>
                <div class="col-md-4">
                    <control:AutoComplete
                        ID="ddlPhase"
                        OnSelectedIndexChanged="ProcessOnPhaseChange"
                        AutoPostBack="True"
                        runat="server" />
                </div>
                <div class="col-md-4"></div>
            </div>

            <div class="form-group">
                <div class="col-sm-4"></div>
                <div class="col-sm-8">
                    <asp:Button
                        CssClass="btn btn-primary"
                        ID="btnRefresh"
                        OnClick="Refresh"
                        runat="server"
                        Visible="False"
                        Text="Refresh" />
                    <asp:Button
                        CssClass="btn btn-default"
                        ID="btnAdd"
                        OnClick="AddUser"
                        runat="server"
                        Visible="False"
                        Text="Thêm mới" />
                </div>
            </div>

            <div class="form-group">
                <div class="col-sm-12">
                    <control:Grid
                        ID="gridData"
                        AutoGenerateColumns="False"
                        runat="server"
                        Visible="false"
                        OnNeedDataSource="OnNeedDataSource">
                        <MasterTableView>
                            <Columns>
                                <telerik:GridTemplateColumn DataField="UserID" HeaderText="Họ và Tên">
                                    <HeaderStyle Width="25%"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server"
                                            OnClick="EditUser"
                                            CssClass="c-edit-link c-theme-color"
                                            CommandArgument='<%#Eval(UserPhaseTable.UserID).ToString()%>'>
                                            <%#FunctionBase.FormatUserID(Eval(UserPhaseTable.UserID).ToString()) %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="PolicyCode" HeaderText="Chính sách">
                                    <HeaderStyle Width="45%"></HeaderStyle>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="KPI" HeaderText="KPI">
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="IsDisable" HeaderText="Đang sử dụng">
                                    <HeaderStyle Width="20%"></HeaderStyle>
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatState(Eval(UserPhaseTable.IsDisable).ToString(), false) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>


<script type="text/javascript">
    function refresh()
    {
        getControl("btnRefresh").click();
    }
</script>