<%@ Control Language="C#" AutoEventWireup="false" CodeFile="EmployeeUpload.ascx.cs" Inherits="DesktopModules.Modules.EmployeeManagement.EmployeeUpload" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnUpload" />
    </Triggers>
    <ContentTemplate>
        <div class="form-group">
            <asp:PlaceHolder ID="phMessage"
                             runat="server" />
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
                    <a class="btn btn-default" href="<%= LinkTemplate %>" target="_blank">Download Template</a>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    function checkFileExtension()
    {
        return validateFileExtension(getControl("fupFile"), ["xls", "xlsx"]);
    }
</script>