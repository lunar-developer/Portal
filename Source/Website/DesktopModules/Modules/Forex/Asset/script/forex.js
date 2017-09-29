function ValidateNumberArray()
{
    for (var i = 0; i < array.length; i++) {
        if (validateNumber(getControl(array[i]),1,9999999) === false) {
            return false;
        }
    }
    return true;
}
function GetControlValue(controlName)
{
    var element = getControl(controlName);
    if (typeof element === "undefined" || element === null)
    {
        return "";
    }
    return element.value;
}
function FormatNumber(value)
{
    if (typeof value !== "undefined" && value !== null && value.length > 0)
    {
        return value.replace(/[^\d.]/g, '');
    }
    return "0";
}

function GetControlNumber(controlName)
{
    var value = GetControlValue(controlName);
    value = FormatNumber(value);
    return parseInt(value,10);
}
function GetMoneyByElement(element)
{
    if (typeof element === "undefined" || element === null)
    {
        return 0;
    }
    var value = FormatNumber(element.value);
    return parseFloat(value, 10);
}
function GetControlMoney(controlName) {
    var value = GetControlValue(controlName);
    value = FormatNumber(value);
    return parseFloat(value, 10);
}

function GetAcceptToChangeStatusMessage(statusId)
{
    var acceptToChangeStatusObj = $.parseJSON(GetControlValue("HiddenAcceptToChangeStatusArr"));
    for (var i = 0; i < acceptToChangeStatusObj.length; i++) {
        if (acceptToChangeStatusObj[i].key === statusId) {
            return acceptToChangeStatusObj[i].val;
        }
    }
  
    return "Bạn có chắc thực hiện thao tác này không";
}
function ValidateExchangeCreationForm()
{
    var arrRequireRateFields = [
        "txtBuyRateFT",  "txtSellRateFT", "txtBuyRateCash",
         "txtSellRateCash"
    ];
    var arrRequireTimeFields = [
        "txtDealTimeBuyFT",  "txtDealTimeSellFT", 
        "txtDealTimeBuyCash",  "txtDealTimeSellCash"
    ];
    var arrRequireRadOptions = ["ctExchangeCode"];
    if (validateInputArray(arrRequireRateFields) === false ||
        validateInputArray(arrRequireTimeFields) === false ||
        validateRadOptionArray(arrRequireRadOptions) === false) {
        return false;
    }
    for (var i = 0; i < arrRequireTimeFields.length; i++) {
        if (validateNumber(getControl(arrRequireTimeFields[i]), 15, 1800) === false) {
            return false;
        }
    }
    for (var j = 0; j < arrRequireRateFields.length; j++) {
        if (validateNumber(getControl(arrRequireRateFields[j]), 1, 999999) === false) {
            return false;
        }
    }
    return true;
}
function RequestTransactionValidate()
{
    if (validateRadOption("ctTransactionType") === false ||
        validateRadOption("ctExchangeCode") === false)
    {
        return false;
    }
    if (validateInput(getControl("calTransactionDate")) === false)
    {
        return false;
    }

    if (validateInput(getControl("txtQuantityTransactionAmount")) === false ||
        validateNumber(getControl("txtQuantityTransactionAmount"), 1, 1000000000000) === false) {
        return false;
    }
    if (validateInput(getControl("txtCustomerIDNo")) === false ||
        validateInput(getControl("txtCustomerIDNo")) === false)
    {
        return false;
    }

    if (validateRadOption("ctCustomerType") === false ||
        validateRadOption("ctReasonTransaction") === false ) {
        return false;
    }

    

    return true;
}

function ExceedLimit(element)
{
    var amount = GetMoneyByElement(element);
    var limit = GetControlMoney("HiddenLimitAmount");
    var masterRate = GetControlMoney("HiddenMasterRate");
    var transactionTypeId = GetControlNumber("HiddenTransactionTypeID");
    if (masterRate === 0 || limit === 0) return true;
    var transactionMessage = "mua/bán";
    
    if (transactionTypeId === 3 || transactionTypeId === 1)
    {
        transactionMessage = "mua";
    }
    else if (transactionTypeId === 4 || transactionTypeId === 2)
    {
        transactionMessage = "bán";
    }
    var minLimit = formatDigit((masterRate - limit));
    var maxLimit = formatDigit((masterRate + limit));
    if ((amount < masterRate - limit) || 
        (amount > masterRate + limit))
    {
        alertMessage("Giá " + transactionMessage + " vượt biên độ NHNN qui định, giá không được nhỏ hơn " +
            minLimit + " và không được phép lớn hơn " + maxLimit,
            undefined, undefined, function() {
            showError(element, true);
            focus(element);
        });
        return false;
    }

    showError(element, false);
    return true;
}
function BidTransaction() {
    var arrRequireFields = [
        "txtCapitalAmount", "txtRemainTime"
    ];
    if (validateInputArray(arrRequireFields) === false) {
        return false;
    }

    if (validateNumber(getControl("txtRemainTime"), 15, 1800) === false ||
        validateNumber(getControl("txtCapitalAmount"), 1, 1000000000000) === false ||
        ExceedLimit(getControl("txtCapitalAmount")) === false) {
        return false;
    }
    return true;
}

