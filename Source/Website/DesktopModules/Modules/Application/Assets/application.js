var gloDefaultCountryCode = "VN";
var gloDefaultTaxCode = "2200269805";
var gloDefaultCompanyName = "NH TMCP VIETNAM THUONG TIN – VIETBANK";

var gloPanelState =
    {
        "ApplicationInfo": true,
        "CustomerInfo": true,
        "ContactInfo": false,
        "HistoryInfo": false,
        "AutoPayInfo": false,
        "FinanceInfo": false,
        "ReferenceInfo": false,
        "SaleInfo": false,
        "CollateralInfo": false,
        "PolicyInfo": false,
        "CardInfo": false,
        "AssessmentInfo": false,
        "ProcessInfo": true
    };


function initializePage()
{
    // SetTimeout to allow Telerik control fully loaded
    setTimeout(function ()
    {
        renderControl();
        bindEvent();
    }, 100);
}


// RENDER
function renderControl()
{
    renderPanel();
}

function renderPanel()
{
    $.each($(".dnnPanels"), function ()
    {
        var state = gloPanelState[this.id] === true ? "open" : "close";
        $(this).dnnPanels(
        {
            defaultState: state,
            onExpand: function ()
            {
                gloPanelState[this.id] = true;
            },
            onHide: function ()
            {
                gloPanelState[this.id] = false;
            }
        });
    });
}


// EVENT
function bindEvent()
{
    // Button Events
    confirmMessage("#btnProcess", getConfirmMessage, undefined, undefined, undefined, onBeforeProcess);

    // Auto Expand Panels when child element receive focus
    $(".dnnFormSectionHead span").focus(function ()
    {
        var $element = $(this).next();
        if ($element.hasClass("dnnSectionExpanded"))
        {
            return;
        }

        $element.click();
        $element.focus();
    });

    // BASIC CARD INDICATOR
    processOnCardIndicatorChange();
}

function processOnCardIndicatorChange()
{
    var isDisable = $find("ctrlCardTypeIndicator").get_value() === "B";
    var element = getControl("ctrlBasicCardNumber");
    element.disabled = isDisable;
    if (isDisable)
    {
        element.value = "";
    }
}

function processOnFullNameChange()
{
    var element = getControl("ctrlEmbossName");
    if (element.value.trim() !== "")
    {
        return;
    }

    element.value = getEmbossingName(getControl("ctrlFullName").value, element.maxLength);
}

function processOnSelectCountry(sender)
{
    var isPostBack;
    var value = sender.get_value();
    if (value === gloDefaultCountryCode)
    {
        isPostBack = true;
    }
    else
    {
        var id = sender.get_attributes().getAttribute("control-id");
        var element = $find(id);
        isPostBack = element.get_items().get_count() > 1;
    }

    if (isPostBack)
    {
        __doPostBack(sender.get_id(), '{\"Command\" : \"Select\"}');
    }
}

function processOnStaffIndicatorChange(sender)
{
    var value = sender.get_value();
    if (value === "Y")
    {
        getControl("ctrlCompanyTaxNo").value = gloDefaultTaxCode;
        getControl("ctrlCompanyName").value = gloDefaultCompanyName;
    }
    else
    {
        getControl("ctrlCompanyTaxNo").value = "";
        getControl("ctrlCompanyName").value = "";
    }
}

function onSelectGender(sender)
{
    var value = sender.get_value();
    getRadCombobox("ctrlTitleOfAddress").findItemByValue(value === "M" ? "MR" : "MS").select();
}


// VALIDATATION
function validateData(element)
{
    try
    {
        var arrRequireFields = [
            "ctrlCustomerID", "ctrlBasicCardNumber", "ctrlFullName", "ctrlEmbossName", "ctrlMobile01",
            "ctrlHomeAddress01", "ctrlHomePhone01"
        ];
        var arrRequireRadOptions = [
            "ctrlHomeCountry"
        ];
        if (validateInputArray(arrRequireFields) === false
            || validateRadOptionArray(arrRequireRadOptions) === false)
        {
            return false;
        }

        var radCombobox = getRadCombobox("ctrlHomeCountry");
        if (radCombobox.get_value() === gloDefaultCountryCode
            && validateRadOptionArray(["ctrlHomeState", "ctrlHomeCity"]) === false)
        {
            return false;
        }

        $(element).hide();
        return true;
    }
    catch (e)
    {
        log(e);
        return false;
    }
}

function onBeforeProcess()
{
    var array;
    var combobox = getRadCombobox("ctrlRoute");
    var actionCode = combobox.get_selectedItem().get_attributes()._data["ActionCode"];
    switch (actionCode)
    {
        case "APPROVE":
            if (validateRadOption("ctrlDecisionCode") === false || validateRadMultiSelectOption("ctrlDecisionReason") === false)
            {
                setRequireUpdate();
                return false;
            }            
            break;
    }

    if (isRequireUpdate() === true)
    {
        return false;
    }

    return true;
}

function onBeforeQueryCustomer()
{
    return validateInput(getControl("ctrlCustomerID"));
}

function onBeforeQueryAccount()
{
    return validateInput(getControl("ctrlPaymentCIFNo"));
}

function onBeforeQueryCollateral()
{
    return validateInput(getControl("ctrlCollateralID"));
}


// LIBRARY
function getEmbossingName(value, maxLength)
{
    value = removeUnicode(replaceAll(value.trim(), "  ", " ").toUpperCase());
    if (value.length > maxLength)
    {
        var array = value.split(" ");
        if (array.length === 1)
        {
            value = value.substring(value.length - maxLength);
        } else
        {
            // Keep the First and Last word
            var length = value.length;
            for (var i = 1; i < array.length - 1; i++)
            {
                length -= array[i].length - 1;
                array[i] = array[i][0];

                if (length <= maxLength)
                {
                    break;
                }
            }
            value = array.join(" ");
            if (value.length > maxLength)
            {
                value = value.substring(value.length - maxLength).trim();
            }
        }
    }

    return value;
}

function alertOnConstruct()
{
    alertMessage("Chức năng này đang được hoàn thiện!");
    return false;
}

function getConfirmMessage()
{
    var combobox = getRadCombobox("ctrlRoute");
    return "Bạn có chắc muốn " +
        (combobox ? "<b>" + combobox.get_text() + "</b>?" : "tiếp tục?");
}

function setRequireUpdate()
{
    getJQueryControl("ctrlIsRequireUpdate").val("1");
}

function isRequireUpdate()
{
    if (getJQueryControl("ctrlIsRequireUpdate").val() === "1")
    {
        alertMessage("Vui lòng <b>CẬP NHẬT THÔNG TIN</b> trước khi tiếp tục.");
        return true;
    }
    return false;
}