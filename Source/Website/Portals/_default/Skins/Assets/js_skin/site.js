﻿var gloModule = window.gloModule || "";
var gloMaxLength = 1000;
var gloPostBackInterval;

$(document).ready(function()
{
    // Global error handler
    window.onerror = function(message)
    {
        log(message);
    };

    // Init page
    initialize();
});

function initialize()
{
    // Register Loading Panel
    addPageLoaded(function()
    {
        // Loading Panel
        injectLoading();
        addBeginRequest(showLoading);
        addEndRequest(function()
        {
            hideLoading();
            cleanUpAutoComplete();
        });
        addPostBackTrigger(function()
        {
            showLoading();
            waitPostBackComplete();
        });
    });
}

function addPageLoaded(fn, autoRefresh)
{
    if (typeof fn !== "function")
    {
        return;
    }

    $(document).ready(function()
    {
        fn();
    });

    if (autoRefresh)
    {
        addEndRequest(fn);
    }
}

function addBeginRequest(fn)
{
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function()
    {
        fn();
    });
}

function addEndRequest(fn)
{
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function()
    {
        fn();
    });
}

function addPostBackTrigger(fn)
{
    var array = Sys.WebForms.PageRequestManager.getInstance()._postBackControlClientIDs;
    for (var i = 0; i < array.length; i++)
    {
        $("#" + array[i]).click(fn);
    }
}

function waitPostBackComplete()
{
    gloPostBackInterval = setInterval(function()
    {
        if ($.cookie("PostBackComplete"))
        {
            clearInterval(gloPostBackInterval);
            $.removeCookie("PostBackComplete", { path: "/" });
            hideLoading();
        }
    }, 1000);
}

function alertMessage(message, title, buttonText, callback)
{
    try
    {
        if (typeof title === "undefined" || title === "")
        {
            title = "Thông báo";
        }
        if (typeof buttonText === "undefined" || buttonText === "")
        {
            buttonText = "Đồng ý";
        }

        $.dnnAlert({
            title: title,
            text: message,
            okText: buttonText,
            buttonOkClass: "btn btn-primary",
            width: "auto",
            height: "auto",
            callback: callback,
            close: callback
        });
    }
    catch (e)
    {
        log(e);
        alert(message);
    }
}

function confirmMessage(jquery, message, title, yesText, noText)
{
    if (typeof title === "undefined" || title === "")
    {
        title = "Xác nhận";
    }
    if (typeof yesText === "undefined" || yesText === "")
    {
        yesText = "Đồng ý";
    }
    if (typeof noText === "undefined" || noText === "")
    {
        noText = "Hủy";
    }

    // Clean all click handler
    $(jquery).unbind("click");

    var tagName = $(jquery).prop("tagName") + "";
    var isButton = tagName.toLowerCase() === "input";
    $(jquery).dnnConfirm({
        title: title,
        text: message,
        yesText: yesText,
        noText: noText,
        buttonYesClass: "btn btn-primary",
        buttonNoClass: "btn btn-default",
        draggable: true,
        isButton: isButton
    });
}

function getClientID(id)
{
    return gloModule + id;
}

function getControl(id)
{
    return document.getElementById(getClientID(id));
}

function getJQueryControl(id)
{
    return $(getControl(id));
}

function injectLoading()
{
    var loadingPanel = "" +
        "<div class='dnnPanelLoading'>" +
        "   <div class='ui-widget-overlay ui-front'></div>" +
        "   <div class='dnnLoading'><i class='fa fa-spinner fa-spin'></i></div>" +
        "</div>";
    $("#Body").append(loadingPanel);
}

function showLoading()
{
    $(".dnnPanelLoading").show();
}

function hideLoading()
{
    $(".dnnPanelLoading").hide();
}

function replaceAll(source, oldString, newString)
{
    var index = source.indexOf(oldString);
    while (index !== -1)
    {
        source = source.replace(oldString, newString);
        index = source.indexOf(oldString);
    }
    return source;
}

function formatDigit(value)
{
    value = value.toString();
    var result = "";
    var count = 0;
    for (var i = (value.length - 1); i >= 0; i--)
    {
        count++;
        if (count === 4)
        {
            count = 1;
            result = value.charAt(i) + "," + result;
        }
        else
        {
            result = value.charAt(i) + result;
        }
    }
    return result;
}

//format currrency
function formatCurrency(element)
{
    var value = replaceAll(element.value, ",", "");
    if (value.length === 0)
    {
        return;
    }
    if (isNaN(value))
    {
        element.value = "";
        return;
    }

    value = parseInt(value, 10);
    element.value = formatDigit(value.toString());
}


// Validate Data
function isNullOrEmpty(value)
{
    return value === null || typeof value === "undefined" || value.trim().length === 0;
}

function isInvalidEmail(email)
{
    var pattern =
        /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(email) === false;
}

function isInvalidNumber(value, min, max)
{
    value = parseInt(value, 10);
    if (isNaN(value))
    {
        value = 0;
    }
    return value < min || value > max;
}