function InputCustomerInvoiceAmount() {
    var arrRequireFields = [
        "txtCustomerInvoiceAmount"
    ];
    if (validateInputArray(arrRequireFields) === false) {
        return false;
    }
    var invoiceAmountElement = getControl("txtCustomerInvoiceAmount");
    if (validateNumber(invoiceAmountElement, 1, 1000000000000) === false ||
        ExceedLimit(getControl("txtCustomerInvoiceAmount")) === false) {
        return false;
    }
   
    return true;
}
function InputBrokerage() {
    var arrRequireFields = [
        "txtBrokerage"
    ];
    if (validateInputArray(arrRequireFields) === false) {
        return false;
    }

    if (validateNumber(getControl("txtBrokerage"), 1, 1000000000000) === false) {
        return false;
    }
    var transactionTypeId = GetControlNumber("HiddenTransactionTypeID");
    var capitalAmount = GetControlMoney("HiddenCapitalAmount");
    var brokerAmount = GetControlMoney("txtBrokerage");
    if ((transactionTypeId === 3 || transactionTypeId === 1) && brokerAmount < capitalAmount) /*buy*/
    {
        alertMessage("Giá môi giới chiều mua chỉ được phép lớn hơn hoặc bằng (>=" +
            capitalAmount + " )",
            undefined, undefined, function () {
                showError(getControl("txtBrokerage"), true);
                focus(getControl("txtBrokerage"));
            });
        return false;
    }
    else if ((transactionTypeId === 4 || transactionTypeId === 2) && brokerAmount > capitalAmount) /*sell*/
    {
        alertMessage("Giá môi giới chiều bán chỉ được phép nhở hơn hoặc bằng giá vốn (<=" +
            capitalAmount + " )",
            undefined, undefined, function () {
                showError(getControl("txtBrokerage"), true);
                focus(getControl("txtBrokerage"));
            });
        return false;
    }
    return true;
}

function InvalidRequestEdit()
{
    var currentInvoiceAmount = GetControlMoney("HiddenInvoiceAmount");
    var invoiceAmount = GetControlMoney("txtCustomerInvoiceAmount");
    var transactionTypeId = GetControlNumber("HiddenTransactionTypeID");
    var transactionMessage = "mua/bán";
    var invalidEditMessage = "";
    var isFail = false;
    if ((transactionTypeId === 3 || transactionTypeId === 1) && (invoiceAmount > currentInvoiceAmount))
    {
        isFail = true;
        transactionMessage = "mua";
        invalidEditMessage = "Chỉ được phép nhỏ hơn " + currentInvoiceAmount;
    }
    else if ((transactionTypeId === 4 || transactionTypeId === 2) && (invoiceAmount < currentInvoiceAmount)) {
        isFail = true;
        transactionMessage = "bán";
        invalidEditMessage = "Chỉ được phép lớn hơn " + currentInvoiceAmount;
    }
    if (isFail === true) {
        alertMessage("Giá khách hàng (" + transactionMessage + ") không hợp lệ, " + invalidEditMessage,
            undefined, undefined, function () {
                showError(getControl("txtCustomerInvoiceAmount"), true);
            focus(getControl("txtCustomerInvoiceAmount"));
        });
        return false;
    }
    showError(getControl("txtCustomerInvoiceAmount"), false);
    return true;
}
function ValidateCustomerInfo() {
    var arrRequireFields = [
        "txtBranchName", "txtMarker", "calTransactionDate", "txtQuantityTransactionAmount", "txtCustomerIDNo",
        "txtCustomerFullname"
    ];
    var arrRequireRadOptions = ["ctCustomerType", "ctReasonTransaction"];
    if (validateInputArray(arrRequireFields) === false
        || validateRadOptionArray(arrRequireRadOptions) === false) {
        return false;
    }
    return true;
}
function RequestEdit() {
    var arrRequireFields = [
        "txtQuantityTransactionAmount","txtCustomerInvoiceAmount"
    ];
    if (validateInputArray(arrRequireFields) === false) {
        return false;
    }
    if (ValidateCustomerInfo() === false ||
        validateNumber(getControl("txtQuantityTransactionAmount"), 1, 1000000000000) === false ||
        validateNumber(getControl("txtCustomerInvoiceAmount"), 1, 1000000000000) === false ||
        InvalidRequestEdit() === false ||
        ExceedLimit(getControl("txtCustomerInvoiceAmount")) === false) {
        return false;
    }
    
    return true;
}
function processOnSelectPostBack(sender) {
    var isPostBack;
    var emptyValue = "";
    var currencyValue = getRadCombobox("ctExchangeCode").get_value();
    var transactionType = getRadCombobox("ctTransactionType").get_value();
    if (currencyValue !== emptyValue && transactionType !== emptyValue) {
        isPostBack = true;
    }
  

    if (isPostBack) {
        __doPostBack(sender.get_id(), '{\"Command\" : \"Select\"}');
    }
}
function BtnSubmition()
{
    var workflowStatusId = GetControlNumber("HiddenWorkflowStatus");
    switch (workflowStatusId)
    {
        case 0:
        case 5:
            return RequestTransactionValidate();
        case 2:
            return BidTransaction();
        case 4:
            return InputCustomerInvoiceAmount();
        case 9:
            return InputBrokerage();
        case 13:
            return RequestEdit();
        default:
            return true;

    }

}
function BtnReloadTransactionManagementGridView()
{
    var element = getControl("btnReload");
    if (typeof element !== "undefined" && element !== null) {
        element.click();
    }
    
}
function BtnReloadBidManagementGridView() {
    var element = getControl("btnInbox");
    if (typeof element !== "undefined" && element !== null) {
        element.click();
    }
}
function BtnReloadExchangeRate() {
    var element = getControl("btnReloadExchangeRate");
    if (typeof element !== "undefined" && element !== null) {
        element.click();
    }
}
function BtnExchangeRateRedirectPage() {
    var element = getControl("btnRedirectPage");
    if (typeof element !== "undefined" && element !== null) {
        element.click();
    }
}


