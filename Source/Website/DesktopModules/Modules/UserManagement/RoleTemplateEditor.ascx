<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoleTemplateEditor.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.RoleTemplateEditor" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register Src="~/controls/Label.ascx" TagName="Label" TagPrefix="control" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                                 runat="server" />
            </div>
            <div id="DivEditor"
                 runat="server">
                <div class="dnnPanels">
                    <h2 class="dnnFormSectionHead">
                        <a href="#">THÔNG TIN CHỨC DANH</a>
                    </h2>
                    <fieldset>
                        <div class="form-group">
                            <div class="col-md-2 control-label">
                                <control:Label HelpText="Vị trí công việc"
                                               IsRequire="True"
                                               runat="server"
                                               Text="Chức Danh"
                                               ViewStateMode="Disabled" />
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox CssClass="form-control c-theme"
                                             ID="tbTemplateName"
                                             placeholder="Chức Danh"
                                             runat="server" />
                            </div>
                            <div class="col-md-2 control-label">
                                <control:Label HelpText="Chi Nhánh"
                                               runat="server"
                                               Text="Chi Nhánh"
                                               ViewStateMode="Disabled" />
                            </div>
                            <div class="col-md-4">
                                <control:Combobox autocomplete="off"
                                                  AutoPostBack="True"
                                                  CssClass="form-control c-theme"
                                                  ID="ddlBranch"
                                                  OnSelectedIndexChanged="ProcessOnChangeBranch"
                                                  runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-2 control-label">
                                <control:Label runat="server"
                                               Text="Ghi chú"
                                               ViewStateMode="Disabled" />
                            </div>
                            <div class="col-sm-4">
                                <asp:TextBox CssClass="form-control c-theme"
                                             Height="70"
                                             ID="tbRemark"
                                             runat="server"
                                             TextMode="MultiLine">
                                </asp:TextBox>
                            </div>
                            <div class="col-sm-2 control-label">
                                <control:Label HelpText="Dùng trong trường hợp tạm khóa Chức danh này"
                                               runat="server"
                                               Text="Disabled"
                                               ViewStateMode="Disabled" />
                            </div>
                            <div class="col-sm-4">
                                <asp:DropDownList autocomplete="off"
                                                  CssClass="form-control c-theme"
                                                  ID="ddlIsDisable"
                                                  runat="server">
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div class="dnnPanels">
                    <h2 class="dnnFormSectionHead">
                        <a href="#">THÔNG TIN PHÂN QUYỀN</a>
                    </h2>
                    <fieldset class="c-padding-20">
                        <div class="dnnPanels"
                             id="DivRoles"
                             runat="server">
                        </div>
                    </fieldset>
                </div>

                <div class="form-group">
                    <div class="col-md-2"></div>
                    <div class="col-md-8">
                        <asp:Button CssClass="btn btn-primary"
                                    ID="btnSave"
                                    OnClick="Update"
                                    OnClientClick="return validate();"
                                    runat="server"
                                    Text="Cập Nhật" />
                        <asp:Button CssClass="btn btn-primary"
                                    ID="btnDelete"
                                    OnClick="Delete"
                                    runat="server"
                                    Text="Xóa" />
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hidTemplateID"
                         runat="server"
                         Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function()
        {
            $(".dnnPanels").dnnPanels({
                defaultState: "open"
            });
        },
        true);

    function toggleGroup(element, groupId)
    {
        $(element)
            .closest("table")
            .find("input[group='" + groupId + "']")
            .attr("checked", element.checked);
    }

    function validate()
    {
        return validateInput(getControl("tbTemplateName"));
    }
</script>