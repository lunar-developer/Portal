<%@ Control Language="C#" AutoEventWireup="false" CodeFile="EmployeeUpdateInfo.ascx.cs" Inherits="DesktopModules.Modules.EmployeeManagement.EmployeeUpdateInfo" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-group">
            <asp:PlaceHolder ID="phMessage"
                             runat="server" />
        </div>
        <div class="form-horizontal" runat="server" id="DivForm">
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblEmployeeID"
                               runat="server"
                               Text="Mã nhân viên"/>
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                               ID="txtEmployeeID"
                               Enabled = "False"
                               runat="server" />
                </div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblAddress"
                              runat="server"
                              Text="Địa chỉ"/>
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                              ID="txtOffice"
                              runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblFullName"
                              runat="server" 
                              Text="Họ Tên"/>
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                              ID="txtFullName"
                              Enabled = "False"
                              runat="server" />
                </div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblEmail"
                              runat="server"
                              Text="Email"/>
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                              ID="txtEmail"
                              Enabled = "False"
                              runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblDateOfBirth"
                              runat="server" 
                              Text="Ngày sinh"/>
                </div>
                <div class="col-sm-4">
                     <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                              EnableTyping="False"
                              ID="calendarDateOfBirth" 
                              runat="server"
                              Width="160px" />
                </div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblPhoneNumber"
                              runat="server"
                              Text="Số điện thoại"/>
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                              ID="txtPhoneNumber"
                              runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblGender"
                              runat="server"
                              Text ="Giới tính"/>


                </div>
                <div class="col-sm-4">
                    <control:AutoComplete 
                              ID="cbxGender" 
                              ClientIDMode="Static"
                              runat="server">
                              <Items>
                                  <control:ComboBoxItem Text="Nam" Value="Nam"/>
                                  <control:ComboBoxItem Text= "Nữ" Value="Nữ"/>
                              </Items>
                     </control:AutoComplete>
                </div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblPhoneExtendNumber"
                              runat="server" 
                              Text="Điện thoại bàn"/>
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                              ID="txtPhoneExtendNumber"
                              runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblRole"
                              runat="server"
                              Text ="Chức vụ"/>

                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                              ID="txtRole"
                              Enabled = "False"
                              runat="server" />
                </div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblBranch"
                              runat="server" 
                              Text="Đơn vị"/>
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                              ID="txtBranch"
                              Enabled = "False"
                              runat="server" />
                </div>
            </div>
                <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-10">
                    <asp:Button CssClass="btn btn-primary"
                              ID="btnUpdate"
                              OnClick="UpdateEmployee"
                              runat="server"
                              Text="Cập nhật" />
                </div>
            </div>
        </div>        
    </ContentTemplate>
</asp:UpdatePanel>
