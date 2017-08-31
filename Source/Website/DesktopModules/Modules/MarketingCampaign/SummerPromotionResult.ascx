<%@ Control Language="C#" AutoEventWireup="false" CodeFile="SummerPromotionResult.ascx.cs" Inherits="DesktopModules.Modules.MarketingCampaign.SummerPromotionResult" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>


<asp:UpdatePanel ID = "updatePanel"
                 runat = "server">
    <ContentTemplate>
        <%--<div class = "c-content-title-1 clearfix c-margin-b-20 c-title-md">
            <h3 class = "c-font-bold c-font-uppercase">Danh sách TOP 10</h3>
            <div class = "c-bg-blue c-line-left"></div>
        </div>--%>
        <div class = "form-horizontal" style="margin-top: -70px">
            <div class = "form-group">
                <asp:PlaceHolder ID = "phMessage"
                                 runat = "server" />
            </div>
            <div class = "form-group">
                <div class = "col-sm-3 control-label">
                    <dnn:Label ID = "lblBranchCode"
                               runat = "server" />
                </div>
                <div class = "col-sm-3">
                    <asp:TextBox CssClass = "form-control c-theme"
                                 ID = "txtBranchCode"
                                 runat = "server">
                    </asp:TextBox>
                </div>
                <div class = "col-sm-6">
                    <asp:Button CssClass = "btn btn-primary c-margin-t-0"
                                ID = "btnSearch"
                                OnClick = "Search"
                                OnClientClick = "return onBeforeSearch();"
                                runat = "server"
                                Text = "Tìm kiếm" />
                </div>
            </div>
            <div runat="server" ID="dsSession" Visible="False">
                <div class = "c-content-title-1 clearfix c-margin-b-20 c-title-md">
                    <h4 class = "c-font-bold c-font-uppercase" runat="server" ID="titleSession">Danh sách gần nhất</h4>
                </div>
                <div class = "form-group">
                    <div class = "col-sm-12">
                        <div id = "DivListBySession"
                             runat = "server" />
                    </div>
                </div>
            </div>
            <div runat="server" ID="dsWeek" Visible="False">
                <div class = "c-content-title-1 clearfix c-margin-b-20 c-title-md">
                    <h4 class = "c-font-bold c-font-uppercase" runat="server" ID="titleWeek">Danh sách tuần gần nhất</h4>
                </div>
                <div class = "form-group">
                    <div class = "col-sm-12">
                        <div id = "DivListByWeek"
                             runat = "server" />
                    </div>
                </div>
            </div>
            <div runat="server" ID="dsMonth" Visible="False">
                <div class = "c-content-title-1 clearfix c-margin-b-20 c-title-md">
                    <h4 class = "c-font-bold c-font-uppercase" runat="server" ID="titleMonth">Danh sách tháng gần nhất</h4>
                </div>
                <div class = "form-group">
                    <div class = "col-sm-12">
                        <div id = "DivListByMonth"
                             runat = "server" />
                    </div>
                </div>
            </div>
            <div runat="server" ID="dsYear" Visible="False">
                <div class = "c-content-title-1 clearfix c-margin-b-20 c-title-md">
                    <h4 class = "c-font-bold c-font-uppercase" runat="server" ID="titleYear">Danh sách theo năm</h4>
                </div>
                <div class = "form-group">
                    <div class = "col-sm-12">
                        <div id = "DivListByYear"
                             runat = "server" />
                    </div>
                </div>
            </div>
            
        </div>

    </ContentTemplate>
</asp:UpdatePanel>

<script type = "text/javascript">

    function onBeforeSearch() {
        return validateInput(getControl("txtBranchCode"));
    }
    function getWeek(target) {
        var dayNr = (target.getDay() + 6) % 7;
        var firstThursday = target.valueOf();

        target.setDate(target.getDate() - dayNr + 3);
        target.setMonth(0, 1);

        if (target.getDay() !== 4) {
            target.setMonth(0, 1 + ((4 - target.getDay()) + 7) % 7);
        }

        return 1 + Math.ceil((firstThursday - target) / 604800000); // 604800000 = 7 * 24 * 3600 * 1000
    }
    //$(document).ready(function ()
    //{
    //    var newDate = new Date();
    //    $('#weekOfYear').html(' Tuần ' + getWeek(newDate));
    //});
</script>