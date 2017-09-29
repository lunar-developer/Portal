using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using DotNetNuke.Services.Mail;
using Website.Library.Business;
using Website.Library.Enum;

namespace Website.Library.Global
{
    public static class MailBase
    {
        private static readonly string EmailAddress;
        private static readonly string EmailTemplate;
        private static readonly string EmailEnvelope;
        private static readonly string Logo;

        static MailBase()
        {
            // Folder
            string assetsFolder =
                FunctionBase.GetConfiguration(ConfigEnum.SiteFolder) +
                FunctionBase.GetConfiguration(ConfigEnum.SiteAssetsFolder);
            string imageFolder = $"{assetsFolder}images/";
            string templateFolder = $"{assetsFolder}templates/";

            // Read Settings
            EmailAddress = FunctionBase.GetConfiguration(ConfigEnum.EmailAddress);
            EmailTemplate = File.ReadAllText($"{templateFolder}EmailTemplate.html");
            EmailEnvelope = ImageBusiness.GetImageFromFile($"{imageFolder}envelope.png");
            Logo = ImageBusiness.GetImageFromFile($"{imageFolder}logo.png");
        }


        public static void SendEmail(
            string toAddress,
            string subject,
            string body,
            List<Attachment> listAttachments = null,
            bool isUseTemplate = true)
        {
            SendEmail(EmailAddress, toAddress, subject, body, listAttachments, isUseTemplate);
        }

        public static void SendEmail(
            string fromAddress,
            string toAddress,
            string subject,
            string body,
            List<Attachment> listAttachments = null,
            bool isUseTemplate = true)
        {
            if (isUseTemplate)
            {
                if (listAttachments == null)
                {
                    listAttachments = new List<Attachment>();
                }
                Attachment emailEnvelope = CreateAttachment(EmailEnvelope, "Envelope");
                Attachment logo = CreateAttachment(Logo, "Logo");
                listAttachments.Add(emailEnvelope);
                listAttachments.Add(logo);
                body = EmailTemplate
                    .Replace("@Envelope", emailEnvelope.ContentId)
                    .Replace("@Logo", logo.ContentId)
                    .Replace("@Content", body);
            }
            Task.Run(() => Mail.SendEmail(fromAddress, fromAddress, toAddress, subject, body, listAttachments));
        }

        private static Attachment CreateAttachment(string imageBase64, string name)
        {
            byte[] byteArray = Convert.FromBase64String(imageBase64);
            MemoryStream stream = new MemoryStream(byteArray);
            Attachment attachment = new Attachment(stream, name, MediaTypeNames.Image.Jpeg)
            {
                ContentId = Guid.NewGuid().ToString(PatternEnum.GuidDigits)
            };
            return attachment;
        }
    }
}