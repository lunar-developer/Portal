<%@ Control Language="C#" AutoEventWireup="false" CodeFile="BatchUpload.ascx.cs" Inherits="DesktopModules.Modules.VSaleKit.BatchUpload" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.VSaleKit.Database" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="control" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnUpload" />
        <asp:AsyncPostBackTrigger ControlID="btnView" />
        <asp:AsyncPostBackTrigger ControlID="btnOK" />
    </Triggers>
    <ContentTemplate>
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
                        Accept=".xls, .xlsx"
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
                    <a class="btn btn-default" href="<%= LinkTemplate %>" target="_blank">Download Template</a>
                </div>
            </div>
            <div id="divResult"
                runat="server"
                visible="False">
                <div class="form-group">
                    <div class="col-sm-12">
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
                </div>
                <div class="form-group">
                    <div class="col-sm-12">
                        <asp:PlaceHolder ID="phMessage"
                            runat="server" />
                    </div>
                </div>
                <div id="divForm"
                    runat="server">
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <dnn:Label ID="lblApplicationType"
                                runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <control:AutoComplete
                                ID="ddlApplicationType"
                                runat="server" />
                        </div>
                        <div class="col-sm-2 control-label">
                            <dnn:Label ID="lblPolicy"
                                runat="server" />
                        </div>
                        <div class="col-sm-3">
                            <control:AutoComplete
                                ID="ddlPolicy"
                                runat="server" />
                        </div>
                        <div class="col-sm-1 control-value" style="text-indent: 0">
                            <asp:LinkButton runat="server" ID="btnView"
                                OnClick="LoadPolicyDetail"
                                OnClientClick="return onBeforeLoadPolicy();"
                                TabIndex="0">
                                <i class="fa fa-search"></i>
                            </asp:LinkButton>
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
                        </div>
                    </div>
                </div>
                <div class="form-group table-responsive">
                    <div class="col-sm-12">
                        <control:Grid
                            AutoGenerateColumns="False"
                            ID="gridData"
                            OnNeedDataSource="OnNeedDataSource"
                            runat="server">
                            <MasterTableView>
                                <Columns>
                                    <control:GridBoundColumn DataField="CustomerID"
                                        HeaderText="CustomerID">
                                    </control:GridBoundColumn>
                                    <control:GridBoundColumn DataField="CustomerName"
                                        HeaderText="CustomerName">
                                    </control:GridBoundColumn>
                                    <control:GridTemplateColumn HeaderText="LegalType">
                                        <ItemTemplate>
                                            <%#FormatIdentityTypeCode(Eval(BatchDataTable.IdentityTypeCode).ToString()) %>
                                        </ItemTemplate>
                                    </control:GridTemplateColumn>
                                    <control:GridTemplateColumn HeaderText="CreditLimit">
                                        <ItemTemplate>
                                            <%#FunctionBase.FormatDecimal(Eval(BatchDataTable.CreditLimit).ToString()) %>
                                        </ItemTemplate>
                                    </control:GridTemplateColumn>
                                    <control:GridTemplateColumn HeaderText="Priority">
                                        <ItemTemplate>
                                            <%#FormatPriority(Eval(BatchDataTable.Priority).ToString()) %>
                                        </ItemTemplate>
                                    </control:GridTemplateColumn>
                                    <control:GridBoundColumn DataField="UserName"
                                        HeaderText="UserName">
                                    </control:GridBoundColumn>
                                    <control:GridBoundColumn DataField="Description"
                                        HeaderText="Description">
                                    </control:GridBoundColumn>
                                    <control:GridTemplateColumn HeaderText="ProcessCode">
                                        <ItemTemplate>
                                            <%#GetResource(Eval(BatchDataTable.ProcessCode) + ".Text") %>
                                        </ItemTemplate>
                                    </control:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </control:Grid>
                        <control:Grid
                            AutoGenerateColumns="True"
                            ID="gridException"
                            OnNeedDataSource="OnNeedDataSource"
                            runat="server">
                        </control:Grid>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>


<script type="text/javascript">
    function checkFileExtension()
    {
        return validateFileExtension(getControl("fupFile"), ["xlsx", "xls"]);
    }

    function onBeforeProcess()
    {
        var array = ["ddlApplicationType", "ddlPolicy"];
        return validateRadOptionArray(array);
    }

    function onBeforeLoadPolicy()
    {
        return validateRadOption("ddlPolicy");
    }
</script>
