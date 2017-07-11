<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ReportInquiry.ascx.cs" Inherits="DesktopModules.Modules.DynamicReport.ReportInquiry" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<script src="<%= FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/DynamicReport/Assets/js/moment.js") %>"></script>
<script src="<%= FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/DynamicReport/Assets/js/daterangepicker.js") %>"></script>


<asp:UpdatePanel ID="updatePanel"
    runat="server"
    UpdateMode="Conditional">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExport" />
        <asp:AsyncPostBackTrigger ControlID="ddlReport" />
    </Triggers>
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label runat="server"
                        Text="Báo cáo" />
                </div>
                <div class="col-sm-6">
                    <control:AutoComplete
                        EmptyMessage="Vui lòng chọn Báo cáo cần xem"
                        AutoPostBack="True"
                        ID="ddlReport"
                        OnSelectedIndexChanged="ProcessOnSelectReport"
                        runat="server" />
                </div>
            </div>
            <div id="DivForm"
                runat="server">
            </div>
            <div class="form-group">
                <asp:UpdatePanel runat="server"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="col-sm-2"></div>
                        <div class="col-sm-10">
                            <asp:Button CssClass="btn btn-primary c-margin-t-0"
                                ID="btnView"
                                OnClick="LoadReportData"
                                OnClientClick="return validate();"
                                runat="server"
                                Text="View" />
                            <asp:Button CssClass="btn btn-primary c-margin-t-0"
                                ID="btnExport"
                                OnClick="ExportData"
                                runat="server"
                                Text="Export" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upGridData"
                runat="server"
                UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-group table-responsive">
                        <control:Grid 
                            AutoGenerateColumns="true"
                            AllowFilteringByColumn="True"
                            ID="gridData"
                            OnNeedDataSource="OnNeedDataSource"
                            runat="server"
                            Visible="False" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>


        <asp:HiddenField ID="hidConnectionName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidDatabaseName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidSchemaName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidProcedureName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidIsAllowExport"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidListFieldName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidListFieldValue"
            runat="server"
            Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function renderForm()
    {
        var container = getJQueryControl("DivForm");
        container.find(".combobox").combobox();
        container.find(".form-calendar").calendar();
    }

    function validate()
    {
        var list = getJQueryControl("DivForm")
            .find("input[is-require='True'], textarea[is-require='True'], select[is-require='True']");
        for (var i = 0; i < list.length; i++)
        {
            var element = list.get(i);
            switch (element.tagName.toLowerCase())
            {
                case "input":
                case "textarea":
                    if (validateInput(element) === false)
                    {
                        return false;
                    }
                    break;

                case "select":
                    if (validateOption(element) === false)
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }
</script>
