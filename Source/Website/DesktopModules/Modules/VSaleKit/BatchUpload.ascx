<%@ Control Language="C#" AutoEventWireup="false" CodeFile="BatchUpload.ascx.cs" Inherits="DesktopModules.Modules.VSaleKit.BatchUpload" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.VSaleKit.Database" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnUpload" />
    </Triggers>
    <ContentTemplate>
        <div class="c-content-title-1 clearfix c-margin-b-20 c-title-md">
            <h3 class="c-font-bold c-font-uppercase">UPLOAD HỒ SƠ</h3>
            <div class="c-bg-blue c-line-left"></div>
        </div>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-3 control-label">
                    <dnn:Label ID="lblFileBrowser"
                               runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:FileUpload CssClass="form-control c-theme"
                                    ID="fupFile"
                                    runat="server" />
                </div>
                <div class="col-sm-3"></div>
            </div>
            <div class="form-group">
                <div class="col-sm-5"></div>
                <div class="col-sm-7">
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnUpload"
                                OnClick="Upload"
                                OnClientClick="return checkFileExtension();"
                                runat="server"
                                Text="Upload" />
                </div>
            </div>

            <!-- Import Result -->
            <div id="divResult"
                 runat="server"
                 Visible="False">
                <div class="form-group">
                    <p>
                        Tổng số dòng:
                        <asp:Label CssClass="c-font-bold"
                                   ID="lblTotalRows"
                                   runat="server" />
                    </p>
                    <p>
                        Tổng số dòng bị lỗi:
                        <asp:Label CssClass="c-font-bold c-color-error"
                                   ID="lblTotalExceptions"
                                   runat="server" />
                    </p>
                    <p>
                        <asp:Label ID="lblMessage"
                                   runat="server" />
                    </p>
                </div>
                <div id="divForm"
                     runat="server">
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <dnn:Label ID="lblApplicationType"
                                       runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList CssClass="form-control c-theme"
                                              ID="ddlApplicationType"
                                              runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <dnn:Label ID="lblPolicy"
                                       runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <control:Combobox CssClass="form-control c-theme"
                                              ID="ddlPolicy"
                                              runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2 control-label"></div>
                        <div class="col-sm-10">
                            <asp:Button CssClass="btn btn-primary"
                                        ID="btnOK"
                                        OnClick="Process"
                                        OnClientClick="return onBeforeProcess();"
                                        runat="server"
                                        Text="Xác Nhận" />
                            <asp:Button CssClass="btn btn-default"
                                        ID="btnViewPolicy"
                                        OnClick="LoadPolicyDetail"
                                        OnClientClick="return onBeforeLoadPolicy();"
                                        runat="server"
                                        Text="Xem Thông Tin Chính Sách" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <control:Grid AllowPaging="true"
                                 AutoGenerateColumns="False"
                                 CssClass="dnnGrid"
                                 EnableViewState="True"
                                 ID="gridData"
                                 OnPageIndexChanged="OnPageIndexChanging"
                                 OnPageSizeChanged="OnPageSizeChanging"
                                 PageSize="10"
                                 runat="server">
                        <MasterTableView>
                            <Columns>
                                <dnn:DnnGridBoundColumn DataField="CustomerID"
                                                        HeaderText="CustomerID">
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="CustomerName"
                                                        HeaderText="CustomerName">
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="LegalType">
                                    <ItemTemplate>
                                        <%#FormatIdentityType(Eval(BatchDataTable.LegalType).ToString()) %>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="CreditLimit">
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatDecimal(Eval(BatchDataTable.CreditLimit).ToString()) %>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="Priority">
                                    <ItemTemplate>
                                        <%#FormatPriority(Eval(BatchDataTable.Priority).ToString()) %>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridBoundColumn DataField="UserName"
                                                        HeaderText="UserName">
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="Description"
                                                        HeaderText="Description">
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="ProcessCode">
                                    <ItemTemplate>
                                        <%#GetResource(Eval(BatchDataTable.ProcessCode) + ".Text") %>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                    <control:Grid AllowPaging="true"
                                 AutoGenerateColumns="True"
                                 CssClass="dnnGrid"
                                 EnableViewState="True"
                                 ID="gridException"
                                 OnPageIndexChanged="OnPageIndexChanging"
                                 OnPageSizeChanged="OnPageSizeChanging"
                                 PageSize="10"
                                 runat="server">
                    </control:Grid>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function checkFileExtension()
    {
        return validateFileExtension(getControl("fupFile"), ["csv"]);
    }

    function onBeforeProcess()
    {
        var array = ["ddlApplicationType", "ddlPolicy"];
        return validateOptionArray(array);
    }

    function onBeforeLoadPolicy()
    {
        return validateOption(getControl("ddlPolicy"));
    }
</script>