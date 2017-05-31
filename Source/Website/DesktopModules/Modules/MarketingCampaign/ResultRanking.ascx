<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ResultRanking.ascx.cs" Inherits="DesktopModules.Modules.MarketingCampaign.ResultRanking" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>


<asp:UpdatePanel ID = "updatePanel"
                 runat = "server">
    <ContentTemplate>
        <div class = "c-content-title-1 clearfix c-margin-b-20 c-title-md">
            <h3 class = "c-font-bold c-font-uppercase">Danh sách Top 5 kết quả cao nhất</h3>
            <div class = "c-bg-blue c-line-left"></div>
        </div>
        <div class = "form-horizontal">
            <div class = "form-group">
                <asp:PlaceHolder ID = "phMessage"
                                 runat = "server" />
            </div>
            <div class = "form-group">
                <div class = "col-sm-2 control-label">
                    <dnn:Label ID = "lblStaffID"
                               runat = "server" />
                </div>
                <div class = "col-sm-3">
                    <asp:TextBox CssClass = "form-control c-theme"
                                 ID = "txtStaffID"
                                 runat = "server">
                    </asp:TextBox>
                </div>
                <div class = "col-sm-7">
                    <asp:Button CssClass = "btn btn-primary c-margin-t-0"
                                ID = "btnSearch"
                                OnClick = "Search"
                                OnClientClick = "return onBeforeSearch();"
                                runat = "server"
                                Text = "Tìm kiếm" />
                </div>
            </div>
            <div runat="server" ID="dsWeek" Visible="False">
                <div class = "c-content-title-1 clearfix c-margin-b-20 c-title-md">
                    <h4 class = "c-font-bold c-font-uppercase" runat="server" ID="titleWeek">Danh sách Top 5</h4>
                </div>
                <div class = "dnnTabs">
                    <div class = "form-group">
                        <ul class = "dnnAdminTabNav dnnClear">
                            <li>
                                <a href = "#TabStaffWeek">Nhân Viên</a>
                            </li>
                            <li>
                                <a href = "#TabManagerWeek">Lãnh Đạo</a>
                            </li>
                        </ul>
                    </div>
                    <div class = "dnnClear"
                         id = "TabStaffWeek">
                        <div class = "form-group">
                            <div class = "col-sm-12">
                                <div id = "DivTopStaffByWeek"
                                     runat = "server" />
                            </div>
                        </div>
                    </div>
                    <div class = "dnnClear"
                         id = "TabManagerWeek">
                        <div class = "form-group">
                            <div class = "col-sm-12">
                                <div id = "DivTopManagerByWeek"
                                     runat = "server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div runat="server" ID="dsMonth" Visible="False">
                <div class = "c-content-title-1 clearfix c-margin-b-20 c-title-md">
                    <h4 class = "c-font-bold c-font-uppercase" runat="server" ID="titleMonth">Danh sách Top 5</h4>
                </div>
                <div class = "dnnTabs">
                    <div class = "form-group">
                        <ul class = "dnnAdminTabNav dnnClear">
                            <li>
                                <a href = "#TabStaffMonth">Nhân Viên</a>
                            </li>
                            <li>
                                <a href = "#TabManagerMonth">Lãnh Đạo</a>
                            </li>
                        </ul>
                    </div>
                    <div class = "dnnClear"
                         id = "TabStaffMonth">
                        <div class = "form-group">
                            <div class = "col-sm-12">
                                <div id = "DivTopStaffByMonth"
                                     runat = "server" />
                            </div>
                        </div>
                    </div>
                    <div class = "dnnClear"
                         id = "TabManagerMonth">
                        <div class = "form-group">
                            <div class = "col-sm-12">
                                <div id = "DivTopManagerByMonth"
                                     runat = "server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div runat="server" ID="dsYear" Visible="False">
                <div class = "c-content-title-1 clearfix c-margin-b-20 c-title-md">
                    <h4 class = "c-font-bold c-font-uppercase" runat="server" ID="titleYear">Danh sách Top</h4>
                </div>
                <div class = "dnnTabs">
                    <div class = "form-group">
                        <ul class = "dnnAdminTabNav dnnClear">
                            <li>
                                <a href = "#TabStaffYear">Nhân Viên</a>
                            </li>
                            <li>
                                <a href = "#TabManagerYear">Lãnh Đạo</a>
                            </li>
                        </ul>
                    </div>
                    <div class = "dnnClear"
                         id = "TabStaffYear">
                        <div class = "form-group">
                            <div class = "col-sm-12">
                                <div id = "DivTopStaffByYear"
                                     runat = "server" />
                            </div>
                        </div>
                    </div>
                    <div class = "dnnClear"
                         id = "TabManagerYear">
                        <div class = "form-group">
                            <div class = "col-sm-12">
                                <div id = "DivTopManagerByYear"
                                     runat = "server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>

    </ContentTemplate>
</asp:UpdatePanel>

<script type = "text/javascript">
    addPageLoaded(function() {
            $(".dnnTabs").dnnTabs({
                selected: 0
            });
        },
        true);

    function onBeforeSearch() {
        return validateInput(getControl("txtStaffID"));
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