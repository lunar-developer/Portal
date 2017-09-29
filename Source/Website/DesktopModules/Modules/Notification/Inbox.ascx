<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Inbox.ascx.cs" Inherits="DesktopModules.Modules.Notification.Inbox" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder
                    ID="phMessage"
                    runat="server" />
            </div>
            <div class="form-group">
                <div class="col-sm-12">
                    <asp:Button
                        CssClass="btn btn-primary"
                        OnClick="Refresh"
                        ID="btnRefresh"
                        runat="server"
                        Text="Làm Mới" />
                    <asp:Button
                        CssClass="btn btn-danger"
                        OnClick="DeleteNotification"
                        ID="btnDelete"
                        runat="server"
                        Text="Xóa Tất Cả" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-12">
                    <control:Grid
                        AutoGenerateColumns="False"
                        ID="gridData"
                        Width="100%"
                        OnNeedDataSource="ProcessOnGridNeedDataSource"
                        runat="server">
                        <MasterTableView TableLayout="Auto">
                            <Columns>
                                <telerik:GridBoundColumn DataField="DisplayDate" HeaderText="Thời Gian">
                                    <HeaderStyle Width="15%"></HeaderStyle>
                                </telerik:GridBoundColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridBoundColumn DataField="From" HeaderText="Người Gửi">
                                    <HeaderStyle Width="15%"></HeaderStyle>
                                </telerik:GridBoundColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridBoundColumn DataField="Subject" HeaderText="Tiêu Đề">
                                    <HeaderStyle Width="20%"></HeaderStyle>
                                </telerik:GridBoundColumn>
                            </Columns>
                            <Columns>
                                <telerik:GridBoundColumn DataField="Body" HeaderText="Nội Dung">
                                    <HeaderStyle Width="50%"></HeaderStyle>
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function ()
    {
        registerConfirm({
            jquery: getControl("btnDelete"),
            message: "Bạn có chắc muốn <b>XÓA TOÀN BỘ</b> tin nhắn trong hộp thư?"
        });
    }, true);
</script>
