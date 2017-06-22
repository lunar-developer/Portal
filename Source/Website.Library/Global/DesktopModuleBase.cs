﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Internal;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins;
using DotNetNuke.UI.Skins.Controls;
using Website.Library.Enum;

namespace Website.Library.Global
{
    public class DesktopModuleBase : PortalModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string script = $"gloModule = 'dnn_ctr{ModuleId}_{GetType().BaseType?.Name}_'";
            RegisterScript(script);
        }


        protected void AutoWire()
        {
            foreach (TextBox control in FindControls<TextBox>())
            {
                control.Attributes.Add("placeholder", GetResource($"{control.ID.Replace("txt", "lbl")}.Help"));
            }
        }


        protected string GetResource(string key)
        {
            return Localization.GetString(key, LocalResourceFile);
        }

        public static string GetExceptionsResource(string key)
        {
            return Localization.GetString(key, Localization.ExceptionsResourceFile);
        }

        public static string GetGlobalResource(string key)
        {
            return Localization.GetString(key, Localization.GlobalResourceFile);
        }

        public static string GetSharedResource(string key)
        {
            return Localization.GetString(key, Localization.SharedResourceFile);
        }

        protected string GetAlertScript(string message, string title = null, bool isUseResource = false,
            string script = "")
        {
            string callback = $"function() {{ {script} }}";
            return $@"
                alertMessage(
                    '{(isUseResource ? GetResource(message) : message)}',
			        '{
                        (string.IsNullOrWhiteSpace(title)
                            ? GetSharedResource("System.Text")
                            : (isUseResource ? GetResource(title) : title))
                    }',
                    '{GetSharedResource("Ok.Text")}',
                    {callback});";
        }

        protected string GetConfirmScript(string jquery, string message, bool isUseResource = false)
        {
            return $@"
                confirmMessage(
                    '{jquery}',
                    '{(isUseResource ? GetResource(message) : message)}',
			        '{GetSharedResource("Confirm.Text")}',
                    '{GetSharedResource("Yes.Text")}',
			        '{GetSharedResource("No.Text")}');";
        }

        protected string GetPopUpMaximumScript()
        {
            return ";$('.dnnToggleMax').click();";
        }

        protected string GetAutoPostScript(string url, Dictionary<string, string> dictionary = null, bool isOpenNewTab = true)
        {
            string id = Guid.NewGuid().ToString(PatternEnum.GuidDigits);
            StringBuilder form = new StringBuilder();
            form.Append(
                $"<form id=\"{id}\" action=\"{url}\" method=\"post\" target=\"{(isOpenNewTab ? "_blank" : "_self")}\">");
            if (dictionary != null)
            {
                foreach (KeyValuePair<string, string> pair in dictionary)
                {
                    form.Append($"<input type=\"hidden\" name=\"{pair.Key}\" value=\"{pair.Value}\" />");
                }
            }
            form.Append("</form>");
            return $"$('body').append('{form}'); $('#{id}').submit().remove();";
        }

        protected string GetWindowOpenScript(string url, Dictionary<string, string> dictionary,
            bool isOpenNewTab = true)
        {
            string parameters = dictionary == null || dictionary.Count == 0
                ? string.Empty
                : "?" + string.Join("&", dictionary.Select(pair => $"{pair.Key}={pair.Value}"));
            return $"window.open(\"{url}{parameters}\", \"{(isOpenNewTab ? "_blank" : "_self")}\")";
        }


        protected void ExportToExcel(DataTable dtSource, string fileName = "Export")
        {
            byte[] bytes = FunctionBase.ExportToExcel(dtSource);
            Response.Clear();
            Response.AddHeader("Set-Cookie", "CookieName=CookieValue; path=/;");
            Response.SetCookie(new HttpCookie("PostBackComplete") { Value = "true", HttpOnly = false });
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("content-disposition", $"attachment; filename={fileName}.xls");
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.Close();
            Response.End();
        }

        protected string EditUrl(string controlKey, int width, int height, bool isReload, string keyName = "",
            string keyValue = "",
            string closingUrl = null, params string[] additionalParameters)
        {
            string moduleIdParam = $"mid={ModuleId}";
            string[] parameters;
            if (string.IsNullOrEmpty(keyName) == false
                && string.IsNullOrEmpty(keyValue) == false)
            {
                parameters = new string[2 + additionalParameters.Length];
                parameters[0] = moduleIdParam;
                parameters[1] = $"{keyName}={keyValue}";
                Array.Copy(additionalParameters, 0, parameters, 2, additionalParameters.Length);
            }
            else
            {
                parameters = new string[1 + additionalParameters.Length];
                parameters[0] = moduleIdParam;
                Array.Copy(additionalParameters, 0, parameters, 1, additionalParameters.Length);
            }

            int tabId = PortalSettings.ActiveTab.TabID;
            var isSuperTab = TestableGlobals.Instance.IsHostTab(tabId);
            var settings = PortalController.Instance.GetCurrentPortalSettings();
            var language = Thread.CurrentThread.CurrentCulture.Name;
            var url = TestableGlobals.Instance.NavigateURL(tabId, isSuperTab, settings, controlKey, language,
                Globals.glbDefaultPage, parameters);

            return UrlUtils.PopUpUrl(url, null, PortalSettings, false, false, height, width, isReload, closingUrl)
                + GetPopUpMaximumScript();
        }

        protected string EditUrl(string url, int width, int height, bool isReload,
            Dictionary<string, string> dictionary,
            string closingUrl = null)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                list.Add($"{pair.Key}={pair.Value}");
            }
            url = $"{url}?{string.Join("&", list.ToArray())}";
            return UrlUtils.PopUpUrl(url, null, PortalSettings, false, false, height, width, isReload, closingUrl)
                + GetPopUpMaximumScript();
        }


        protected T FindControl<T>() where T : Control
        {
            List<T> list = FindControls<T>(false);
            return list.Count > 0 ? list[0] : null;
        }

        protected List<T> FindControls<T>(bool isRepeatUntilEnd = true) where T : Control
        {
            List<T> listResult = new List<T>();
            Queue<Control> queueControl = new Queue<Control>();
            queueControl.Enqueue(Control);

            while (queueControl.Count > 0)
            {
                Control control = queueControl.Dequeue();
                if (control.GetType() == typeof(T))
                {
                    listResult.Add(control as T);
                    if (isRepeatUntilEnd == false)
                    {
                        break;
                    }
                }
                foreach (Control item in control.Controls)
                {
                    queueControl.Enqueue(item);
                }
            }
            return listResult;
        }


        protected bool IsInRole(string roleName)
        {
            return UserInfo.UserID > 0 && UserInfo.IsInRole(roleName);
        }

        protected bool IsInRole(string roleName, int userID)
        {
            return FunctionBase.IsInRole(roleName, userID);
        }


        protected void ShowMessage(string message,
            ModuleMessage.ModuleMessageType messageType = ModuleMessage.ModuleMessageType.BlueInfo)
        {
            PlaceHolder phMessage = (PlaceHolder) FindControl("phMessage");
            if (phMessage == null)
            {
                ShowAlertDialog(message);
            }
            else
            {
                ModuleMessage moduleMessage =
                    Skin.GetModuleMessageControl(string.Empty, message, messageType, string.Empty);
                phMessage.Controls.Add(moduleMessage);
                RegisterScript("rollToTop()");
            }
        }

        protected void ShowAlertDialog(string message, string title = null, bool isUseResource = false)
        {
            RegisterScript(GetAlertScript(message, title, isUseResource));
        }

        protected void ShowException(Exception exception)
        {
            FunctionBase.LogError(exception);
            ShowMessage(exception.ToString(), ModuleMessage.ModuleMessageType.RedError);
        }


        protected void RegisterScript(string script)
        {
            string key = Guid.NewGuid().ToString(PatternEnum.GuidDigits);
            script = $@"
                $(document).ready(function()                
                {{
                    {script}
                }});
                ";

            ScriptManager.RegisterClientScriptBlock(Page, GetType(), key, script, true);
        }

        protected void RegisterStartupScript(string script, bool autoRefresh = true)
        {
            script = $@"
                addPageLoaded(function()
                {{
                    {script}
                }}, {(autoRefresh ? "true" : "false")});";

            RegisterScript(script);
        }

        protected void RegisterConfirmDialog(Control control, string message, bool isUseResource = false)
        {
            RegisterConfirmDialog($"#{control.ClientID}", message, isUseResource);
        }

        protected void RegisterConfirmDialog(string jquery, string message, bool isUseResource = false)
        {
            RegisterStartupScript(GetConfirmScript(jquery, message, isUseResource));
        }

        protected string GetCloseScript()
        {
            return "$(parent.document).find('button.ui-button:visible').click();";
        }
    }
}