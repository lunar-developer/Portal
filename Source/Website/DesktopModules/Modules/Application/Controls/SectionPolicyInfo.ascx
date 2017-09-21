<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionPolicyInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionPolicyInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Credit Policy"
                Text="Điều kiện cấp tín dụng" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlPolicyCode"
                ClientIDMode="Static"
                OnSelectedIndexChanged="ProcessOnPolicyCodeChange"
                AutoPostBack="true"
                runat="server"/>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Num of Document"
                Text="Số tờ trình" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlNumOfDocument"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Membership ID"
                Text="Mã thành viên" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlMembershipID"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Guarantor Name"
                Text="Người bảo lãnh" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlGuarantorName"
                runat="server" />
        </div>
    </div>
</div>

<div class="col-sm-6" id="rightPanel" runat="server">
<div class="form-group" id="AgeBand" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Age Band"
            Text="Độ tuổi" />
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ID="ctrlAgeBand"
            ClientIDMode="Static"
            EmptyMessage="Độ tuổi"
            runat="server">
            <Items>
                <control:ComboBoxItem value="01" Text="18-25"/>
                <control:ComboBoxItem value="02" Text="26-31"/>
                <control:ComboBoxItem value="03" Text="32-36"/>
                <control:ComboBoxItem value="04" Text="37-41"/>
                <control:ComboBoxItem value="05" Text="42-51"/>
                <control:ComboBoxItem value="06" Text="52-65"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="HouseFee" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="House Fee"
            Text="Chi phí thuê nhà" />
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlHouseFee"
            runat="server"/>
    </div>
</div>
<div class="form-group" id="HasUndetermineIncoming" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Undetermine Incoming"
            Text="Có nguồn thu nhập ngoài không xác định"/>
    </div>
    <div class="col-sm-8">
        <asp:CheckBox
            ID="ctrlHasUndetermineIncoming"
            runat="server" />
    </div>
</div>
<div class="form-group" id="AuthorizedCapital" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Authorized Capital"
            Text="Vốn điều lệ"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlAuthorizedCapital"
            runat="server" />
    </div>
</div>
<div class="form-group" id="EstablishmentTime" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Establishment Time"
            Text="Thời gian thành lập"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlEstablishmentTime"
            runat="server" >
            <Items>
                <control:ComboBoxItem value="01" text="≤ 3 năm"/>
                <control:ComboBoxItem value="02" text="3-5 năm"/>
                <control:ComboBoxItem value="03" text="5-10 năm"/>
                <control:ComboBoxItem value="04" text="> 10 năm"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="VATPerMonth" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="VAT/Month"
            Text="Số tiền VAT/Tháng"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlVATPerMonth"
            runat="server" />
    </div>
</div>
<div class="form-group" id="SalesPerMonth" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Sales/Month"
            Text="Doanh số/tháng"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlSalesPerMonth"
            runat="server" />
    </div>
</div>
<div class="form-group" id="Yielding" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Yielding"
            Text="Tỷ suất sinh lợi"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlYielding"
            runat="server" />
    </div>
</div>
<div class="form-group" id="Rule" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Rule"
            Text="Điều lệ"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlRule"
            runat="server" />
    </div>
</div>
<div class="form-group" id="EquityRatio" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Equity Ratio"
            Text="Tỷ trọng góp vốn"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlEquityRatio"
            runat="server" />
    </div>
</div>
<div class="form-group" id="SalesOfLastYear" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Sales of Last Year"
            Text="Doanh số năm gần nhất"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlSalesOfLastYear"
            runat="server" />
    </div>
</div>
<div class="form-group" id="SalesPerLastSixMonth" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Sales of Last Six Month"
            Text="Doanh số 6 tháng gần nhất"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlSalesOfLastSixMonth"
            runat="server" />
    </div>
</div>
<div class="form-group" id="IsJobDetermine" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Is Job Determine"
            Text="Công việc xác nhận được"/>
    </div>
    <div class="col-sm-8">
        <asp:CheckBox
            ID="ctrlIsJobDetermine"
            runat="server" />
    </div>
