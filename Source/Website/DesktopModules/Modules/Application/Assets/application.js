var gloDefaultCountryCode = "VN";
var gloPanelState =
{
    "ApplicationInfo": true,
    "CustomerInfo": true,
    "ContactInfo": false,
    "HistoryInfo": false,
    "ProcessInfo": true
};


function initializePage()
{
    renderControl();
    bindEvent();
}


// RENDER
function renderControl()
{
    renderPanel();
}

function renderPanel()
{
    $.each($(".dnnPanels"), function()
    {
        var state = gloPanelState[this.id] === true ? "open" : "close";
        $(this).dnnPanels(
            {
                defaultState: state
            });
    });
}


// EVENT
function bindEvent()
{
    // BASIC CARD INDICATOR
    getJQueryControl("ctrlIsBasicCard").bind("change", processOnCardIndicatorChange);
    processOnCardIndicatorChange();

    // EMBOSSING NAME
    getJQueryControl("ctrlFullName").bind("blur", processOnFullNameChange);
}

function processOnCardIndicatorChange()
{
    getControl("ctrlBasicCardNumber").disabled = getControl("ctrlIsBasicCard").value === "1";
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

function getEmbossingName(value, maxLength)
{
    value = removeUnicode(replaceAll(value.trim(), "  ", " ").toUpperCase());
    if (value.length > maxLength)
    {
        var array = value.split(" ");
        if (array.length === 1)
        {
            value = value.substring(value.length - maxLength);
        }
        else
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

function processOnSelectCountry(element)
{
    var value = element.value;
    if (value === gloDefaultCountryCode)
    {
        return true;
    }

    var id = $(element).attr("control-id");
    element = getControl(id);
    return element.options && element.options.length > 1;
}


// LIBRARY
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