function isInvalidLength(value, min, max)
{
    var length = value.trim().length;
    if (typeof min !== "number" || min === -1)
    {
        min = 0;
    }
    if (typeof max !== "number" || max === -1)
    {
        max = gloMaxLength;
    }
    return length < min || length > max;
}

function isInvalidOption(option, value)
{
    return option.trim() === value;
}

function showError(element, isHasError)
{
    var container = $(element).parent();
    var icon = container.find("i.fa");
    if (isHasError)
    {
        container.addClass("has-error");
        icon.removeClass("fa-check").addClass("fa-remove");
    }
    else
    {
        container.removeClass("has-error");
        icon.removeClass("fa-remove").addClass("fa-check");
    }
}

function validateInputArray(array)
{
    for (var i = 0; i < array.length; i++)
    {
        if (validateInput(getControl(array[i])) === false)
        {
            return false;
        }
    }
    return true;
}

function validateInput(element)
{
    try
    {
        var message;
        if (isNullOrEmpty(element.value))
        {
            message = "Vui lòng nhập vào <b>" + element.placeholder + "</b>";
            alertMessage(message, undefined, undefined, function()
            {
                showError(element, true);
                element.focus();
            });
            return false;
        }
        if (isInvalidLength(element.value, element.minLength, element.maxLength))
        {            
            message = "Độ dài <b>" + element.placeholder + "</b> không phù hợp." +
                " Min = <b>" + (element.minLength || 0) + "</b>," +
                " Max = <b>" + (element.maxLength || gloMaxLength) + "</b>";
            alertMessage(message, undefined, undefined, function()
            {
                showError(element, true);
                element.focus();
            });
            return false;
        }

        showError(element, false);
        return true;
    }
    catch (e)
    {
        log(e);
        return false;
    }
}

function validateOptionArray(array)
{
    for (var i = 0; i < array.length; i++)
    {
        if (validateOption(getControl(array[i])) === false)
        {
            return false;
        }
    }
    return true;
}

function validateOption(element)
{
    try
    {
        if (isInvalidOption(element.value, ""))
        {
            var message = "Vui lòng chọn <b>" + element.attributes.placeholder.value + "</b>";
            alertMessage(message, undefined, undefined, function()
            {
                showError(element, true);
                element.focus();
            });
            return false;
        }

        showError(element, false);
        return true;
    }
    catch (e)
    {
        log(e);
        return false;
    }
}

function validateNumber(element, min, max)
{
    var value = replaceAll(element.value, ",", "");
    if (value === "" || isNaN(value) || isInvalidNumber(value, min, max))
    {
        var message = "Vui lòng nhập vào " + element.placeholder + " một số lớn hơn bằng " + formatDigit(min) +
            " và bé hơn bằng " + formatDigit(max);
        alertMessage(message, undefined, undefined, function()
        {
            showError(element, true);
            element.focus();
        });
        return false;
    }

    showError(element, false);
    return true;
}

function validateEmail(element)
{
    if (isInvalidEmail(element.value))
    {
        alertMessage("Email không hợp lệ", undefined, undefined, function()
        {
            showError(element, true);
            element.focus();
        });
        return false;
    }

    showError(element, false);
    return true;
}

function validateFileExtension(element, arrayExtensions)
{
    var fileName = element.value;
    if (isNullOrEmpty(fileName) === false)
    {
        var array = fileName.split(".");
        var extension = array[array.length - 1].toLowerCase();
        for (var i = 0; i < arrayExtensions.length; i++)
        {
            if (extension === arrayExtensions[i].toLowerCase())
            {
                return true;
            }
        }
    }

    alertMessage("Hệ thống chỉ hỗ trợ định dạng file " + arrayExtensions.join(", ") + ".", undefined, undefined,
        function()
        {
            hideLoading();
            showError(element, true);
            element.focus();
        });
    return false;
}

function log(e)
{
    if (console)
    {
        console.log(e);
    }
}

function fixClientIDModeStatic()
{
    if (Sys.WebForms.PageRequestManager)
    {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm._findNearestElement = function(uniqueId)
        {
            while (uniqueId.length > 0)
            {
                var clientId = this._uniqueIDToClientID(uniqueId);
                var element = document.getElementById(clientId);
                if (element)
                {
                    return element;
                }
                var indexOfFirstDollar = uniqueId.indexOf("$", 1);
                if (indexOfFirstDollar === -1)
                {
                    return null;
                }
                uniqueId = uniqueId.substring(indexOfFirstDollar + 1, uniqueId.length);
            }
            return null;
        };
    }
}

function cleanUpAutoComplete()
{
    // Query list reserve
    var array = new Array();
    $("input.ui-autocomplete-input").each(function ()
    {
        array.push($(this).data("ui-autocomplete").menu.element.attr("id"));
    });

    // Clean up list duplicate
    $("ul.ui-autocomplete").each(function () {
        if (array.indexOf(this.id) === -1)
        {
            this.remove();
        }
    });
}