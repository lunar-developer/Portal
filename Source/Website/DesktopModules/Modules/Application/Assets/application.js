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
    return true;
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

function removeUnicode(value)
{
    return value
        .replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a")
        .replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A")
        .replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e")
        .replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E")
        .replace(/ì|í|ị|ỉ|ĩ/g, "i")
        .replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I")
        .replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o")
        .replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O")
        .replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u")
        .replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U")
        .replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y")
        .replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y")
        .replace(/đ/g, "d")
        .replace(/Đ/g, "D")
        .replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\:|\;|\"|\$|\&|\#|\[|\]|~|$|_/g, "")
        .replace(/-+-/g, "")
        .replace(/^\-+|\-+$/g, "");
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