</div>
<div class="form-group" id="BrokerRole" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Broker Role"
            Text="Chức vụ Người giới thiệu"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlBrokerRole"
            runat="server" >
            <Items>
                <control:ComboBoxItem value="01" Text="HĐQT/TGĐ"/>
                <control:ComboBoxItem value="02" Text="BAN TGĐ/BĐH"/>
                <control:ComboBoxItem value="03" Text="GĐ KHỐI"/>
                <control:ComboBoxItem value="04" Text="BGĐ TRUNG TÂM/PHÒNG/CGPD CÂP A"/>
                <%-- ReSharper disable once Asp.Entity --%>
                <control:ComboBoxItem value="05" Text="CGPD CÂP B&C "/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="BankLoyalty" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Bank Loyalty"
            Text="Thời gian giao dịch với VB"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlBankLoyalty"
            runat="server" >
            <Items>
                <control:ComboBoxItem value="01" Text="≤ 1 năm"/>
                <control:ComboBoxItem value="02" Text="1-3 năm"/>
                <control:ComboBoxItem value="03" Text="3-5 năm"/>
                <control:ComboBoxItem value="04" Text="> 5 năm"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="DepositAmount" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Deposit Amount"
            Text="Số dư tiền gửi"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlDepositAmount"
            runat="server" />
    </div>
</div>
<div class="form-group" id="DepositAmountAverage" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Deposit Amount Average"
            Text="Số dư tiền gửi bình quân"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlDepositAmountAverage"
            runat="server" />
    </div>
</div>
<div class="form-group" id="LoanLimit" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Loan Limit"
            Text="Hạn mức được duyệt vay"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlLoanLimit"
            runat="server" />
    </div>
</div>
<div class="form-group" id="MortgageCollateral" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Mortgage Collateral"
            Text="Loại TSDB đang thế chấp VB"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlMortgageCollateral"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="Số dư TG"/>
                <control:ComboBoxItem Value="02" Text="BĐS"/>
                <control:ComboBoxItem Value="03" Text="Xe"/>
                <control:ComboBoxItem Value="04" Text="Khác"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="IsExistCustomer" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Is Existing Customer"
            Text="Có giao dịch với VB"/>
    </div>
    <div class="col-sm-8">
        <asp:CheckBox
            ID="ctrlIsExistCustomer"
            runat="server" />
    </div>
</div>
<div class="form-group" id="MaximumLoanAmount" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Maximum Loan Amount"
            Text="Mức duyệt vay cao nhất của 1 TCTD"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlMaximumLoanAmount"
            runat="server" />
    </div>
</div>
<div class="form-group" id="CompanyRole" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Company Role"
            Text="Vị trí KH trong công ty"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlCompanyRole"
            runat="server">
            <Items>
                <control:ComboBoxItem value="01" Text="Người đại diện pháp luật"/>
                <control:ComboBoxItem value="02" Text="Thành viên góp vốn"/>
                <control:ComboBoxItem value="03" Text="Người đại diện ký kết khoản vay"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="VisaAllocationCountry" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Visa Allocation Country"
            Text="Tên quốc gia cấp Visa"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlVisaAllocationCountry"
            runat="server"/>
    </div>
</div>
<div class="form-group" id="TotalVisaAllocationLastYear" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Number of Visa Allocation Last year"
            Text="SL Visa được cấp năm gần nhất"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlTotalVisaAllocationLastYear"
            runat="server" />
    </div>
</div>
<div class="form-group" id="TotalTimeAbroadLastYear" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Total Time Abroad Last Year"
            Text="Số lần đi nước ngoài năm gần nhất"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlTotalTimeAbroadLastYear"
            runat="server" />
    </div>
</div>
<div class="form-group" id="TourCost" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Tour Cost"
            Text="Giá trị tour làm cơ sở cấp thẻ"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlTourCost"
            runat="server" />
    </div>
