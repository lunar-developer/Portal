<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicationHistory.ascx.cs" Inherits="DesktopModules.Modules.Application.ApplicationHistory" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>
        <div class="form-group col-sm-12" style="margin-bottom: 30px;">
            <div class="col-sm-2 control-label">
                <label style="margin-top: 8px;">Enter an Application ID: </label>
            </div>
            <div class="col-sm-3">
                <asp:TextBox
                    CssClass="form-control"
                    runat="server"
                    ID="tbApplicationID" 
                    placeholder = "Application ID"/>
            </div>
            <div class="col-sm-1">
                <asp:Button
                    style="margin-top: 0;"
                    CssClass="btn btn-primary"
                    ID="btnSearch"
                    OnClick="Search"
                    runat="server"
                    Text="Search" />
            </div>
        </div>
        <div class="form-group">
            <table class="table table-bordered table-hover">
                <thead>
                    <tr>
                        <th class="col-sm-3">
                            <control:Combobox
                                ID="ddlField"
                                CssClass="form-control c-theme"
                                AutoPostBack="False" 
                                onchange="doFilter();"
                                runat="server"/>
                        </th>						    
                        <th class="col-sm-4">
                            <asp:DropDownList
                                CssClass="form-control c-theme"
                                ID="ddlLeftVersion"
                                AutoPostBack="True" 
                                onselectedindexchanged="LeftVersionChange"
                                runat="server"/>
                        </th>
                        <th class="col-sm-4">
                            <asp:DropDownList
                                ID="ddlRightVersion"
                                CssClass="form-control c-theme"
                                AutoPostBack="True" 
                                onselectedindexchanged="RightVersionChange"
                                runat="server"/>
                        </th>
                        <th class="col-sm-1">
                            <asp:DropDownList
                                ID="ddlDiff"
                                CssClass="form-control c-theme"
                                AutoPostBack="False" 
                                onchange="doFilter();"
                                runat="server"/>

                        </th>
                    </tr>
                </thead>
                <tbody id="tableView" runat="server">
					    
                </tbody>
            </table>
        </div>
        
        <asp:HiddenField ID="hidApplicationId"
                         runat="server"
                         Value="XOQIXIAUREQAQWRFSFASDFADDD"
                         Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function doFilter()
    {
        var wrapId = getControl("tableView").id;
        // get all rows and show them.
        var rows = $("#" + wrapId + " tr");
        rows.show();

        // filter rows based on field selection
        var field = getControl("ddlField");
        var id = "#" + field.value;
        var filter = rows.filter(id);

        if ("109XXQIEIXUARJIOAJFK" !== field.value) {
            rows.not(filter).hide();
        }

        // filter rows based on diff selection.
        var diff = getControl("ddlDiff");
        var clazz = "." + diff.value;
        var filterDiff = rows.filter(clazz);
        
        if ("109XXQIEIXUARJIOAJFK" !== diff.value) {
            rows.not(filterDiff).hide();
        }
    }
</script>

