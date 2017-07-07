using System;
using DotNetNuke.UI.Skins.Controls;
using Modules.Disbursement.Business;
using Modules.Disbursement.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.Disbursement
{
    public partial class DisbursementRoom : DesktopModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            btnUpdate.Visible = true;
            tbRateLDR.Enabled = true;
            tbRate.Enabled = true;
            tbRoom.Enabled = true;
        }
        
        protected void History(object sender, EventArgs e)
        {
            string script = EditUrl("History", 1024, 768, false);
            RegisterScript(script);
        }
        protected void Update(object sender, EventArgs e)
        {
            string rateLdr = tbRateLDR.Text;
            string room = tbRoom.Text;
            string rate = tbRate.Text;

            if (string.IsNullOrEmpty(rateLdr) || string.IsNullOrEmpty(rateLdr))
            {
                ShowMessage("Tỷ lệ LDR không được để trống", ModuleMessage.ModuleMessageType.RedError);
                return;
            }
            double tmp;
            if (!double.TryParse(rateLdr, out tmp))
            {
                ShowMessage("Tỷ lệ LDR không phải là số", ModuleMessage.ModuleMessageType.RedError);
                return;
            }

            if (string.IsNullOrEmpty(room) || string.IsNullOrEmpty(room))
            {
                ShowMessage("Room không được để trống", ModuleMessage.ModuleMessageType.RedError);
                return;
            }
            if (string.IsNullOrEmpty(rate) || string.IsNullOrEmpty(rate))
            {
                ShowMessage("Tỷ lệ không được để trống", ModuleMessage.ModuleMessageType.RedError);
                return;
            }

            if (!double.TryParse(rate, out tmp))
            {
                ShowMessage("Tỷ lệ không phải là số", ModuleMessage.ModuleMessageType.RedError);
                return;
            }
            long milliseconds = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
            DisbursementRoomData roomData =
                new DisbursementRoomData
                {
                    ID = milliseconds + "",
                    RateLdr = rateLdr,
                    Room = room,
                    Rate = rate,
                    CreatedBy = UserInfo.Email,
                    CreatedAt = DateTime.Now.ToString(PatternEnum.DateTime)
                };
            string result = DisbursementRoomBusiness.Update(roomData);
            if (!string.IsNullOrEmpty(result))
            {
                ShowMessage("Cập nhật dữ liệu bị lỗi, vui lòng liên hệ admin", ModuleMessage.ModuleMessageType.RedError);
                return;
            }
            ShowMessage("Cập nhật room thành công", ModuleMessage.ModuleMessageType.GreenSuccess);
            btnUpdate.Visible = false;
            tbRateLDR.Enabled = false;
            tbRate.Enabled = false;
            tbRoom.Enabled = false;
        }
    }
}