</div>
<div class="form-group" id="CreditLimitLastThreeMonth" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText=""
            Text="Dư nợ/HM 3 tháng gần"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlCreditLimitLastThreeMonth"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="≤ 20%"/>
                <control:ComboBoxItem Value="02" Text="> 20-30%"/>
                <control:ComboBoxItem Value="03" Text="> 30-50%"/>
                <control:ComboBoxItem Value="04" Text="> 50-70%"/>
                <control:ComboBoxItem Value="05" Text="> 70-100%"/>
                <control:ComboBoxItem Value="06" Text="> 100%"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="HasExceedLimitInTwelveMonth" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Exceed limit in history"
            Text="Có vượt HM trong 12 tháng"/>
    </div>
    <div class="col-sm-8">
        <asp:CheckBox
            ID="ctrlHasExceedLimitInTwelveMonth"
            runat="server" />
    </div>
</div>
<div class="form-group" id="MemberLevel" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Member Level"
            Text="Hạng hội viên"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlMemberLevel"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="BẠC"/>
                <control:ComboBoxItem Value="02" Text="TITAN"/>
                <control:ComboBoxItem Value="03" Text="VÀNG"/>
                <control:ComboBoxItem Value="04" Text="BẠCH KIM"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="FirstCardIssueTime" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="First Card Issue Time"
            Text="Thời gian chủ thẻ được cấp"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlFirstCardIssueTime"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="≤ 1 năm"/>
                <control:ComboBoxItem Value="02" Text="> 1-2 năm"/>
                <control:ComboBoxItem Value="03" Text="> 2-5 năm"/>
                <control:ComboBoxItem Value="04" Text="> 5 năm"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="PaymentRatioAverageInSixMonth" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Payment Ratio Average in Six Month"
            Text="Tỷ lệ thanh toán bình quan 6 tháng"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlPaymentRatioAverageInSixMonth"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="≤ 30%"/>
                <control:ComboBoxItem Value="02" Text="> 30-50%"/>
                <control:ComboBoxItem Value="03" Text="> 50-80%"/>
                <control:ComboBoxItem Value="04" Text="> 80-100%"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="PaymentTransactionRatio" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Payment Transaction Ratio"
            Text="Tỷ lệ giao dịch thanh toán"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlPaymentTransactionRatio"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="< 50%"/>
                <control:ComboBoxItem Value="02" Text="≥ 50%"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="StudyingAbroadCountry" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Studying Abroad Country"
            Text="Quốc gia du học"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlStudyingAbroadCountry"
            runat="server"/>
    </div>
</div>
<div class="form-group" id="StudyingAbroadFee" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Studying Abroad Fee (Thousand VND)"
            Text="Học phí du học (triệu đồng)"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlStudyingAbroadFee"
            runat="server" />
    </div>
</div>
<div class="form-group" id="PartnerLevel" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Partner Level"
            Text="Hạng thành viên"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlPartnerLevel"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="THÀNH VIÊN"/>
                <control:ComboBoxItem Value="01" Text="VIP"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="CompanyInList" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Company in List"
            Text="Công ty thuộc Danh sách"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlCompanyInList"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="Niêm yêt HOSE"/>
                <control:ComboBoxItem Value="01" Text="DN nộp Thuế"/>
                <control:ComboBoxItem Value="01" Text="500 DN lớn - B1"/>
                <control:ComboBoxItem Value="01" Text="500 DN lớn - B2"/>
                <control:ComboBoxItem Value="01" Text="Ngân hàng"/>
                <control:ComboBoxItem Value="01" Text="Cty Nhà nước/ Nước ngoài"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="CustomerGroup" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Customer Group"
            Text="Đối tượng KH"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlCustomerGroup"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="HĐQT/BTGD/BKS"/>
                <control:ComboBoxItem Value="02" Text="Phòng Hội sở/ Khối/Trung tâm/Cty con"/>
                <control:ComboBoxItem Value="03" Text="BLĐ Chi nhánh"/>
                <control:ComboBoxItem Value="04" Text="CBNV"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="SchoolInDistrict" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="School in District"
            Text="Trường học thuộc quận"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlSchoolInDistrict"
            runat="server"/>
    </div>
</div>
<div class="form-group" id="PupilName" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Pupil Name"
            Text="Tên học sinh"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlPupilName"
            runat="server" />
    </div>
