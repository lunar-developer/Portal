using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using Modules.EmployeeManagement.Business;
using Website.Library.Business;
using Website.Library.Enum;
using Website.Library.Global;

namespace Modules.EmployeeManagement.Global
{
    public class EmployeeManagementModuleBase : DesktopModuleBase
    {
        private const int ScaleMaxHeight = 250;
        private const int ScaleMaxWidth = 150;

        private static readonly string TemplateFolder = FunctionBase.GetConfiguration("EM_TemplateFolder");

        private static readonly string AssetFolder =
            FunctionBase.GetConfiguration(ConfigEnum.SiteFolder) + TemplateFolder;

        private static readonly string TemplateUrl =
            FunctionBase.GetAbsoluteUrl($"/{TemplateFolder}{FunctionBase.GetConfiguration("EM_UploadTemplate")}");

        protected string LinkTemplate => TemplateUrl;

        protected static readonly string VCardTemplate;

        protected static string DefaultMaleImage =
            ImageBusiness.GetImageFromFile($"{AssetFolder}{FunctionBase.GetConfiguration("EM_MaleImage")}");

        protected static string DefaultFemaleImage =
            ImageBusiness.GetImageFromFile($"{AssetFolder}{FunctionBase.GetConfiguration("EM_FemaleImage")}");


        static EmployeeManagementModuleBase()
        {
            string fileTemplate = AssetFolder + FunctionBase.GetConfiguration("EM_VCardTemplate");
            VCardTemplate = FunctionBase.ReadFile(fileTemplate);
        }

        protected string GenerateQRCode(string name, string role, string phoneExtend, string phone,
            string officeAddress, string email)
        {
            string data = string.Format(VCardTemplate, name, role, phoneExtend, phone, officeAddress, email);
            return ImageBusiness.GenerateQRCode(data, 2);
        }

        protected string GetImageBase64String(Stream stream)
        {
            using (Bitmap postedImage = new Bitmap(stream))
            {
                Bitmap image = ImageBusiness.ScaleImage(postedImage, ScaleMaxWidth, ScaleMaxHeight);
                string imageBase64 = ImageBusiness.ConvertImageToBase64(image, ImageFormat.Jpeg);
                image.Dispose();
                return imageBase64;
            }
        }

        protected void UpdateContactQRCode(Dictionary<string, string> employeeQRDictionary)
        {
            new Thread(() =>
                {
                    try
                    {
                        EmployeeBusiness.UpdateEmployeeQRCode(employeeQRDictionary);
                    }
                    catch (Exception exception)
                    {
                        FunctionBase.LogError(exception);
                    }
                }
            ).Start();
        }
    }
}