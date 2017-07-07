<%@ Control Language="C#" AutoEventWireup="false" CodeFile="DisbursementEditor.ascx.cs" Inherits="DesktopModules.Modules.Disbursement.DisbursementEditor" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" TagName="Label" Src="~/controls/Label.ascx" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                    runat="server" />
            </div>
            <div class="dnnTabs"
                id="DivEditor"
                runat="server">

                <div class="form-group">
                    <ul class="dnnAdminTabNav dnnClear">
                        <li>
                            <a href="#DivInfo">Thông tin Giải Ngân</a>
                        </li>
                        <li id="TabHistory"
                            runat="server">
                            <a href="#DivHistory">Lịch sử thao tác</a>
                        </li>
                    </ul>
                </div>

                <div class="dnnClear"
                    id="DivInfo">
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label ID="Identifier"
                                IsRequire="True"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox CssClass="form-control c-theme"
                                ID="tbIdentifier"
                                MaxLength="20"
                                placeholder="Số CMND/Mã số thuế"
                                runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <control:Label ID="BranchID"
                                runat="server" />
                        </div>
                        <div class="col-sm-4 control-value">
                            <asp:Label ID="lblBranchID"
                                runat="server" />
                            <asp:HiddenField ID="hidBranchID"
                                runat="server"
                                Visible="False" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label ID="CustomerName"
                                IsRequire="True"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox CssClass="form-control c-theme"
                                ID="tbCustomerName"
                                MaxLength="100"
                                placeholder="Tên Khách hàng/Công ty"
                                runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <control:Label HelpText="Trạng thái hiện tại của yêu cầu"
                                runat="server"
                                Text="Trạng thái" />
                        </div>
                        <div class="col-sm-4 control-value">
                            <asp:Label CssClass="c-font-bold"
                                ID="lblStatus"
                                runat="server" />
                            <asp:HiddenField ID="hidStatus"
                                runat="server"
                                Visible="False" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label ID="CurrencyCode"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList AutoPostBack="False"
                                CssClass="form-control c-theme"
                                ID="ddlCurrencyCode"
                                runat="server" />
                        </div>

                        <div class="col-sm-2 control-label">
                            <control:Label ID="DisbursementDate"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                EnableTyping="False"
                                ID="tbDisbursementDate"
                                runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label ID="Amount"
                                IsRequire="True"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox autocomplete="off"
                                CssClass="form-control c-theme"
                                ID="tbAmount"
                                MaxLength="20"
                                placeholder="Số tiền"
                                runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <control:Label ID="DisbursementMethod"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList AutoPostBack="False"
                                CssClass="form-control c-theme"
                                ID="ddlDisbursementMethod"
                                runat="server">
                                <asp:ListItem Text="Chuyển khoản"
                                    Value="CK" />
                                <asp:ListItem Text="Tiền mặt"
                                    Value="TM" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label ID="LoanMethod"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox CssClass="form-control c-theme"
                                ID="tbLoanMethod"
                                MaxLength="20"
                                placeholder="Phương thức vay"
                                runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <control:Label HelpText="Lãi suất (ví dụ: 0.08)"
                                runat="server"
                                Text="Lãi suất" />
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox autocomplete="off"
                                CssClass="form-control c-theme"
                                placeHolder="Lãi suất"
                                ID="tbInterestRate"
                                runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label HelpText="Loại Khách hàng"
                                runat="server"
                                Text="Loại KH" />
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList AutoPostBack="False"
                                CssClass="form-control c-theme"
                                ID="ddlCustomerType"
                                runat="server" >
                                <asp:ListItem Text="KH Hiện hữu" Value="E" />
                                <asp:ListItem Text="KH Mới" Value="N" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2 control-label">
                            <control:Label HelpText="Thời hạn vay"
                                    runat="server"
                                    Text="Thời hạn vay" />
                        </div>
                        <div class="col-sm-4">
                            <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                EnableTyping="False"
                                ID="tbLoanExpire"
                                runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label HelpText="Ghi chú"
                                runat="server"
                                Text="Ghi chú" />
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList AutoPostBack="False"
                                CssClass="form-control c-theme"
                                ID="ddlNote"
                                runat="server" >
                                <asp:ListItem Text="KH giải ngân và thu nợ trong ngày" Value="0" />
                                <asp:ListItem Text="KH giải ngân nội bộ" Value="1" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <control:Label ID="DisbursementPurpose"
                                IsRequire="True"
                                runat="server" />
                        </div>
                        <div class="col-sm-10">
                            <asp:TextBox CssClass="form-control c-theme"
                                ID="tbDisbursementPurpose"
                                TextMode="MultiLine"
                                minLength="0"
                                placeholder="Mục đích vay vốn"
                                runat="server" />
                        </div>
                    </div>
                </div>

                <div class="dnnClear"
                    clientidmode="Static"
                    id="DivHistory"
                    runat="server">
                    <div class="form-group"
                        id="DivLog"
                        runat="server">
                    </div>
                </div>

            </div>

            <div class="form-group"
                id="DivRemark"
                runat="server">
                <div class="col-sm-2 control-label">
                    <control:Label HelpText="Ghi chú lại thông tin xử lý"
                        runat="server"
                        Text="Ghi chú" />
                </div>
                <div class="col-sm-10">
                    <asp:TextBox CssClass="form-control c-theme"
                        ID="tbRemark"
                        TextMode="MultiLine"
                        placeholder="Ghi chú xử lý"
                        runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-10">
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnSubmit"
                                OnClick="Submit"
                                runat="server"/>
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnRevise"
                                OnClick="Submit"
                                runat="server"/>
                    <%--<asp:Button CssClass="btn btn-primary"
                                ID="btnPreapprove"
                                OnClick="Submit"
                                runat="server"/>--%>
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnApprove"
                                OnClick="Submit"
                                runat="server"/>
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnReject"
                                OnClick="Submit"
                                runat="server"/>
                    
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnRequestCancel"
                                OnClick="Cancel"
                                runat="server"/>
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnRequestApprove"
                                OnClick="Cancel"
                                runat="server"/>
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnCancel"
                                OnClick="Cancel"
                                runat="server"/>

                    <asp:Button CssClass="btn btn-primary"
                        ID="btnCreate"
                        OnClick="Create"
                        OnClientClick="return validate();"
                        runat="server"
                        Text="Lưu" />
                    <asp:Button CssClass="btn btn-primary"
                        ID="btnUpdate"
                        OnClick="Update"
                        runat="server"
                        Text="Cập Nhật" />
                    <asp:Button CssClass="btn btn-default"
                        ID="btnDelete"
                        OnClick="Delete"
                        runat="server"
                        Text="Xóa" />
                </div>
            </div>

            <asp:HiddenField ID="hidDisbursementID"
                runat="server"
                Visible="False" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function validate() {
        var array = ["tbIdentifier", "tbCustomerName", "tbAmount", "tbDisbursementPurpose"];
        return validateInputArray(array);
    }

    addPageLoaded(function () {
        // Render tabs
        $(".dnnTabs").dnnTabs({
            selected: 0
        });

        // Attach event handler
        var formatHandler = function () {
            formatCurrency(this);
        }
        getJQueryControl("tbAmount").on("keyup blur", formatHandler);
        getJQueryControl("tbCollectAmount").on("keyup blur", formatHandler);
    }, true);
</script>