</div>
<div class="form-group" id="TuitionFeePerMonth" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Tuition Fee (Thousand VND)"
            Text="Học phí/Tháng (triệu đồng)"/>
    </div>
    <div class="col-sm-8">
        <asp:TextBox
            CssClass="form-control c-theme"
            ID="ctrlTuitionFeePerMonth"
            runat="server" />
    </div>
</div>
<div class="form-group" id="EducationStage" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Education Stage"
            Text="Cấp đào tạo"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlEducationStage"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="MẦM NON"/>
                <control:ComboBoxItem Value="02" Text="TIỂU HỌC"/>
                <control:ComboBoxItem Value="03" Text="THCS"/>
                <control:ComboBoxItem Value="04" Text="THPT"/>
                <control:ComboBoxItem Value="05" Text="CAO ĐẲNG"/>
                <control:ComboBoxItem Value="06" Text="ĐẠI HỌC"/>
                <control:ComboBoxItem Value="07" Text="KHÁC" />
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="SchoolRole" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="School Role"
            Text="Chức vụ"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlSchoolRole"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="Chủ sở hữu"/>
                <control:ComboBoxItem Value="02" Text="Ban Giám hiệu"/>
                <control:ComboBoxItem Value="03" Text="Ban Giám hiệu"/>
                <control:ComboBoxItem Value="04" Text="Khác"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="GovernmentServiceType" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Service Type"
            Text="Hình thức quản lý"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlGovernmentServiceType"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="CÔNG LẬP"/>
                <control:ComboBoxItem Value="02" Text="DÂN LẬP"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="GovernmentDirectManager" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Direct Manager"
            Text="Cấp quản lý trực tiếp(công lập)"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlGovernmentDirectManager"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="Bộ"/>
                <control:ComboBoxItem Value="02" Text="Sở"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="HospitalRole" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Hospital Role"
            Text="Chức vụ"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlHospitalRole"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="Chủ sở hữu"/>
                <control:ComboBoxItem Value="02" Text="Ban Giám đốc"/>
                <control:ComboBoxItem Value="03" Text="BLĐ Phòng/Khoa/TT"/>
                <control:ComboBoxItem Value="04" Text="Khác"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="JobSeniority" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Job Seniority"
            Text="Thâm niên"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlJobSeniority"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="≤ 1 năm"/>
                <control:ComboBoxItem Value="02" Text="≤ 3 năm"/>
                <control:ComboBoxItem Value="03" Text="> 3 năm"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="HasDrugstore" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Have Surgery or Drugstore"
            Text="Có phòng mạch/Nhà thuốc"/>
    </div>
    <div class="col-sm-8">
        <asp:CheckBox
            ID="ctrlHasDrugstore"
            runat="server" />
    </div>
</div>
<div class="form-group" id="GovernmentWorkingPlace" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Government Working Place"
            Text="Đơn vị công tác"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlGovernmentWorkingPlace"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="HCSN"/>
                <control:ComboBoxItem Value="02" Text="ĐẢNG"/>
                <control:ComboBoxItem Value="03" Text="TCCTXH"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="GovernmentOrganizationLevel" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Government Organization Level"
            Text="Cấp của cơ quan"/>
    </div>
    <div class="col-sm-8">
        <control:AutoComplete
            ClientIDMode="Static"
            ID="ctrlGovernmentOrganizationLevel"
            runat="server">
            <Items>
                <control:ComboBoxItem Value="01" Text="Dưới Quận/Huyện/TX"/>
                <control:ComboBoxItem Value="02" Text="Cấp Quận/Huyện/TX/TP thuộc tỉnh"/>
                <control:ComboBoxItem Value="03" Text="Cấp TP thuộc TW/ Tỉnh"/>
                <control:ComboBoxItem Value="03" Text="Cấp TW"/>
            </Items>
        </control:AutoComplete>
    </div>
</div>
<div class="form-group" id="IsInPayroll" Visible="false" runat="server">
    <div class="col-sm-4 control-label">
        <control:DoubleLabel
            runat="server"
            SubText="Is In Payroll"
            Text="Thuộc biên chế"/>
    </div>
    <div class="col-sm-8">
        <asp:CheckBox
            ID="ctrlIsInPayroll"
            runat="server" />
    </div>
</div>
</div>