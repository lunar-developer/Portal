<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ApplicationDetail.ascx.cs" Inherits="DesktopModules.Modules.VSaleKit.ApplicationDetail" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="control" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
    .iconButton {
        cursor: pointer;
        margin-top: 10px;
        font-weight: normal !important;
        color: #7f8c97 !important;
        font-size: 18px;
    }

        .iconButton:hover {
            color: #5893dd !important;
        }

    .iconInline {
        display: inline-block;
        margin-left: 10px;
    }
</style>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                    runat="server" />
            </div>

            <div id="PanelApplication">
                <div class="form-group">
                    <ul class="dnnAdminTabNav dnnClear">
                        <li>
                            <a href="#tabGeneralInfo">Thông Tin Hồ Sơ</a>
                        </li>
                        <li id="TabDocumentInfo" runat="server">
                            <a href="#<%= TabDocumentContainer.ClientID %>">Chứng Từ</a>
                        </li>
                        <li id="TabHistoryInfo" runat="server">
                            <a href="#<%= TabHistoryContainer.ClientID %>">Lịch Sử</a>
                        </li>
                    </ul>
                </div>

                <!-- Tab General Information -->
                <div class="dnnClear"
                    id="tabGeneralInfo">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <dnn:Label ID="lblCustomerName"
                                    runat="server" />
                            </div>
                            <div class="col-sm-8">
                                <asp:TextBox
                                    CssClass="form-control c-theme"
                                    ID="txtCustomerName"
                                    runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <dnn:Label ID="lblMobileNumber"
                                    runat="server" />
                            </div>
                            <div class="col-sm-8">
                                <asp:TextBox CssClass="form-control c-theme"
                                    ID="txtMobileNumber"
                                    runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <dnn:Label ID="lblIdentityType"
                                    runat="server" />
                            </div>
                            <div class="col-sm-8">
                                <control:AutoComplete
                                    ID="ddlIdentityType"
                                    runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <dnn:Label ID="lblCustomerID"
                                    runat="server" />
                            </div>
                            <div class="col-sm-8">
                                <asp:TextBox CssClass="form-control c-theme"
                                    ID="txtCustomerID"
                                    runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <dnn:Label ID="lblBranchLabel"
                                    Text="Chi nhánh"
                                    HelpText="Chi nhánh tiếp nhận"
                                    runat="server" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:Label ID="lblBranch"
                                    runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <dnn:Label ID="lblUniqueIDLabel"
                                    Text="Mã hồ sơ"
                                    HelpText="Mã hồ sơ"
                                    runat="server" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:Label ID="lblUniqueID"
                                    runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <dnn:Label ID="lblApplicationType"
                                    runat="server" />
                            </div>
                            <div class="col-sm-8">
                                <control:AutoComplete
                                    ID="ddlApplicationType"
                                    runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <dnn:Label ID="lblPolicy"
                                    runat="server" />
                            </div>
                            <div class="col-sm-7">
                                <control:AutoComplete
                                    ID="ddlPolicy"
                                    runat="server">
                                </control:AutoComplete>
                            </div>
                            <div class="col-sm-1 control-value" style="text-indent: 0">
                                <asp:LinkButton runat="server" OnClick="ViewPolicyDetai" TabIndex="0">
                                    <i class="fa fa-search"></i>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <dnn:Label ID="lblProposalLimit"
                                    runat="server" />
                            </div>
                            <div class="col-sm-8">
                                <asp:TextBox CssClass="form-control c-theme"
                                    ID="txtProposalLimit"
                                    runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <dnn:Label ControlName="chkIsHighPriority"
                                    ID="lblIsHighPriority"
                                    runat="server" />
                            </div>
                            <div class="col-sm-8">
                                <control:AutoComplete runat="server" ID="ddlPriority">
                                    <Items>
                                        <control:RadComboBoxItem Value="T" Text="Thường" />
                                        <control:RadComboBoxItem Value="N" Text="Nhanh" />
                                    </Items>
                                </control:AutoComplete>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4 control-label">
                                <dnn:Label ID="lblStatusLabel"
                                    runat="server" />
                            </div>
                            <div class="col-sm-8 control-value">
                                <asp:Label ID="lblStatus" CssClass="c-font-bold"
                                    runat="server" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Tab Document -->
                <div
                    class="dnnClear"
                    id="TabDocumentContainer"
                    runat="server">
                </div>

                <!-- Tab History -->
                <div class="dnnClear" id="TabHistoryContainer" runat="server">
                    <div class="form-group">
                        <div class="col-sm-12">
                            <control:Grid
                                AutoGenerateColumns="False"
                                AllowFilteringByColumn="False"
                                CssClass="dnnGrid"
                                ID="gridLogData"
                                runat="server">
                                <ClientSettings
                                    AllowColumnsReorder="false"
                                    AllowDragToGroup="false"
                                    AllowRowsDragDrop="false">
                                    <Selecting AllowRowSelect="false" EnableDragToSelectRows="false" />
                                </ClientSettings>
                                <MasterTableView>
                                    <Columns>
                                        <control:GridTemplateColumn HeaderText="ModifyDateTime">
                                            <HeaderStyle Width="16%" />
                                            <ItemTemplate>
                                                <%# FunctionBase.FormatDate(Eval("DateCreate").ToString().Substring(0, 14)) %>
                                            </ItemTemplate>
                                        </control:GridTemplateColumn>
                                        <control:GridTemplateColumn HeaderText="ModifyUserID">
                                            <HeaderStyle Width="20%" />
                                            <ItemTemplate>
                                                <%# FunctionBase.FormatUserName(Eval("UserCreate").ToString().Trim()) %>
                                            </ItemTemplate>
                                        </control:GridTemplateColumn>
                                        <control:GridTemplateColumn DataField="ActionCode"
                                            HeaderText="ActionCode">
                                            <HeaderStyle Width="20%" />
                                            <ItemTemplate>
                                                <%# FormatAction(Eval("Status").ToString().Trim()) %>
                                            </ItemTemplate>
                                        </control:GridTemplateColumn>
                                        <control:GridBoundColumn DataField="Description"
                                            HeaderText="Remark">
                                            <HeaderStyle Width="30%" />
                                        </control:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </control:Grid>
                        </div>
                    </div>
                </div>
            </div>


            <div id="PanelNavigator"
                class="c-margin-t-20"
                runat="server">
                <div class="col-sm-12">
                    <div class="form-group">
                        <div class="col-sm-2 control-label">
                            <dnn:Label ID="lblRemark"
                                runat="server" />
                        </div>
                        <div class="col-sm-10">
                            <asp:TextBox CssClass="form-control c-theme"
                                Height="100"
                                ID="txtRemark"
                                runat="server"
                                TextMode="MultiLine" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-10">
                            <asp:Button CssClass="btn btn-primary"
                                ID="btnSave"
                                OnClick="Save"
                                OnClientClick="return onBeforeUpdate();"
                                runat="server"
                                Text="Lưu" />
                            <asp:Button CssClass="btn btn-primary"
                                ID="btnUpdate"
                                OnClick="Update"
                                OnClientClick="return onBeforeUpdate()"
                                runat="server"
                                Text="Cập nhật" />

                            <asp:Button CssClass="btn btn-primary"
                                ID="btnApprove"
                                OnClick="ProcessDocument"
                                OnClientClick="return onBeforeProcess(arguments[1], this)"
                                CommandName="Approve"
                                runat="server"
                                Text="Trình hồ sơ" />
                            <asp:Button CssClass="btn btn-primary"
                                ID="btnRecall"
                                OnClick="ProcessDocument"
                                CommandName="Recall"
                                OnClientClick="return onBeforeProcess(arguments[1], this)"
                                runat="server"
                                Text="Rút hồ sơ" />
                            <asp:Button CssClass="btn btn-primary"
                                ID="btnClose"
                                OnClick="ProcessDocument"
                                CommandName="Close"
                                OnClientClick="return onBeforeProcess(arguments[1], this)"
                                runat="server"
                                Text="Đóng hồ sơ" />
                            <asp:Button CssClass="btn btn-primary"
                                ID="btnReturn"
                                OnClick="ProcessDocument"
                                CommandName="Return"
                                OnClientClick="return onBeforeProcess(arguments[1], this)"
                                runat="server"
                                Text="Trả lại hồ sơ" />


                            <asp:Button ID="btnReloadFile"
                                OnClick="ReloadFile"
                                CssClass="invisible"
                                runat="server" />
                            <asp:Button ID="btnUploadFile"
                                OnClick="UploadFile"
                                CssClass="invisible"
                                runat="server" />
                            <asp:Button ID="btnSortFile"
                                CssClass="invisible"
                                OnClick="SortFile"
                                runat="server" />
                            <asp:Button ID="btnDeleteFile"
                                OnClick="DeleteFile"
                                CssClass="invisible"
                                runat="server" />
                            <asp:Button ID="btnEditFile"
                                OnClick="EditFile"
                                CssClass="invisible"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <asp:HiddenField ID="hidUniqueID"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidStatus"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidBranchCode"
            runat="server"
            Visible="False" />

        <asp:HiddenField ID="hidRoleName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidCreateUserName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidProcessUserName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidNextProcessUserName"
            runat="server"
            Visible="False" />
        <asp:HiddenField ID="hidIsLinkApplication"
            runat="server"
            Visible="False" />


        <asp:HiddenField
            ID="hidFileID"
            runat="server" />
        <asp:HiddenField
            ID="hidFileName"
            runat="server" />
        <asp:HiddenField
            ID="hidFileData"
            runat="server" />
        <asp:HiddenField
            ID="hidDocumentCode"
            runat="server" />
        <asp:HiddenField
            ID="hidFileNumber"
            runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    var gloButtonClicked;

    addPageLoaded(function ()
    {
        // Render tabs
        $("#PanelApplication").dnnTabs();

        // Render Panel
        $(".dnnPanels").dnnPanels({
            defaultState: "open"
        });

        // Attach event handler
        var formatHandler = function ()
        {
            formatCurrency(this);
        }
        getJQueryControl("txtProposalLimit").on("keyup blur", formatHandler);

        // Confirm
        confirmMessage("a.iconDelete", "Bạn có chắc muốn xoá nội dung này?", undefined, undefined, undefined, deleteFile);

        // Enable Lightbox
        enableLightBox(getControl("TabDocumentContainer"));
    }, true);

    function onBeforeUpdate()
    {
        var arrayInputs = ["txtCustomerName", "txtMobileNumber", "txtCustomerID", "txtProposalLimit"];
        return validateInputArray(arrayInputs);
    }

    function getFile(id)
    {
        getControl("hidFileID").value = id;
    }

    function editFile(documentCode, fileId, fileNumber)
    {
        getControl("hidDocumentCode").value = documentCode;

        var image = $("#image" + fileId);
        getControl("hidFileID").value = fileId;
        getControl("hidFileName").value = image.attr("name");
        getControl("hidFileData").value = image.attr("src");
        getControl("hidFileNumber").value = fileNumber;
        getJQueryControl("btnEditFile").click();
    }

    function deleteFile()
    {
        getJQueryControl("btnDeleteFile").click();
    }

    function uploadFile(event, documentCode)
    {
        getControl("hidDocumentCode").value = documentCode;
        getJQueryControl("btnUploadFile").click();
        event.stopPropagation();
    }

    function sortFile(event, documentCode)
    {
        getControl("hidDocumentCode").value = documentCode;
        getJQueryControl("btnSortFile").click();
        event.stopPropagation();
    }

    function reloadFile()
    {
        getJQueryControl("btnReloadFile").click();
    }

    function onBeforeProcess(isConfirmed, element)
    {
        if (isConfirmed === true)
        {
            return true;
        }
        gloButtonClicked = element;
        var message = "Bạn có chắc muốn <b>" + element.value + "</b> này không?";
        var msg = {
            text: message,
            title: "Nhắc nhở",
            yesText: "Đồng ý",
            noText: "Hủy",
            buttonYesClass: "btn btn-primary",
            buttonNoClass: "btn btn-default",
            draggable: true,
            isButton: true,
            callbackTrue: callBackTrue,
            onBeforeOpen: element.id === getClientID("btnApprove") ? isUploadRequireDocument : null
        };
        $.dnnConfirm(msg);
        return false;
    }

    function callBackTrue()
    {
        $(gloButtonClicked).trigger("click", [true]);
        gloButtonClicked = "";
    }

    function isUploadRequireDocument()
    {
        var controls = $("#" + getClientID("TabDocumentContainer") + " input.TotalFiles");
        for (var i = 0; i < controls.length; i++)
        {
            var control = controls[i];
            var value = parseInt(control.value, 10);
            if (value === 0)
            {
                alertMessage("Vui lòng đính kèm chứng từ <b>" + control.alt + "</b>.");
                return false;
            }
        }
        return true;
    }
</script>

