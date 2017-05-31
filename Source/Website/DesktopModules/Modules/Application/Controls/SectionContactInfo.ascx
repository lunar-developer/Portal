﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionContactInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionContactInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<div class="col-sm-4">
    <div class="form-group">
        <div class="col-sm-12 control-label">
            <h4 class="c-font-bold c-font-uppercase c-left c-margin-b-10">HỘ KHẨU</h4>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Address 1 (Home)"
                                 Text="Dòng thứ 1" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlHomeAddress01"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Address 2 (Home)"
                                 Text="Dòng thứ 2" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlHomeAddress02"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Country (Home)"
                                 Text="Quốc Gia" />
        </div>
        <div class="col-sm-12">
            <control:Combobox autocomplete="off"
                              AutoPostBack="True"
                              CssClass="form-control c-theme"
                              ID="ctrlHomeCountry" control-id="ctrlHomeState"
                              OnSelectedIndexChanged="ProcessOnSelectCountry" onchange="if(processOnSelectCountry(this) === false) return;"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="State (Home)"
                                 Text="Tỉnh/Thành Phố" />
        </div>
        <div class="col-sm-12">
            <control:Combobox autocomplete="off"
                              AutoPostBack="True"
                              CssClass="form-control c-theme"
                              ID="ctrlHomeState"
                              OnSelectedIndexChanged="ProcessOnSelectState"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="City (Home)"
                                 Text="Quận/Huyện" />
        </div>
        <div class="col-sm-12">
            <control:Combobox autocomplete="off"
                              CssClass="form-control c-theme"
                              ID="ctrlHomeCity"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Address 3 (Home)"
                                 Text="Phường/Xã" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlHomeAddress03"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Phone 1 (Home)"
                                 Text="Điện thoại" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlHomePhone01"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Remarks (Home)"
                                 Text="Ghi chú" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlHomeRemark"
                         runat="server" />
        </div>
    </div>
</div>

<div class="col-sm-4">
    <div class="form-group">
        <div class="col-sm-12 control-label">
            <h4 class="c-font-bold c-font-uppercase c-left c-margin-b-10">TẠM TRÚ 1</h4>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Address 1 (Alternative 1)"
                                 Text="Dòng thứ 1" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlAlternative01Address01"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Address 2 (Alternative 1)"
                                 Text="Dòng thứ 2" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlAlternative01Address02"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Country (Alternative 1)"
                                 Text="Quốc Gia" />
        </div>
        <div class="col-sm-12">
            <control:Combobox autocomplete="off"
                              AutoPostBack="True"
                              CssClass="form-control c-theme"
                              ID="ctrlAlternative01Country"
                              OnSelectedIndexChanged="ProcessOnSelectCountry"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="State (Alternative 1)"
                                 Text="Tỉnh/Thành Phố" />
        </div>
        <div class="col-sm-12">
            <control:Combobox autocomplete="off"
                              AutoPostBack="True"
                              CssClass="form-control c-theme"
                              ID="ctrlAlternative01State"
                              OnSelectedIndexChanged="ProcessOnSelectState"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="City (Alternative 1)"
                                 Text="Quận/Huyện" />
        </div>
        <div class="col-sm-12">
            <control:Combobox autocomplete="off"
                              CssClass="form-control c-theme"
                              ID="ctrlAlternative01City"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Address 3 (Alternative 1)"
                                 Text="Phường/Xã" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlAlternative01Address03"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Phone 1 (Alternative 1)"
                                 Text="Điện thoại" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlAlternative01Phone01"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Remarks (Alternative 1)"
                                 Text="Ghi chú" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlAlternative01Remark"
                         runat="server" />
        </div>
    </div>
</div>

<div class="col-sm-4">
    <div class="form-group">
        <div class="col-sm-12 control-label">
            <h4 class="c-font-bold c-font-uppercase c-left c-margin-b-10">TẠM TRÚ 2</h4>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Address 1 (Alternative 2)"
                                 Text="Dòng thứ 1" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlAlternative02Address01"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Address 2 (Alternative 2)"
                                 Text="Dòng thứ 2" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlAlternative02Address02"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Country (Alternative 2)"
                                 Text="Quốc Gia" />
        </div>
        <div class="col-sm-12">
            <control:Combobox autocomplete="off"
                              AutoPostBack="True"
                              CssClass="form-control c-theme"
                              ID="ctrlAlternative02Country"
                              OnSelectedIndexChanged="ProcessOnSelectCountry"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="State (Alternative 2)"
                                 Text="Tỉnh/Thành Phố" />
        </div>
        <div class="col-sm-12">
            <control:Combobox autocomplete="off"
                              AutoPostBack="True"
                              CssClass="form-control c-theme"
                              ID="ctrlAlternative02State"
                              OnSelectedIndexChanged="ProcessOnSelectState"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="City (Alternative 2)"
                                 Text="Quận/Huyện" />
        </div>
        <div class="col-sm-12">
            <control:Combobox autocomplete="off"
                              CssClass="form-control c-theme"
                              ID="ctrlAlternative02City"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Address 3 (Alternative 2)"
                                 Text="Phường/Xã" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlAlternative02Address03"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Phone 1 (Alternative 2)"
                                 Text="Điện thoại" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlAlternative02Phone01"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-12 double-label-inline">
            <control:DoubleLabel runat="server"
                                 SubText="Remarks (Alternative 2)"
                                 Text="Ghi chú" />
        </div>
        <div class="col-sm-12">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlAlternative02Remark"
                         runat="server" />
        </div>
    </div>
</div>