<%@ Control Language="C#" AutoEventWireup="false" CodeFile="DisbursementRoom.ascx.cs" Inherits="DesktopModules.Modules.Disbursement.DisbursementRoom" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnn" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-group">
            <asp:PlaceHolder ID="phMessage"
                             runat="server" />
        </div>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-3 control-label">
                    <dnn:Label ID="RateLDR"
                               runat="server" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox CssClass="form-control"
                                 ID="tbRateLDR"
                                 MaxLength="20"
                                 placeholder="Tỷ lệ LDR"
                                 runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-3 control-label">
                    <dnn:Label ID="Room"
                               runat="server" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox CssClass="form-control"
                                 ID="tbRoom"
                                 MaxLength="100"
                                 placeholder="Tên Room"
                                 runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-3 control-label">
                    <dnn:Label ID="Rate"
                               runat="server" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox CssClass="form-control"
                                 ID="tbRate"
                                 MaxLength="20"
                                 placeholder="Nhập tỷ giá"
                                 runat="server" />
                </div>
            </div>
            <div class="clearfix"></div>
            <div class="col-sm-3"></div>
            <div class="col-sm-9">
                <asp:Button CssClass="btn btn-primary"
                            ID="btnUpdate"
                            OnClick="Update"
                            runat="server"
                            Text="Cập nhật" />
                <asp:Button CssClass="btn btn-primary"
                            ID="btnHistory"
                            OnClick="History"
                            runat="server"
                            Text="500 thay đổi gần nhất" />
            </div>
        </div>
        <asp:HiddenField runat="server" Visible="True" ID="HidenId"/>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
</script>