<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SummerPromotionUpload.ascx.cs" Inherits="DesktopModules.Modules.MarketingCampaign.SummerPromotionUpload" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>


<asp:UpdatePanel ID = "updatePanel"
                 runat = "server">
    <Triggers>
        <asp:PostBackTrigger ControlID = "btnUpload" />
    </Triggers>
    <ContentTemplate>
        <div class = "c-content-title-1 clearfix c-margin-b-20 c-title-md">
            <h3 class = "c-font-bold c-font-uppercase">Cập nhật kết quả CTĐH Mùa hè rực nắng</h3>
            <div class = "c-bg-blue c-line-left"></div>
        </div>
        
        <div class = "form-horizontal">
            <div class = "form-group">
                <asp:PlaceHolder ID = "phMessage"
                                 runat = "server" />
            </div>
            <div class="form-group">
            <div class="col-sm-12 dnnPanels">
                <h2 class="dnnFormSectionHead">
                    <a href="#">Hướng dẫn sử dụng</a>
                </h2>
                <fieldset>
                    <ul class="c-content-list-1 c-separator-dot c-theme c-margin-t-0">
                        <li class="text-justify">
                            Vui lòng sử dụng file theo <a class="c-theme-color c-edit-link" href="<%= LinkTemplateEvent02 %>">Template</a> để thực hiện cập nhật kết quả.
                        </li>
                        <li class="text-justify">
                            Dữ liệu trên file template phải là dữ liệu thuần giá trị (đã được xử lí, không được chứa công thức excel).
                        </li>
                        <li class="text-justify">
                            Chương trình sẽ kiểm tra ReportType, ReportYear, ReportNum. Nếu giống nhau sẽ lấy dữ liệu từ file import gần nhất.
                        </li>
                        <li class="text-justify">
                            <b>
                                <i>Các trường dữ liệu trên template đã được ghi chú dạng comment, sau đây là các lưu ý thêm:</i>
                            </b>
                        </li>
                        <li class="c-margin-l-20 text-justify c-bg-before-green">
                            ReportType: Loại báo cáo 'W': Tuần; 'M': Tháng; 'Y': Năm; K: Kỳ
                        </li>
                        <li class="c-margin-l-20 text-justify c-bg-before-green">
                            ReportYear: Dữ liệu import thuộc năm báo cáo. VD: 2017
                        </li>
                        <li class="c-margin-l-20 text-justify c-bg-before-green">
                            ReportNum: Mã số ID của báo cáo.
                        </li>
                        <li class="text-justify">
                            <b>Ví dụ:</b>
                        </li>
                        <li class="c-margin-l-20 text-justify c-bg-before-green">
                            Trường hợp ReportYear = 2017, ReportType = W (Tuần): Kết quả theo tuần. ReportNum = 10. Nghĩa là kết quả tuần thứ 10 của năm 2017.
                        </li>
                        <li class="c-margin-l-20 text-justify c-bg-before-green">
                            Trường hợp ReportYear = 2017, ReportType = M (Tháng): Kết quả theo tháng. ReportNum = 12. Nghĩa là kết quả tháng 12 của năm 2017.
                        </li>
                        <li class="c-margin-l-20 text-justify c-bg-before-green">
                            Trường hợp ReportYear = 2017, ReportType = K (Kỳ): Kết quả theo Kỳ. ReportNum = 8. Nghĩa là kết quả Kỳ 8 của năm 2017.
                        </li>
                    </ul>
                </fieldset>
            </div>
        </div>
            <div class = "form-group">
                <div class = "col-sm-2"></div>
                <div class = "col-sm-3 control-label">
                    <dnn:Label ID = "lblFileBrowser"
                               runat = "server" />
                </div>
                <div class = "col-sm-4">
                    <asp:FileUpload CssClass = "form-control c-theme"
                                    ID = "fupFile"
                                    runat = "server" />
                </div>
                <div class = "col-sm-3"></div>
            </div>
            <div class = "form-group">
                <div class = "col-sm-5"></div>
                <div class = "col-sm-7">
                    <asp:Button CssClass = "btn btn-primary"
                                ID = "btnUpload"
                                OnClick = "Upload"
                                OnClientClick = "return checkExtension();"
                                runat = "server"
                                Text = "Upload" />
                    <a class="btn btn-default"
                       href="<%= LinkTemplateEvent02 %>">
                        Download Template
                    </a>
                </div>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>

<script type = "text/javascript">
    addPageLoaded(function () {
        $(".dnnPanels").dnnPanels({
            defaultState: "close"
        });
    }, true);
    function checkExtension() {
        var value = getControl("fupFile").value;
        var part = value.split(".");
        var extension = part[part.length - 1].toLowerCase();
        if (extension === "csv" || extension === "xls" || extension === "xlsx") {
            return true;
        }
        else {
            alertMessage("Hệ thống chỉ hỗ trợ file CSV và Excel (xls, xlsx)", undefined, undefined, hideLoading);
            return false;
        }
    }
</script>