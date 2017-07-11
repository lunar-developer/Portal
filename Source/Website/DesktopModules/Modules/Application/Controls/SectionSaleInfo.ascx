<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionSaleInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionSaleInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Card App Source Code"
                Text="Nguồn Khách hàng" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlApplicationSourceCode"
                ClientIDMode="Static"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="1" Text="1 - Mới" />
                    <control:ComboBoxItem Value="2" Text="2 - Kênh hỗ trợ" />
                    <control:ComboBoxItem Value="3" Text="3 - Hiện hữu" />
                </Items>
            </control:AutoComplete>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Sale Method"
                Text="Phương thức bán hàng" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlSaleMethod"
                ClientIDMode="Static"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="1" Text="1 - Bán hàng trực tiếp" />
                    <control:ComboBoxItem Value="2" Text="2 - Phối hợp CTV/MG" />
                    <control:ComboBoxItem Value="3" Text="3 - Phối hợp telesales" />
                </Items>
            </control:AutoComplete>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Sale Program"
                Text="Mã chương trình" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlProgramCode"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Processing Branch"
                Text="Đơn vị tiếp nhận" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlProcessingBranch"
                ClientIDMode="Static"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="1" Text="1 - Từ TTKD VietBank" />
                    <control:ComboBoxItem Value="2" Text="2 - Trung tâm thẻ" />
                </Items>
            </control:AutoComplete>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Branch Code"
                Text="Mã TTKD" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlSourceBranchCode"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Checker"
                Text="Cấp kiểm soát" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlChecker"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Sales Officer"
                Text="Nhân viên kinh doanh" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlSaleOfficer"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Sales Reference"
                Text="MSNV" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                Enabled="False"
                CssClass="form-control c-theme"
                ID="ctrlSaleStaffID"
                runat="server" />
        </div>
    </div>
</div>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-12 control-label" style="height: 35px"></div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 control-label" style="height: 35px"></div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Sales Support Name"
                Text="Tên Người phối hợp" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlSaleSupporter"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Sales ID"
                Text="Số CMND" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlSaleID"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Sale Account"
                Text="Số tài khoản" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlSaleAccount"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Mobile"
                Text="Điện thoại di động" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlSaleMobile"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Email"
                Text="Thư điện tử" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlSaleEmail"
                runat="server" />
        </div>
    </div>
</div>
