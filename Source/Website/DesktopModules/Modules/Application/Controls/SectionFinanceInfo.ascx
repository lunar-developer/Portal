<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionFinanceInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionFinanceInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Staff"
                Text="NV VietBank" />
        </div>
        <div class="col-sm-2">
            <control:AutoComplete
                ID="ctrlStaffIndicator"
                runat="server"
                OnClientSelectedIndexChanged="processOnStaffIndicatorChange"
                ClientIDMode="Static">
                <Items>
                    <control:ComboBoxItem Value="Y" Text="Yes" />
                    <control:ComboBoxItem Value="N" Text="No" />
                </Items>
            </control:AutoComplete>
        </div>
        <div class="col-sm-3 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Staff ID"
                Text="MSNV" />
        </div>
        <div class="col-sm-3">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlStaffID"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Bunisess Tax No"
                Text="Mã số thuế" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlCompanyTaxNo"
                runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:Button
                CssClass="btn btn-primary c-margin-0"
                TabIndex="-1"
                ID="btnSearchCompanyTaxNo"
                OnClientClick="return alertOnConstruct()"
                runat="server"
                Text="Tìm" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Company Name"
                Text="Tên doanh nghiệp" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlCompanyName"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Address 1 (Company)"
                Text="Địa chỉ dòng thứ 1" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlCompanyAddress01"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Address 2 (Company)"
                Text="Địa chỉ dòng thứ 2" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlCompanyAddress02"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Country (Company)"
                Text="Quốc Gia" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlCompanyCountry"
                ClientIDMode="Static"
                control-id="ctrlCompanyState"
                OnSelectedIndexChanged="ProcessOnSelectCountry"
                OnClientSelectedIndexChanged="processOnSelectCountry"
                EmptyMessage="Country (Company)"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="State (Company)"
                Text="Tỉnh/Thành Phố" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                AutoPostBack="True"
                ID="ctrlCompanyState"
                ClientIDMode="Static"
                OnSelectedIndexChanged="ProcessOnSelectState"
                EmptyMessage="State (Company)"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="City (Company)"
                Text="Quận/Huyện" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlCompanyCity"
                ClientIDMode="Static"
                EmptyMessage="City (Company)"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Address 3 (Company)"
                Text="Phường/Xã" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlCompanyAddress03"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Phone 1 (Company)"
                Text="Điện thoại" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlCompanyPhone01"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Department Name (Main)"
                Text="Phòng ban" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlDepartmentName"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Working Period (Main)"
                Text="Thời gian công tác" />
        </div>
        <div class="col-sm-2">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlWorkingYear"
                runat="server" />
        </div>
        <div class="col-sm-2 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Year"
                Text="Năm" />
        </div>
        <div class="col-sm-2">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlWorkingMonth"
                runat="server" />
        </div>
        <div class="col-sm-2 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Month"
                Text="Tháng" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Position (Main)"
                Text="Chức vụ" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlPosition"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Customer title (Main)"
                Text="Chức danh" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlTitle"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Remarks (Company)"
                Text="Ghi chú" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlCompanyRemark"
                TextMode="MultiLine"
                Height="70"
                runat="server" />
        </div>
    </div>
</div>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Contract Type (Main)"
                Text="Tình trạng làm việc" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlContractType"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Customer Category (Main)"
                Text="Nghề nghiệp" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlJobCategory"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Business Type (Main)"
                Text="Loại hình doanh nghiệp" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlBusinessType"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Number of Staff"
                Text="Số lượng nhân viên" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlTotalStaff"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Business Size"
                Text="Quy mô doanh nghiệp" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlBusinessSize"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="SIC"
                Text="Lĩnh vực hoạt động" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlSIC"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Net Income (Main)"
                Text="Thu nhập" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlNetIncome"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Total Expense (Main)"
                Text="Tổng chi phí" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlTotalExpense"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Num Of Dependents"
                Text="Số người chu cấp" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlNumOfDependent"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Have credit card"
                Text="Đã có thẻ TD Bank khác" />
        </div>
        <div class="col-sm-2">
            <control:AutoComplete
                ID="ctrlHasOtherBankCreditCard"
                runat="server"
                ClientIDMode="Static">
                <Items>
                    <control:ComboBoxItem Value="Y" Text="Yes" />
                    <control:ComboBoxItem Value="N" Text="No" />
                </Items>
            </control:AutoComplete>
        </div>
        <div class="col-sm-3 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="No of bank"
                Text="Số lượng" />
        </div>
        <div class="col-sm-3">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlTotalBankHasCreditCard"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Home Ownership"
                Text="Hình thức sở hữu nhà" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlHomeOwnership"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Education (Main)"
                Text="Trình độ học vấn" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlEducation"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Secret Phrase"
                Text="Thông tin xác thực KH" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlSecretPhrase"
                runat="server" />
        </div>
    </div>
</div>
